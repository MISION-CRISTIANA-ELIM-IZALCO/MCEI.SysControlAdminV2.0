#region REFERENCIAS
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.BL.Membership___BL;
using MCEI.SysControlAdmin.BL.MembershipHistory___BL;
using MCEI.SysControlAdmin.EN.Membership___EN;
using MCEI.SysControlAdmin.EN.MembershipHistory___EN;
using MCEI.SysControlAdmin.EN.Role___EN;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Numerics;
using System.Reflection;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;
using MCEI.SysControlAdmin.Core.Utils;
using Rotativa.AspNetCore;

#endregion

namespace MCEI.SysControlAdmin.WebApp.Controllers.Membership___Controller
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Desarrollador, Administrador, Digitador")]
    public class MembershipController : Controller
    {
        // Creamos las instancias para acceder a los metodos
        MembershipBL membershipBL = new MembershipBL();
        MembershipHistoryBL membershipHistoryBL = new MembershipHistoryBL();

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
                membership.DateCreated = DateTime.Now.GetFechaZonaHoraria();
                membership.DateModification = DateTime.Now.GetFechaZonaHoraria();
                int result = await membershipBL.CreateAsync(membership);

                // Crear un nuevo objeto para mapear las propiedades a MembershipHistory
                var membershipHistory = new MembershipHistory
                {
                    Name = membership.Name,
                    LastName = membership.LastName,
                    Dui = membership.Dui,
                    DateOfBirth = membership.DateOfBirth,
                    Age = membership.Age,
                    Gender = membership.Gender,
                    CivilStatus = membership.CivilStatus,
                    Phone = membership.Phone,
                    Address = membership.Address,
                    ProfessionOrStudy = membership.ProfessionOrStudy,
                    PlaceOfWorkOrStudy = membership.PlaceOfWorkOrStudy,
                    WorkOrStudyPhone = membership.WorkOrStudyPhone,
                    ConversionDate = membership.ConversionDate,
                    PlaceOfConversion = membership.PlaceOfConversion,
                    WaterBaptism = membership.WaterBaptism,
                    BaptismOfTheHolySpirit = membership.BaptismOfTheHolySpirit,
                    PastorsName = membership.PastorsName,
                    SupervisorsName = membership.SupervisorsName,
                    LeadersName = membership.LeadersName,
                    TimeToGather = membership.TimeToGather,
                    Zone = membership.Zone,
                    District = membership.District,
                    Sector = membership.Sector,
                    Cell = membership.Cell,
                    InternalIdentityCode = membership.InternalIdentityCode,
                    Status = membership.Status,
                    ImageData = membership.ImageData,
                    CommentsOrObservations = membership.CommentsOrObservations,
                    DateCreated = membership.DateCreated,
                    DateModification = membership.DateModification,
                    NameOfSpouse = membership.NameOfSpouse,
                    LastNameOfSpouse = membership.LastNameOfSpouse,
                    DateOfBirthOfSpouse = membership.DateOfBirthOfSpouse,
                    AgeOfSpouse = membership.AgeOfSpouse,
                    GenderOfSpouse = membership.GenderOfSpouse,
                    PhoneOfSpouse = membership.PhoneOfSpouse
                };

                int resultMembershipHistory = await membershipHistoryBL.CreateAsync(membershipHistory);

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
                membership.DateModification = DateTime.Now.GetFechaZonaHoraria();
                int result = await membershipBL.UpdateAsync(membership);

                // Crear un nuevo objeto para mapear las propiedades a MembershipHistory
                var membershipHistory = new MembershipHistory
                {
                    Name = membership.Name,
                    LastName = membership.LastName,
                    Dui = membership.Dui,
                    DateOfBirth = membership.DateOfBirth,
                    Age = membership.Age,
                    Gender = membership.Gender,
                    CivilStatus = membership.CivilStatus,
                    Phone = membership.Phone,
                    Address = membership.Address,
                    ProfessionOrStudy = membership.ProfessionOrStudy,
                    PlaceOfWorkOrStudy = membership.PlaceOfWorkOrStudy,
                    WorkOrStudyPhone = membership.WorkOrStudyPhone,
                    ConversionDate = membership.ConversionDate,
                    PlaceOfConversion = membership.PlaceOfConversion,
                    WaterBaptism = membership.WaterBaptism,
                    BaptismOfTheHolySpirit = membership.BaptismOfTheHolySpirit,
                    PastorsName = membership.PastorsName,
                    SupervisorsName = membership.SupervisorsName,
                    LeadersName = membership.LeadersName,
                    TimeToGather = membership.TimeToGather,
                    Zone = membership.Zone,
                    District = membership.District,
                    Sector = membership.Sector,
                    Cell = membership.Cell,
                    InternalIdentityCode = membership.InternalIdentityCode,
                    Status = membership.Status,
                    ImageData = membership.ImageData,
                    CommentsOrObservations = membership.CommentsOrObservations,
                    DateCreated = membership.DateCreated,
                    DateModification = membership.DateModification,
                    NameOfSpouse = membership.NameOfSpouse,
                    LastNameOfSpouse = membership.LastNameOfSpouse,
                    DateOfBirthOfSpouse = membership.DateOfBirthOfSpouse,
                    AgeOfSpouse = membership.AgeOfSpouse,
                    GenderOfSpouse = membership.GenderOfSpouse,
                    PhoneOfSpouse = membership.PhoneOfSpouse
                };

                int resultMembershipHistory = await membershipHistoryBL.CreateAsync(membershipHistory);

                TempData["SuccessMessageUpdate"] = "Membresia Modificada Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(membership); // Devolver la vista con el objeto Membership para que el usuario pueda corregir los datos
            }
        }
        #endregion

        #region METODO PARA MOSTRAR DETALLES
        // Accion Que Muestra El Detalle De Un Registro
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public async Task<IActionResult> DetailsMembership(int id)
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
                return View(membership); // Retornamos los Detalles a La Vista
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(); // Devolver la vista sin ningún objeto Membership
            }
        }
        #endregion

        #region METODO PARA ELIMINAR
        // Accion Que Muestra La Vista De Eliminar
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public async Task<IActionResult> DeleteMembership(int id)
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

        // Accion Que Recibe Los Datos Del Formulario Para Ser Enviados a La BD
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMembership(int id, Membership membership)
        {
            try
            {
                Membership membershipDB = await membershipBL.GetByIdAsync(membership);
                int result = await membershipBL.DeleteAsync(membershipDB);
                TempData["SuccessMessageDelete"] = "Membresia Eliminada Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                var membershipDB = await membershipBL.GetByIdAsync(membership);
                if (membershipDB == null)
                    membershipDB = new Membership();
                return View(membershipDB);
            }
        }
        #endregion

        #region METODO PARA FICHA OFICIAL
        // Metodo Para Generar Ficha o Reporte En PDF 
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public async Task<ActionResult> GeneratePDFfileMembership(int id)
        {
            var generatePDF = await membershipBL.GetByIdAsync(new Membership { Id = id });
            string fileName = $"Ficha_{generatePDF.Name}_{generatePDF.LastName}_{generatePDF.InternalIdentityCode}_MCEI.pdf";
            return new ViewAsPdf("GeneratePDFfileMembership", generatePDF)
            {
                FileName = fileName
            };
        }
        #endregion
    }
}
