#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.EN.Membership___EN;
using Microsoft.EntityFrameworkCore;


#endregion

namespace MCEI.SysControlAdmin.DAL.Membership___DAL
{
    public class MembershipDAL
    {
        #region METODO PARA VALIDAR UNICA EXISTENCIA DEL REGISTRO
        // Metodo para validar la unica existencia del registro y no haber duplicidad
        private static async Task<bool> ExistMembership(Membership membership, ContextDB contextDB)
        {
            bool result = false;
            var memberships = await contextDB.Membership.FirstOrDefaultAsync(m => m.InternalIdentityCode == membership.InternalIdentityCode && m.Id != membership.Id);
            if (memberships != null && memberships.Id > 0 && memberships.InternalIdentityCode == membership.InternalIdentityCode)
                result = true;
            return result;
        }
        #endregion

        #region METODO PARA CREAR
        // Metodo para crear agregar un nuevo registro a la base de datos
        public static async Task<int> CreateAsync(Membership membership)
        {
            if (membership == null)
            {
                throw new ArgumentNullException(nameof(membership), "La Membresia no puede ser nulo.");
            }

            int result = 0;
            using (var contextDB = new ContextDB())
            {
                // Verificamos si la membresia ya existe
                bool membershipExists = await ExistMembership(membership, contextDB);
                if (membershipExists)
                {
                    throw new Exception("Membresia Ya Existente, Vuelve a Intentarlo Nuevamente.");
                }

                // Guardar la membresi en la base de datos
                contextDB.Membership.Add(membership);
                result = await contextDB.SaveChangesAsync();
            }
            return result;
        }
        #endregion

        #region METODO PARA MOSTRAR
        // Metodo Para Mostrar La Lista De Registros
        public static async Task<List<Membership>> GetAllAsync()
        {
            var membership = new List<Membership>();
            using (var dbContext = new ContextDB())
            {
                membership = await dbContext.Membership.ToListAsync();
            }
            return membership;
        }
        #endregion

        #region METODO PARA OBTENER POR ID
        // Metodo Para Mostrar Un Registro En Base A Su Id
        public static async Task<Membership> GetByIdAsync(Membership membership)
        {
            var membershipDB = new Membership();
            // Un bloque de conexion que mientras se permanezca en el bloque la base de datos permanecera abierta y al terminar se destruira
            using (var dbContext = new ContextDB())
            {
                membershipDB = await dbContext.Membership.FirstOrDefaultAsync(m => m.Id == membership.Id);
            }
            return membershipDB!; // Retornamos el Registro Encontrado
        }
        #endregion

        #region METODO PARA BUSCAR REGISTROS MEDIANTE EL USO DE FILTROS
        // Metodo Para Buscar Por Filtros
        // IQueryable es una interfaz que toma un coleccion a la cual se le pueden implementar multiples consultas (Filtros)
        internal static IQueryable<Membership> QuerySelect(IQueryable<Membership> query, Membership membership)
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
        public static async Task<List<Membership>> SearchAsync(Membership membership)
        {
            var memberships = new List<Membership>();
            using (var dbContext = new ContextDB())
            {
                var select = dbContext.Membership.AsQueryable();
                select = QuerySelect(select, membership);
                memberships = await select.ToListAsync();
            }
            return memberships;
        }
        #endregion
    }
}
