#region REFERENCIAS
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.BL.Privilege___BL;
using MCEI.SysControlAdmin.EN.Privilege___EN;
using MCEI.SysControlAdmin.EN.Role___EN;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using MCEI.SysControlAdmin.Core.Utils;

#endregion

namespace MCEI.SysControlAdmin.WebApp.Controllers.Privilege___Controller
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Desarrollador, Administrador, Digitador")]
    public class PrivilegeController : Controller
    {
        // Creamos las intancias para accerder a los metodos
        PrivilegeBL privilegeBL = new PrivilegeBL();

        #region METODO PARA GUARDAR
        // Metodo Para Mostrar La Vista Guardar
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public IActionResult CreatePrivilege()
        {
            ViewBag.Error = "";
            return View();
        }

        // Metodo que recibe y envia a la base de datos
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePrivilege(Privilege privilege)
        {
            try
            {
                privilege.Status = 1;
                privilege.DateCreated = DateTime.Now.GetFechaZonaHoraria();
                privilege.DateModification = DateTime.Now.GetFechaZonaHoraria();
                int result = await privilegeBL.CreateAsync(privilege);
                TempData["SuccessMessageCreate"] = "Privilegio Agregado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(privilege);
            }
        }
        #endregion

        #region METODO PARA INDEX
        // Metodo para mostrar la vista Index
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public async Task<IActionResult> Index(Privilege privilege = null!)
        {
            if (privilege == null)
                privilege = new Privilege();

            var privileges = await privilegeBL.SearchAsync(privilege);
            return View(privileges);
        }
        #endregion

        #region METODO PARA MODIFICAR
        // Metodo para mostar la vista de modificar
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public async Task<IActionResult> EditPrivilege(int id)
        {
            var privilege = await privilegeBL.GetByIdAsync(new Privilege { Id = id });
            ViewBag.Error = "";
            return View(privilege);
        }

        // Metodo Que Recibe y Envia a La Base De Datos
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPrivilege(Privilege privilege)
        {
            try
            {
                privilege.DateModification = DateTime.Now.GetFechaZonaHoraria();
                int result = await privilegeBL.UpdateAsync(privilege);
                TempData["SuccessMessageUpdate"] = "Privilegio Modificado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(privilege);
            }
        }
        #endregion

        #region METODO PARA ELIMINAR
        // Metodo para mostrar la vista de eliminar
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public async Task<IActionResult> DeletePrivilege(int id)
        {
            var privilege = await privilegeBL.GetByIdAsync(new Privilege { Id = id });
            ViewBag.Error = "";
            return View(privilege);
        }

        // Metodo que recibe y envia a la base de datos
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePrivilege(int id, Privilege privilege)
        {
            try
            {
                int result = await privilegeBL.DeleteAsync(privilege);
                TempData["SuccessMessageDelete"] = "Privilegio Eliminado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(privilege);
            }
        }
        #endregion
    }
}
