﻿#region REFERENCIAS
// Referencias Necesarios Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.BL.Role___BL;
using MCEI.SysControlAdmin.BL.User___BL;
using MCEI.SysControlAdmin.EN.Role___EN;
using MCEI.SysControlAdmin.EN.User___EN;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


#endregion

namespace MCEI.SysControlAdmin.WebApp.Controllers.User___Controller
{
    public class UserController : Controller
    {
        // Creamos Las Instancias Para Acceder a Los Metodos
        UserBL userBL = new UserBL();
        RoleBL roleBL = new RoleBL();

        #region METODO PARA GUARDAR
        // Accion Que Muestra El Formulario
        public async Task<IActionResult> Create()
        {
            var roles = await roleBL.GetAllAsync();
            ViewBag.Roles = roles;
            return View();
        }

        // Accion Que Recibe Los Datos y Los Envia a La Base De Datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user, IFormFile imagen)
        {
            try
            {
                // Mapeo de img a ArrayByte
                if (imagen != null && imagen.Length > 0)
                {
                    byte[] imagenData = null!;
                    using (var memoryStream = new MemoryStream())
                    {
                        await imagen.CopyToAsync(memoryStream);
                        imagenData = memoryStream.ToArray();
                    }

                    user.ImageData = imagenData; // Asigna el array de bytes al campo de la imagen en tu modelo Membership
                }
                user.DateCreated = DateTime.Now;
                user.DateModification = DateTime.Now;
                int result = await userBL.CreateAsync(user);
                TempData["SuccessMessageCreate"] = "Usuario Agregado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                ViewBag.Roles = await roleBL.GetAllAsync();
                return View(user);
            }
        }
        #endregion

        #region METODO PARA INDEX
        // Metodo Para Mostrar La Vista Index
        public async Task<IActionResult> Index(User user = null!)
        {
            if (user == null)
                user = new User();

            var users = await userBL.SearchIncludeRoleAsync(user);
            var roles = await roleBL.GetAllAsync();

            ViewBag.Roles = roles;

            return View(users);
        }
        #endregion

        #region METODO PARA MODIFICAR
        // Acción que muestra el formulario
        public async Task<IActionResult> Edit(int id)
        {
            var user = await userBL.GetByIdAsync(new User { Id = id });
            user.Role = await roleBL.GetByIdAsync(new Role { Id = user.Id });

            // Convertir el array de bytes en imagen para mostrar en la vista (si la imagen existe)
            if (user.ImageData != null && user.ImageData.Length > 0)
            {
                ViewBag.ImageUrl = Convert.ToBase64String(user.ImageData);
            }
            ViewBag.Roles = await roleBL.GetAllAsync();
            return View(user);
        }

        // Acción que recibe los datos del formulario y los envía a la base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user, IFormFile imagen)
        {
            try
            {
                // Verificar que el id coincida con el usuario que se está modificando
                if (id != user.Id)
                {
                    return BadRequest();
                }
                // Si se ha subido una nueva imagen, actualizar el campo de imagen
                if (imagen != null && imagen.Length > 0)
                {
                    byte[] imagenData = null!;
                    using (var memoryStream = new MemoryStream())
                    {
                        await imagen.CopyToAsync(memoryStream);
                        imagenData = memoryStream.ToArray();
                    }
                    user.ImageData = imagenData; // Asignar el array de bytes de la nueva imagen al objeto User
                }
                else
                {
                    // Si no se proporciona una nueva imagen, mantener la imagen existente
                    User existingUser = await userBL.GetByIdAsync(new User { Id = id });
                    user.ImageData = existingUser.ImageData;
                }

                // Actualizar la fecha de modificación
                user.DateModification = DateTime.Now;
                int result = await userBL.UpdateAsync(user);
                TempData["SuccessMessageUpdate"] = "Usuario Modificado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                ViewBag.Roles = await roleBL.GetAllAsync();
                return View(user);
            }
        }
        #endregion
    }
}
