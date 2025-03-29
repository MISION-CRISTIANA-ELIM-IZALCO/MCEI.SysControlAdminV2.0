#region REFERENCIAS
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.BL.MembershipHistory___BL;
using MCEI.SysControlAdmin.EN.MembershipHistory___EN;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


#endregion

namespace MCEI.SysControlAdmin.WebApp.Controllers.MembershipHistory___Controller
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Desarrollador, Administrador, Digitador")]
    public class MembershipHistoryController : Controller
    {
        // Creamos la instancia para acceeder a los metodos
        MembershipHistoryBL membershipHistoryBL = new MembershipHistoryBL();

        #region METODO PARA MOSTRAR INDEX
        // Accion Para Mostrar La Vista Index
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public async Task<IActionResult> Index(MembershipHistory membership = null!)
        {
            if (membership == null)
                membership = new MembershipHistory();

            var memberships = await membershipHistoryBL.SearchAsync(membership);
            return View(memberships);
        }
        #endregion

        #region METODO PARA MOSTRAR DETALLES
        // Accion Que Muestra El Detalle De Un Registro
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public async Task<IActionResult> DetailsMembershipHistory(int id)
        {
            try
            {
                MembershipHistory membership = await membershipHistoryBL.GetByIdAsync(new MembershipHistory { Id = id });
                if (membership == null)
                {
                    return NotFound();
                }
                // Convertir el array de bytes en imagen para mostrar en la vista
                if (membership.ImageData != null && membership.ImageData.Length > 0)
                {
                    ViewBag.ImageUrl = Convert.ToBase64String(membership.ImageData);
                }
                return View(membership); // Retornamos los Detalles a La Vista
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(); // Devolver la vista sin ningún objeto Membership
            }
        }
        #endregion
    }
}
