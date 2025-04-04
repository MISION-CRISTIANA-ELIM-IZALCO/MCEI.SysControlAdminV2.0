#region REFERENCIAS
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.BL.Membership___BL;
using MCEI.SysControlAdmin.BL.Privilege___BL;
using MCEI.SysControlAdmin.BL.ServerHistory___BL;
using MCEI.SysControlAdmin.EN.Membership___EN;
using MCEI.SysControlAdmin.EN.Privilege___EN;
using MCEI.SysControlAdmin.EN.Server___EN;
using MCEI.SysControlAdmin.EN.ServerHistory___EN;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


#endregion

namespace MCEI.SysControlAdmin.WebApp.Controllers.ServerHistory___Controller
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Desarrollador, Administrador, Digitador")]
    public class ServerHistoryController : Controller
    {
        // Creamos las instancias para acceder a los metodos
        ServerHistoryBL serverHistoryBL = new ServerHistoryBL();
        MembershipBL membershipBL = new MembershipBL();
        PrivilegeBL privilegeBL = new PrivilegeBL();

        #region METODOS PARA AUTOCOMPLETADO
        // Metodo que extrae por Id y devolver a la vista en formato Json
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        [HttpGet]
        public async Task<IActionResult> GetMembershipDetails(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El ID proporcionado no es válido.");
            }

            var membership = await membershipBL.GetByIdAsync(id);
            if (membership == null)
            {
                return NotFound();
            }

            // Validación de DUI
            string duiValue = string.IsNullOrWhiteSpace(membership.Dui) ? "MENOR DE EDAD" : membership.Dui;

            // Validación de Sector y Cell
            string sectorValue = membership.Sector != "0" ? membership.Sector : "SOY SUPERVISOR";
            string cellValue;

            if (membership.Sector == "0" && membership.Cell == "0")
            {
                cellValue = "SOY SUPERVISOR";
            }
            else if (membership.Sector != "0" && membership.Cell == "0")
            {
                cellValue = "SOY LIDER";
            }
            else
            {
                cellValue = membership.Cell;
            }

            var membershipDetails = new
            {
                Dui = duiValue,
                DateOfBirth = membership.DateOfBirth.ToString("dd/MM/yyyy"),
                Age = membership.Age + " AÑOS",
                Gender = membership.Gender,
                CivilStatus = membership.CivilStatus,
                ProfessionOrStudy = membership.ProfessionOrStudy,
                BaptismOfTheHolySpirit = membership.BaptismOfTheHolySpirit,
                Zone = membership.Zone,
                District = membership.District,
                Sector = sectorValue,
                Cell = cellValue,
                Status = membership.Status == 1 ? "ACTIVO" : membership.Status == 2 ? "INACTIVO" : "Desconocido",
                CommentsOrObservations = string.IsNullOrWhiteSpace(membership.CommentsOrObservations) ? "VACIO" : membership.CommentsOrObservations,
                InternalIdentityCode = membership.InternalIdentityCode,
                Photo = membership.ImageData
            };

            return Json(membershipDetails);
        }

        // Metodo que extrae por Id y devolver a la vista en formato Json
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        [HttpGet]
        public async Task<IActionResult> GetPrivilegeDetails(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El ID proporcionado no es válido.");
            }

            var privilege = await privilegeBL.GetByIdAsync(id);
            if (privilege == null)
            {
                return NotFound();
            }

            var privilegeDetails = new
            {
                Status = privilege.Status == 1 ? "ACTIVO" : privilege.Status == 2 ? "INACTIVO" : "Desconocido",
                DateCreated = privilege.DateCreated.ToString("dd/MM/yyyy"),
                DateModification = privilege.DateModification.ToString("dd/MM/yyyy")
            };
            return Json(privilegeDetails);
        }
        #endregion

        #region METODO PARA MOSTRAR INDEX
        // Accion Para Mostrar La Vista Index
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public async Task<IActionResult> Index(ServerHistory serverHistory = null!)
        {
            if (serverHistory == null)
                serverHistory = new ServerHistory();

            var serverHistorys = await serverHistoryBL.SearchIncludeAsync(serverHistory);
            var membership = await membershipBL.GetAllAsync();
            var privilege = await privilegeBL.GetAllAsync();

            ViewBag.Memberships = membership;
            ViewBag.Privileges = privilege;

            return View(serverHistorys);
        }
        #endregion

        #region METODO PARA MOSTRAR DETALLES
        // Acción que muestra el detalle de un registro
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public async Task<IActionResult> DetailsServerHistory(int id)
        {
            try
            {
                ServerHistory serverHistory = await serverHistoryBL.GetByIdAsync(new ServerHistory { Id = id });

                // Verifica si se encontró el registro
                if (serverHistory == null)
                {
                    return NotFound();
                }

                // Cargar entidades relacionadas
                serverHistory.Membership = await membershipBL.GetByIdAsync(new Membership { Id = serverHistory.IdMembership });
                serverHistory.Privilege = await privilegeBL.GetByIdAsync(new Privilege { Id = serverHistory.IdPrivilege });

                // Comprueba si las entidades relacionadas existen
                if (serverHistory.Membership == null || serverHistory.Privilege == null)
                {
                    return NotFound();
                }

                // Si hay imagen, conviértela a Base64 para mostrarla en la vista
                if (serverHistory.Membership.ImageData != null && serverHistory.Membership.ImageData.Length > 0)
                {
                    ViewBag.ImageUrl = Convert.ToBase64String(serverHistory.Membership.ImageData);
                }

                return View(serverHistory);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(); // Puedes retornar un modelo vacío o una vista de error
            }
        }
        #endregion
    }
}
