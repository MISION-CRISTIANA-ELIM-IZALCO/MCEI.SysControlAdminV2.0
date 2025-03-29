#region REFERENCIAS
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.BL.Privilege___BL;
using MCEI.SysControlAdmin.EN.Privilege___EN;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


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
        [Authorize(Roles = "Desarrollador, Administrador. Digitador")]
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
                privilege.DateCreated = DateTime.Now;
                privilege.DateModification = DateTime.Now;
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
    }
}
