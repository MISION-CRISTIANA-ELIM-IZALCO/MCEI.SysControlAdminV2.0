#region REFERENCIAS
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.BL.Server___BL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;


#endregion

namespace MCEI.SysControlAdmin.WebApp.Controllers.Annual_Servers___Controller
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Desarrollador, Administrador")]
    public class AnnualServersController : Controller
    {
        // Creamos las instancias para acceder a los metodos
        ServerBL serverBL = new ServerBL();

        #region METODO PARA MOSTRAR INDEX
        [Authorize(Roles = "Desarrollador, Administrador")]
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region METODO PARA REPORTE DE SERVIDORES ACTIVOS
        // Método Para Generar un PDF con la Lista de Servidores Activos Agrupados por Privilegio
        [Authorize(Roles = "Desarrollador, Administrador")]
        public async Task<ActionResult> GenerateGroupedServersPDF()
        {
            // Obtener la lista de servidores activos agrupados por privilegio
            var groupedServers = await serverBL.GetActiveServersGroupedByPrivilegeAsync();
            string fileName = "Reporte_Servidores_Activos_Agrupados_MCEI.pdf";
            var pdfResult = new ViewAsPdf("GenerateGroupedServersPDF", groupedServers)
            {
                FileName = fileName,
            };
            return pdfResult;
        }
        #endregion
    }
}
