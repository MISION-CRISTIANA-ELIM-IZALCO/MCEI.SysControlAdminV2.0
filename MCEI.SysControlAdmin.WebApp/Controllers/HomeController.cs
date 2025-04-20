using MCEI.SysControlAdmin.BL.Membership___BL;
using MCEI.SysControlAdmin.BL.Privilege___BL;
using MCEI.SysControlAdmin.BL.Server___BL;
using MCEI.SysControlAdmin.WebApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MCEI.SysControlAdmin.WebApp.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Desarrollador, Administrador, Digitador")]
    public class HomeController : Controller
    {
        // Creamos las instancias para acceder a los metodos
        MembershipBL membershipBL = new MembershipBL();
        PrivilegeBL privilegeBL = new PrivilegeBL();
        ServerBL serverBL = new ServerBL();

        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Desarrollador, Administrador")]
        public async Task<IActionResult> Dashboard()
        {
            // Recuedros superiores - Datos Generales
            int totalMiembros = await membershipBL.GetTotalCountAsync(); // Total de miembros
            int totalPrivilegios = await privilegeBL.GetTotalCountAsync(); // Total de privilegios
            int totalServidores = await serverBL.GetTotalCountAsync(); // Total de servidores

            // Membership
            var ageCategories = membershipBL.GetMembershipsAgeCategories(); // Categorizacion de edad de los mimebros
            var (totalmasculino, totalfemenino) = await membershipBL.GetMembershipsByGenderAsync(); // Total de miembros por genero masculino y femenino
            var (bautizadosEspiritu, noBautizadosEspiritu) = await membershipBL.GetBautizadosEspirituSantoAsync(); // Total de miembros si son o no bautisados por el espiritu santo
            var (totalActivos, totalInactivos) = await membershipBL.GetTotalByStatusAsync(); // Total de miembros por estado: Activo e Inactivo

            // Server
            var (totalmasculinoServer, totalfemeninoServer) = await serverBL.GetServerByGenderAsync(); // Total de servidores por genero masculino y femenino
            var (totalBautisadosSpirit, totalNoBautisadosSpirit) = await serverBL.GetBautizadosEspirituSantoAsync(); // Total de servidores bautisados y no por el espiritu santo
            var serversByPrivilege = await serverBL.GetServerCountByPrivilegeAsync(); // Obtener conteo de servidores según privilegio

            // ViewData recuedros superiores - Datos Generales
            ViewData["TotalMiembros"] = totalMiembros; // Total de miembros
            ViewData["TotalPrivilegios"] = totalPrivilegios; // Total de privilegios
            ViewData["TotalServidores"] = totalServidores; // Total de servidores

            // ViewData Memberships
            ViewData["TotalNinos"] = ageCategories["Niños (5-12)"]; // Total de alumnos niños
            ViewData["TotalAdolescentes"] = ageCategories["Adolescentes (13-17)"]; // Total de alumnos adolecentes
            ViewData["TotalJovenes"] = ageCategories["Jóvenes (18-25)"]; // Total de alumnos jovenes
            ViewData["TotalAdultos"] = ageCategories["Adultos (26+)"]; // Total de alumnos adultos
            ViewData["TotalMiembrosMasculinos"] = totalmasculino; // Total miembros Masculinos
            ViewData["TotalMiembrosFemeninos"] = totalfemenino; // Total miembros femeninos
            ViewData["BautizadosEspirituSanto"] = bautizadosEspiritu; // Total miembros bautisados por el espiritu santo
            ViewData["NoBautizadosEspirituSanto"] = noBautizadosEspiritu; // Total miembros No-bautisados por el espiritu santo
            ViewData["TotalMiembrosActivos"] = totalActivos; // Total miembros activos
            ViewData["TotalMiembrosInactivos"] = totalInactivos; // Total miembros inactivos

            // ViewData Server
            ViewData["TotalServidoresMasculinos"] = totalmasculinoServer; // Total servidores Masculinos
            ViewData["TotalServidoresFemeninos"] = totalfemeninoServer; // Total servidores femeninos
            ViewData["TotalServidoresBautisadosPorElEspirituSanto"] = totalBautisadosSpirit; // Total servidores bautisados por el espirtiu santo
            ViewData["TotalServidoresNoBautisadosPorElEspirituSanto"] = totalNoBautisadosSpirit; // Total servidores no bautisados por el espiritu santo
            ViewData["ServersByPrivilege"] = serversByPrivilege; // Obtener conteo de servidores según privilegio

            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Desarrollador, Administrador")]
        public async Task<JsonResult> GetMiembrosPorEstadoCivil()
        {
            var datos = await membershipBL.GetTotalByEstadoCivilAsync();

            return Json(new
            {
                labels = datos.Keys,
                data = datos.Values
            });
        }
    }
}
