#region REFERENCIAS
using MCEI.SysControlAdmin.EN.MembershipHistory___EN;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento



#endregion

namespace MCEI.SysControlAdmin.DAL.MembershipHistory___DAL
{
    public class MembershipHistoryDAL
    {
        #region METODO PARA CREAR
        // Metodo para crear agregar un nuevo registro a la base de datos
        public static async Task<int> CreateAsync(MembershipHistory membership)
        {
            if (membership == null)
            {
                throw new ArgumentNullException(nameof(membership), "La Membresia no puede ser nulo.");
            }

            int result = 0;
            using (var contextDB = new ContextDB())
            {
                // Guardar la membresi en la base de datos
                contextDB.MembershipHistory.Add(membership);
                result = await contextDB.SaveChangesAsync();
            }
            return result;
        }
        #endregion

        #region METODO PARA MOSTRAR
        // Metodo Para Mostrar La Lista De Registros
        public static async Task<List<MembershipHistory>> GetAllAsync()
        {
            var membership = new List<MembershipHistory>();
            using (var dbContext = new ContextDB())
            {
                membership = await dbContext.MembershipHistory.ToListAsync();
            }
            return membership;
        }
        #endregion

        #region METODO PARA OBTENER POR ID
        // Metodo Para Mostrar Un Registro En Base A Su Id
        public static async Task<MembershipHistory> GetByIdAsync(MembershipHistory membership)
        {
            var membershipDB = new MembershipHistory();
            // Un bloque de conexion que mientras se permanezca en el bloque la base de datos permanecera abierta y al terminar se destruira
            using (var dbContext = new ContextDB())
            {
                membershipDB = await dbContext.MembershipHistory.FirstOrDefaultAsync(m => m.Id == membership.Id);
            }
            return membershipDB!; // Retornamos el Registro Encontrado
        }
        #endregion

        #region METODO PARA BUSCAR REGISTROS MEDIANTE EL USO DE FILTROS
        // Metodo Para Buscar Por Filtros
        // IQueryable es una interfaz que toma un coleccion a la cual se le pueden implementar multiples consultas (Filtros)
        internal static IQueryable<MembershipHistory> QuerySelect(IQueryable<MembershipHistory> query, MembershipHistory membership)
        {
            // Por ID
            if (membership.Id > 0)
                query = query.Where(m => m.Id == membership.Id);

            if (!string.IsNullOrWhiteSpace(membership.Name))
                query = query.Where(m => m.Name.Contains(membership.Name));

            if (!string.IsNullOrWhiteSpace(membership.LastName))
                query = query.Where(m => m.LastName.Contains(membership.LastName));

            if (!string.IsNullOrWhiteSpace(membership.Dui))
                query = query.Where(m => m.Dui!.Contains(membership.Dui));

            if (!string.IsNullOrWhiteSpace(membership.InternalIdentityCode))
                query = query.Where(m => m.InternalIdentityCode.Contains(membership.InternalIdentityCode));

            if (!string.IsNullOrWhiteSpace(membership.Gender))
                query = query.Where(m => m.Gender.Contains(membership.Gender));

            // Ordenamos de Manera Desendente
            query = query.OrderByDescending(c => c.Id).AsQueryable();

            return query;
        }
        #endregion

        #region METODO PARA BUSCAR
        // Metodo para Buscar Registros Existentes
        public static async Task<List<MembershipHistory>> SearchAsync(MembershipHistory membership)
        {
            var memberships = new List<MembershipHistory>();
            using (var dbContext = new ContextDB())
            {
                var select = dbContext.MembershipHistory.AsQueryable();
                select = QuerySelect(select, membership);
                memberships = await select.ToListAsync();
            }
            return memberships;
        }
        #endregion
    }
}
