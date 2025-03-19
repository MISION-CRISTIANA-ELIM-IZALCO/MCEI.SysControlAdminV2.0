﻿#region REFERENCIAS
// Referencias Necesarios Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.BL.Role___BL;
using MCEI.SysControlAdmin.BL.User___BL;
using MCEI.SysControlAdmin.EN.Role___EN;
using MCEI.SysControlAdmin.EN.User___EN;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


#endregion

namespace MCEI.SysControlAdmin.WebApp.Controllers.User___Controller
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Desarrollador, Administrador")]
    public class UserController : Controller
    {
        // Creamos Las Instancias Para Acceder a Los Metodos
        UserBL userBL = new UserBL();
        RoleBL roleBL = new RoleBL();

        #region METODO PARA GUARDAR
        // Accion Que Muestra El Formulario
        [Authorize(Roles = "Desarrollador, Administrador")]
        public async Task<IActionResult> Create()
        {
            var roles = await roleBL.GetAllAsync();
            ViewBag.Roles = roles;
            return View();
        }

        // Accion Que Recibe Los Datos y Los Envia a La Base De Datos
        [Authorize(Roles = "Desarrollador, Administrador")]
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
        [Authorize(Roles = "Desarrollador, Administrador")]
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
        [Authorize(Roles = "Desarrollador, Administrador")]
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
        [Authorize(Roles = "Desarrollador, Administrador")]
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

        #region METODO PARA DETALLES
        // Acción que muestra los detalles de un registro
        [Authorize(Roles = "Desarrollador, Administrador")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                // Obtener el usuario por ID
                var user = await userBL.GetByIdAsync(new User { Id = id });
                if (user == null)
                {
                    return NotFound();
                }
                // Verificar si el usuario tiene una imagen y convertirla para mostrar en la vista
                if (user.ImageData != null && user.ImageData.Length > 0)
                {
                    ViewBag.ImageUrl = Convert.ToBase64String(user.ImageData);
                }
                user.Role = await roleBL.GetByIdAsync(new Role { Id = user.IdRole });

                return View(user); // Retornar los detalles del usuario a la vista
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(); // Devolver la vista sin ningún objeto en caso de error
            }
        }
        #endregion

        #region METODO PARA ELIMINAR
        // Acción que muestra el formulario de eliminación
        [Authorize(Roles = "Desarrollador, Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Obtener el usuario por ID
                var user = await userBL.GetByIdAsync(new User { Id = id });
                if (user == null)
                {
                    return NotFound();
                }
                if (user.ImageData != null && user.ImageData.Length > 0)
                {
                    ViewBag.ImageUrl = Convert.ToBase64String(user.ImageData);
                }
                user.Role = await roleBL.GetByIdAsync(new Role { Id = user.IdRole });
                ViewBag.Roles = await roleBL.GetAllAsync();

                return View(user);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(); // Devolver la vista sin ningún objeto en caso de error
            }
        }

        // Acción que recibe los datos del formulario para ser eliminados en la base de datos
        [Authorize(Roles = "Desarrollador, Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, User user)
        {
            try
            {
                // Obtener el usuario por ID para asegurarse de que existe antes de eliminar
                var userDb = await userBL.GetByIdAsync(new User { Id = id });
                if (userDb == null)
                {
                    return NotFound();
                }
                int result = await userBL.DeleteAsync(userDb);
                TempData["SuccessMessageDelete"] = "Usuario Eliminado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                // Volver a cargar el usuario en caso de error
                var userDb = await userBL.GetByIdAsync(new User { Id = id });
                if (userDb == null)
                {
                    userDb = new User();
                }
                // Obtener el rol del usuario en caso de que exista
                if (userDb.Id > 0)
                {
                    userDb.Role = await roleBL.GetByIdAsync(new Role { Id = userDb.IdRole });
                }
                ViewBag.Roles = await roleBL.GetAllAsync();

                return View(userDb); // Devolver la vista con los datos del usuario para que pueda corregir o revisar
            }
        }
        #endregion

        #region METODO DE INICIO DE SESION Y CERRAR SESION (LOGIN, LOGOUT)
        // Accion Que Muestra El Formulario
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null!)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ViewBag.Url = returnUrl;
            ViewBag.Error = "";
            return View();
        }

        // Accion Que Ejecuta La Autenticacion Del Usuario
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user, string returnUrl = null!)
        {
            try
            {
                var userDb = await userBL.LoginAsync(user);
                if (userDb != null && userDb.Id > 0 && userDb.Email == user.Email)
                {
                    userDb.Role = await roleBL.GetByIdAsync(new Role { Id = userDb.IdRole });
                    var claims = new[] { new Claim(ClaimTypes.Name, userDb.Email), new Claim(ClaimTypes.Role, userDb.Role.Name) };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                }
                else
                    throw new Exception("Credenciales Incorrectas, Vuelve a Intentarlo");

                if (!string.IsNullOrWhiteSpace(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                ViewBag.Url = returnUrl;
                ViewBag.Error = e.Message;
                return View(new User { Email = user.Email });
            }
        }

        // Accion Que Permite Cerrar La Sesion
        [AllowAnonymous]
        public async Task<IActionResult> Logout(string returnUrl = null!)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }
        #endregion
    }
}
