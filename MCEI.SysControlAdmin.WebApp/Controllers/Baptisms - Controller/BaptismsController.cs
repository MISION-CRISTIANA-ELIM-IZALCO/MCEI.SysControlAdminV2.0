#region REFERENCIAS
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.BL.Baptisms___BL;
using MCEI.SysControlAdmin.Core.Utils;
using MCEI.SysControlAdmin.EN.Baptisms___EN;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace MCEI.SysControlAdmin.WebApp.Controllers.Baptisms___Controller
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Desarrollador, Administrador, Digitador")]
    public class BaptismsController : Controller
    {
        // Creamos las instancias para acceder a los metodos
        BaptismsBL baptismsBL = new BaptismsBL();

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
    }
}
