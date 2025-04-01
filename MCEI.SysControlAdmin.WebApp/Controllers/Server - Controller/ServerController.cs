#region REFERENCIAS
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.BL.Membership___BL;
using MCEI.SysControlAdmin.BL.Privilege___BL;
using MCEI.SysControlAdmin.BL.Server___BL;
using MCEI.SysControlAdmin.EN.Server___EN;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


#endregion

namespace MCEI.SysControlAdmin.WebApp.Controllers.Server___Controller
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Desarrollador, Administrador, Digitador")]
    public class ServerController : Controller
    {
        // Creamos la instancia para acceder a los metodos
        ServerBL serverBL = new ServerBL();
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

        #region METODO PARA CREAR
        // Accion para mostrar la vista crear
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public async Task<IActionResult> CreateServer()
        {
            ViewBag.Membership = await membershipBL.GetAllAsync();
            ViewBag.Privilege = await privilegeBL.GetAllAsync();
            ViewBag.Error = "";
            return View();
        }

        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateServer(Server server)
        {
            try
            {
                server.Status = 1;
                server.DateCreated = DateTime.Now;
                server.DateModification = DateTime.Now;
                int result = await serverBL.CreateAsync(server);
                TempData["SuccessMessageCreate"] = "Servidor Agregado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Membership = await membershipBL.GetAllAsync();
                ViewBag.Privilege = await privilegeBL.GetAllAsync();
                return View(server);
            }
        }
        #endregion
    }
}
