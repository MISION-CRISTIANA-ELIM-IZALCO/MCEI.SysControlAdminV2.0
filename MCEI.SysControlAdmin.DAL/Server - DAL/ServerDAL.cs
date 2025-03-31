#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.EN.Server___EN;
using Microsoft.EntityFrameworkCore;


#endregion

namespace MCEI.SysControlAdmin.DAL.Server___DAL
{
    public class ServerDAL
    {
        #region METODO PARA VALIDAR UNICA EXISTENCIA DEL REGISTRO Y OTRAS METODOS PARA VALIDACIONES EXTRAS
        // Metodo Para Validar La Unica Existencia De Un Registro y No Haber Duplicidad
        private static async Task<bool> ExistServer(Server server, ContextDB contextDB)
        {
            var existingServer = await contextDB.Server.FirstOrDefaultAsync(s =>
                s.IdMembership == server.IdMembership &&
                s.IdPrivilege == server.IdPrivilege &&
                s.Id != server.Id);

            return existingServer != null;
        }

        // Metodo Para Validar Si El Miembro Esta Activo
        private static async Task<bool> IsMembershipActive(int membershipId, ContextDB contextDB)
        {
            var membership = await contextDB.Server.FirstOrDefaultAsync(m => m.Id == membershipId);
            if (membership == null)
            {
                throw new Exception("El Miembro No Existe");
            }
            return membership.Status == 1; // Retorna true si el miembro esta activo
        }

        // Metodo Para Validar Si El Privilegio Esta Activo
        private static async Task<bool> IsPrivilegeActive(int privilegeId, ContextDB contextDB)
        {
            var privilege = await contextDB.Privilege.FirstOrDefaultAsync(p => p.Id == privilegeId);
            if (privilege == null)
            {
                throw new Exception("Privilegio No Existente");
            }
            return privilege.Status == 1; // Retorna true si el privilegio esta activo
        }
        #endregion

        #region METODO PARA CREAR
        // Metodo para crear un nuevo registro
        public static async Task<int> CreateAsync(Server server)
        {
            if (server == null)
            {
                throw new ArgumentNullException(nameof(server), "El Servidor no puede ser nulo.");
            }

            int result = 0;

            using (var contextDB = new ContextDB())
            {
                // Validar si ya existe el servidor
                if (await ExistServer(server, contextDB))
                {
                    throw new Exception("Privilegio ya existente, vuelve a intentarlo nuevamente.");
                }

                // Validar si el miembro esta activo
                if (await IsMembershipActive(server.IdMembership, contextDB))
                {
                    throw new Exception("No se puede crear el servidor ya que el Miembro no esta activo.");
                }

                // Validar si el miembro esta activo
                if (await IsPrivilegeActive(server.IdPrivilege, contextDB))
                {
                    throw new Exception("No se puede crear el servidor ya que el Privilegio no esta activo.");
                }

                // Guardar el servidor a la base de datos
                contextDB.Server.Add(server);
                result = await contextDB.SaveChangesAsync();
            }
            return result;
        }
        #endregion
    }
}
