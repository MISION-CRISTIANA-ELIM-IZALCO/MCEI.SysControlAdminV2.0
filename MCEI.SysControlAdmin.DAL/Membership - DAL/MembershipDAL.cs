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
    }
}
