#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCEI.SysControlAdmin.DAL.Membership___DAL;
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.EN.Baptisms___EN;
using Microsoft.EntityFrameworkCore;


#endregion

namespace MCEI.SysControlAdmin.DAL.Baptisms___DAL
{
    public class BaptismsDAL
    {
        #region METODO PARA VALIDAR UNICA EXISTENCIA DEL REGISTRO
        // Metodo para validar la unica existencia del registro y no haber duplicidad
        private static async Task<bool> ExistBaptisms(Baptisms baptisms, ContextDB contextDB)
        {
            bool result = false;
            var baptismss = await contextDB.Baptisms.FirstOrDefaultAsync(b => b.Name == baptisms.Name && b.LastName == baptisms.LastName && b.Age == baptisms.Age && b.Id != baptisms.Id);
            if (baptismss != null && baptismss.Id > 0 && baptismss.Name == baptisms.Name && baptismss.LastName == baptisms.LastName && baptismss.Age == baptisms.Age)
                result = true;
            return result;
        }
        #endregion

        #region METODO PARA CREAR
        // Metodo para crear un nuevo registro a la base de datos
        public static async Task<int> CreateAsync(Baptisms baptisms)
        {
            if(baptisms == null)
            {
                throw new ArgumentNullException(nameof(baptisms), "El Bautismo no puede ser nulo.");
            }

            int result = 0;
            using (var contextDB = new ContextDB())
            {
                // Verificamos si ya existe
                bool baptismsExists = await ExistBaptisms(baptisms, contextDB);
                if (baptismsExists)
                    throw new Exception("Bautismo Ya Existente, Vuelve a Intentarlo Nuevamente.");

                // Guardamos en la base de datos
                contextDB.Baptisms.Add(baptisms);
                result = await contextDB.SaveChangesAsync();
            }
            return result;
       
        }
        #endregion

        #region METODO PARA MOSTRAR
        // Metodo para mostrar la lista de registros
        public static async Task<List<Baptisms>> GetAllAsync()
        {
            var baptisms = new List<Baptisms>();
            using (var contextDB = new ContextDB())
            {
                baptisms = await contextDB.Baptisms.ToListAsync();
            }
            return baptisms;
        }
        #endregion

        #region METODO PARA OBTENER POR ID
        // Metodo para mostrar un registro por su id
        public static async Task<Baptisms> GetByIdAsync(Baptisms baptisms)
        {
            var baptismsDB = new Baptisms();
            using (var contextDB = new ContextDB())
            {
                baptismsDB = await contextDB.Baptisms.FirstOrDefaultAsync(b => b.Id == baptisms.Id);
            }
            return baptismsDB!;
        }
        #endregion

        #region METODO PARA BUSCAR REGISTROS MEDIANTE EL USO DE FILTROS
        // Metodo para buscar por filtros
        internal static IQueryable<Baptisms> QuerySelect(IQueryable<Baptisms> query, Baptisms baptisms)
        {
            // Por ID
            if (baptisms.Id > 0)
                query = query.Where(m => m.Id == baptisms.Id);

            if (!string.IsNullOrWhiteSpace(baptisms.Name))
                query = query.Where(m => m.Name.Contains(baptisms.Name));

            if (!string.IsNullOrWhiteSpace(baptisms.LastName))
                query = query.Where(m => m.LastName.Contains(baptisms.LastName));
            
            if (!string.IsNullOrWhiteSpace(baptisms.CivilStatus))
                query = query.Where(m => m.LastName.Contains(baptisms.CivilStatus));

            // Ordenamos de manerae desendente
            query = query.OrderByDescending(m => m.Id).AsQueryable();

            return query;
        }
        #endregion

        #region METODO PARA BUSCAR
        // Metodo para buscar registros existentes
        public static async Task<List<Baptisms>> SearchAsync(Baptisms baptisms)
        {
            var baptismss = new List<Baptisms>();
            using (var contextDB =  new ContextDB())
            {
                var select = contextDB.Baptisms.AsQueryable();
                select = QuerySelect(select, baptisms);
                baptismss = await select.ToListAsync();
            }
            return baptismss;
        }
        #endregion

        #region METODO PARA MODIFICAR
        // Metodo para modificar un registro
        public static async Task<int> UpdateAsync(Baptisms baptisms)
        {
            int result = 0;
            using (var contextDB = new ContextDB())
            {
                var baptismsDB = await contextDB.Baptisms.FirstOrDefaultAsync(b => b.Id == baptisms.Id);
                if (baptismsDB == null)
                    throw new Exception("Bautismo no encontrado para actualizar");

                // Verificamos si ya existe
                bool baptismsExists = await ExistBaptisms(baptisms, contextDB);
                if (baptismsExists)
                    throw new Exception("Bautismo Ya Existente, Vuelve a Intentarlo Nuevamente");

                // Actualizamos los datos
                baptismsDB.Name = baptisms.Name;
                baptismsDB.LastName = baptisms.LastName;
                baptismsDB.Address = baptisms.Address;
                baptismsDB.Phone = baptisms.Phone;
                baptismsDB.PlaceOfWorkOrStudy = baptisms.PlaceOfWorkOrStudy;
                baptismsDB.WorkOrStudyPhone = baptisms.WorkOrStudyPhone;
                baptismsDB.CivilStatus = baptisms.CivilStatus;
                baptismsDB.BaptismOfTheHolySpirit = baptisms.BaptismOfTheHolySpirit;
                baptismsDB.ConversionDate = baptisms.ConversionDate;
                baptismsDB.PlaceOfConversion = baptisms.PlaceOfConversion;
                baptismsDB.TimeToGather = baptisms.TimeToGather;
                baptismsDB.PastorsName = baptisms.PastorsName;
                baptismsDB.SupervisorsName = baptisms.SupervisorsName;
                baptismsDB.LeadersName = baptisms.LeadersName;
                baptismsDB.Zone = baptisms.Zone;
                baptismsDB.District = baptisms.District;
                baptismsDB.Sector = baptisms.Sector;
                baptismsDB.Cell = baptisms.Cell;
                baptismsDB.CommentsOrObservations = baptisms.CommentsOrObservations;
                baptismsDB.ImageData = baptisms.ImageData;
                baptismsDB.DateModification = baptisms.DateModification;

                contextDB.Update(baptismsDB);
                result = await contextDB.SaveChangesAsync();
            }
            return result;
        }
        #endregion

        #region METODO PARA ELIMINAR
        // Metodo para eliminar un registro existente en la base de datos
        public static async Task<int> DeleteAsync(Baptisms baptisms)
        {
            int result = 0;
            using (var contextDB = new ContextDB())
            {
                var baptismsDB = await contextDB.Baptisms.FirstOrDefaultAsync(b => b.Id == baptisms.Id);
                if (baptismsDB != null)
                {
                    contextDB.Baptisms.Remove(baptismsDB);
                    result = await contextDB.SaveChangesAsync();
                }
            }
            return result;
        }
        #endregion
    }
}
