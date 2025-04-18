#region REFERENCIAS
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.BL.Membership___BL;
using MCEI.SysControlAdmin.EN.Membership___EN;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;


#endregion

namespace MCEI.SysControlAdmin.WebApp.Controllers.Recommendation___Controller
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Desarrollador, Administrador")]
    public class RecommendationController : Controller
    {
        // Creamos Una Instancia Para Acceder a Los Metodos
        MembershipBL membershipBL = new MembershipBL();

        #region METODO PARA MOSTRAR INDEX
        // Accion Para Mostrar La Vista Index
        [Authorize(Roles = "Desarrollador, Administrador")]
        public async Task<IActionResult> Index(Membership membership = null!)
        {
            if (membership == null)
                membership = new Membership();

            var memberships = await membershipBL.SearchAsync(membership);
            return View(memberships);
        }
        #endregion

        #region METODO PARA REPORTE
        // Metodo Para Generar Ficha o Reporte En PDF 
        [Authorize(Roles = "Desarrollador, Administrador")]
        public async Task<ActionResult> GeneratePDFfileRecomendation(int id)
        {
            var generatePDF = await membershipBL.GetByIdAsync(new Membership { Id = id });
            string fileName = $"CartaDeRecomendacion_{generatePDF.Name}_{generatePDF.LastName}_{generatePDF.Dui}_MCEI.pdf";
            generatePDF.DateNow = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));
            return new ViewAsPdf("GeneratePDFfileRecomendation", generatePDF)
            {
                FileName = fileName
            };
        }
        #endregion
    }
}
