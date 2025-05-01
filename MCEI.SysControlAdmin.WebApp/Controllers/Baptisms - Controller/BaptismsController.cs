#region REFERENCIAS
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.BL.Baptisms___BL;
using MCEI.SysControlAdmin.BL.Membership___BL;
using MCEI.SysControlAdmin.Core.Utils;
using MCEI.SysControlAdmin.EN.Baptisms___EN;
using MCEI.SysControlAdmin.EN.Membership___EN;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using System.Runtime.Serialization;

#endregion

namespace MCEI.SysControlAdmin.WebApp.Controllers.Baptisms___Controller
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Desarrollador, Administrador, Digitador")]
    public class BaptismsController : Controller
    {
        // Creamos las instancias para acceder a los metodos
        BaptismsBL baptismsBL = new BaptismsBL();
        MembershipBL membershipBL = new MembershipBL();

        #region METODO PARA CREAR
        // Accion para mostrar la vista de crear
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public IActionResult CreateBaptisms()
        {
            ViewBag.Error = "";
            return View();
        }

        // Metodo que recibe y envia a la base de datos
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBaptisms(Baptisms baptisms, IFormFile imagen)
        {
            try
            {
                const int maxFileSize = 1572864; // 1.5 MB
                if (imagen.Length > maxFileSize)
                {
                    throw new Exception("La imagen no debe exceder los 1.5MB de tamaño.");
                }
                // Mapeo de img a ArrayByte
                if (imagen != null && imagen.Length > 0)
                {
                    byte[] imagenData = null!;
                    using (var memoryStream = new MemoryStream())
                    {
                        await imagen.CopyToAsync(memoryStream);
                        imagenData = memoryStream.ToArray();
                    }

                    baptisms.ImageData = imagenData; // Asigna el array de bytes al campo de la imagen en tu modelo
                }
                baptisms.WaterBaptism = "SI, DENTRO DE LA MISIÓN";
                baptisms.DateCreated = DateTime.Now.GetFechaZonaHoraria();
                baptisms.DateModification = DateTime.Now.GetFechaZonaHoraria();
                int result = await baptismsBL.CreateAsync(baptisms);

                // Creamos un nuevo objeto para mapear las propiedades a membership
                var membership = new Membership
                {
                    Name = baptisms.Name,
                    LastName = baptisms.LastName,
                    Dui = "",
                    DateOfBirth = DateTime.Now.GetFechaZonaHoraria(),
                    Age = baptisms.Age,
                    Gender = "",
                    CivilStatus = baptisms.CivilStatus,
                    Phone = baptisms.Phone,
                    Address = baptisms.Address,
                    ProfessionOrStudy = "",
                    PlaceOfWorkOrStudy = baptisms.PlaceOfWorkOrStudy,
                    WorkOrStudyPhone = baptisms.WorkOrStudyPhone,
                    ConversionDate = baptisms.ConversionDate,
                    PlaceOfConversion = baptisms.PlaceOfConversion,
                    WaterBaptism = baptisms.WaterBaptism,
                    BaptismOfTheHolySpirit = baptisms.BaptismOfTheHolySpirit,
                    PastorsName = baptisms.PastorsName,
                    SupervisorsName = baptisms.SupervisorsName,
                    LeadersName = baptisms.LeadersName,
                    TimeToGather = baptisms.TimeToGather,
                    Zone = baptisms.Zone,
                    District = baptisms.District,
                    Sector = baptisms.Sector,
                    Cell = baptisms.Cell,
                    InternalIdentityCode = "",
                    Status = 0,
                    ImageData = baptisms.ImageData,
                    CommentsOrObservations = baptisms.CommentsOrObservations,
                    DateCreated = baptisms.DateCreated,
                    DateModification = baptisms.DateModification,
                };
                int resultMembership = await membershipBL.CreateAsync(membership);

                TempData["SuccessMessageCreate"] = "Bautismo Agregado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(baptisms);
            }
        }
        #endregion

        #region METODO PARA MOSTRAR INDEX
        // Accion para mostrar la vista index
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public async Task<IActionResult> Index(Baptisms baptisms = null!)
        {
            if(baptisms == null)
                baptisms= new Baptisms();

            var baptismss = await baptismsBL.SearchAsync(baptisms);
            return View(baptismss);
        }
        #endregion

        #region METODO PARA MODIFICAR
        // Accion que muestra la vista de modificar
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public async Task<IActionResult> EditBaptisms(int id)
        {
            try
            {
                Baptisms baptisms = await baptismsBL.GetByIdAsync(new Baptisms { Id = id });
                if(baptisms == null)
                    return NotFound();

                // Convertir el array de bytes en imagen para mostrar en la vista
                if (baptisms.ImageData != null && baptisms.ImageData.Length > 0)
                {
                    ViewBag.ImageUrl = Convert.ToBase64String(baptisms.ImageData);
                }
                return View(baptisms);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        // Acción que recibe los datos del formulario para ser enviados a la base de datos
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBaptisms(int id, Baptisms baptisms, IFormFile imagen)
        {
            try
            {
                if(id != baptisms.Id)
                    return BadRequest();

                const int maxFileSize = 1572864; // 1.5 MB
                if (imagen.Length > maxFileSize)
                {
                    throw new Exception("La imagen no debe exceder los 1.5MB de tamaño.");
                }

                if (imagen != null && imagen.Length > 0) // Verificar si se ha subido una nueva imagen
                {
                    byte[] imagenData = null!;
                    using (var memoryStream = new MemoryStream())
                    {
                        await imagen.CopyToAsync(memoryStream);
                        imagenData = memoryStream.ToArray();
                    }
                    baptisms.ImageData = imagenData; // Asignar el array de bytes de la nueva imagen a la entidad Membership
                }
                else
                {
                    // Si no se proporciona una nueva imagen, se conserva la imagen existente
                    Baptisms existingBaptisms = await baptismsBL.GetByIdAsync(new Baptisms { Id = id });
                    baptisms.ImageData = existingBaptisms.ImageData;
                }
                baptisms.DateModification = DateTime.Now.GetFechaZonaHoraria();
                int result = await baptismsBL.UpdateAsync(baptisms);
                TempData["SuccessMessageUpdate"] = "Bautismo Modificado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(baptisms);
            }
        }
        #endregion

        #region METODO PARA MOSTRAR DETALLES
        // Accion que muestra el detalle de un registro
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public async Task<IActionResult> DetailsBaptisms(int id)
        {
            try
            {
                Baptisms baptisms = await baptismsBL.GetByIdAsync(new Baptisms { Id = id });
                if (baptisms == null)
                    return NotFound();

                // Convertir el array de bytes en imagen para mostrar en la vista
                if (baptisms.ImageData != null && baptisms.ImageData.Length > 0)
                {
                    ViewBag.ImageUrl = Convert.ToBase64String(baptisms.ImageData);
                }
                return View(baptisms);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }
        #endregion

        #region METODO PARA ELIMINAR
        // Metodo para mostrar la vista de eliminar
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public async Task<IActionResult> DeleteBaptisms(int id)
        {
            try
            {
                Baptisms baptisms = await baptismsBL.GetByIdAsync(new Baptisms { Id = id });
                if(baptisms == null)
                    return NotFound();

                // Convertir el array de bytes en imagen para mostrar en la vista
                if (baptisms.ImageData != null && baptisms.ImageData.Length > 0)
                {
                    ViewBag.ImageUrl = Convert.ToBase64String(baptisms.ImageData);
                }
                return View(baptisms);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        // Accion que recibe los datos del formulario para ser enviados a la DB
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBaptisms(int id, Baptisms baptisms)
        {
            try
            {
                Baptisms baptismsDB = await baptismsBL.GetByIdAsync(baptisms);
                int result = await baptismsBL.DeleteAsync(baptismsDB);
                TempData["SuccessMessageDelete"] = "Bautismo Eliminado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                var baptismsDB = await baptismsBL.GetByIdAsync(baptisms);
                if (baptismsDB == null)
                    baptismsDB = new Baptisms();
                return View(baptismsDB);
            }
        }
        #endregion

        #region METODO PARA FICHA OFICIAL DE BAUTISMO
        [Authorize(Roles = "Desarrollador, Administrador, Digitador")]
        public async Task<ActionResult> GeneratePDFfileBaptisms(int id)
        {
            var generatePDF = await baptismsBL.GetByIdAsync(new Baptisms { Id = id });
            string fileName = $"FichaBautismo_{generatePDF.Name}_{generatePDF.LastName}_MCEI.pdf";
            return new ViewAsPdf("GeneratePDFfileBaptisms", generatePDF)
            {
                FileName = fileName,
            };
        }
        #endregion
    }
}
