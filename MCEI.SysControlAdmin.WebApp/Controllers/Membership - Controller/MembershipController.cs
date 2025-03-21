﻿#region REFERENCIAS
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.BL.Membership___BL;
using MCEI.SysControlAdmin.EN.Membership___EN;
using MCEI.SysControlAdmin.EN.Role___EN;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;


#endregion

namespace MCEI.SysControlAdmin.WebApp.Controllers.Membership___Controller
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Desarrollador, Administrador, Digitador")]
    public class MembershipController : Controller
    {
        // Creamos las instancias para acceder a los metodos
        MembershipBL membershipBL = new MembershipBL();

        #region METODO PARA CREAR
        // Accion Para Mostrar La Vista De Crear
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public IActionResult CreateMembership()
        {
            ViewBag.Error = "";
            return View();
        }

        // Metodo que recibe y envia a la base de datos
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMembership(Membership membership, IFormFile imagen)
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

                    membership.ImageData = imagenData; // Asigna el array de bytes al campo de la imagen en tu modelo Membership
                }
                membership.DateCreated = DateTime.Now;
                membership.DateModification = DateTime.Now;
                int result = await membershipBL.CreateAsync(membership);
                TempData["SuccessMessageCreate"] = "Membresia Agregada Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(membership);
            }
        }
        #endregion

        #region METODO PARA MOSTRAR INDEX
        // Accion Para Mostrar La Vista Index
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public async Task<IActionResult> Index(Membership membership = null!)
        {
            if (membership == null)
                membership = new Membership();

            var memberships = await membershipBL.SearchAsync(membership);
            return View(memberships);
        }
        #endregion

        #region METODO PARA MODIFICAR
        // Acción que muestra la vista de modificar
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public async Task<IActionResult> EditMembership(int id)
        {
            try
            {
                Membership membership = await membershipBL.GetByIdAsync(new Membership { Id = id });
                if (membership == null)
                {
                    return NotFound();
                }
                // Convertir el array de bytes en imagen para mostrar en la vista
                if (membership.ImageData != null && membership.ImageData.Length > 0)
                {
                    ViewBag.ImageUrl = Convert.ToBase64String(membership.ImageData);
                }
                return View(membership);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(); // Devolver la vista sin ningún objeto Membership
            }
        }

        // Acción que recibe los datos del formulario para ser enviados a la base de datos
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMembership(int id, Membership membership, IFormFile imagen)
        {
            try
            {
                if (id != membership.Id)
                {
                    return BadRequest();
                }
                if (imagen != null && imagen.Length > 0) // Verificar si se ha subido una nueva imagen
                {
                    byte[] imagenData = null!;
                    using (var memoryStream = new MemoryStream())
                    {
                        await imagen.CopyToAsync(memoryStream);
                        imagenData = memoryStream.ToArray();
                    }
                    membership.ImageData = imagenData; // Asignar el array de bytes de la nueva imagen a la entidad Membership
                }
                else
                {
                    // Si no se proporciona una nueva imagen, se conserva la imagen existente
                    Membership existingMembership = await membershipBL.GetByIdAsync(new Membership { Id = id });
                    membership.ImageData = existingMembership.ImageData;
                }
                membership.DateModification = DateTime.Now;
                int result = await membershipBL.UpdateAsync(membership);
                TempData["SuccessMessageUpdate"] = "Miembro Modificado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(membership); // Devolver la vista con el objeto Membership para que el usuario pueda corregir los datos
            }
        }
        #endregion
    }
}
