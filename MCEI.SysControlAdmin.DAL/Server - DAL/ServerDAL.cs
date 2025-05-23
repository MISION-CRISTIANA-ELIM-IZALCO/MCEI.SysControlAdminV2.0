﻿#region REFERENCIAS
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

        // Metodo para validar si ya existe el servidor en base al codigo de identidad interna
        private static async Task<bool> ExistServerAsignatedPreview(Server server, ContextDB contextDB)
        {
            var existingServer = await contextDB.Server.FirstOrDefaultAsync(s =>
                 s.IdMembership == server.IdMembership);

            return existingServer != null;
        }

        // Metodo Para Validar Si El Miembro Esta Activo
        private static async Task<bool> IsMembershipActive(int membershipId, ContextDB contextDB)
        {
            var membership = await contextDB.Membership.FirstOrDefaultAsync(m => m.Id == membershipId);
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
                    throw new Exception("Servidor ya existente, vuelve a intentarlo nuevamente.");
                }

                // Validar si el mimebo ya tiene un privilegio asociado osea ya es un servidor
                if (await ExistServerAsignatedPreview(server, contextDB))
                {
                    throw new Exception("Miembro ya asociado a un privilegio, vuelve a intentarlo nuevamente.");
                }

                // Validar si el miembro esta activo
                if (!await IsMembershipActive(server.IdMembership, contextDB))
                {
                    throw new Exception("No se puede crear el servidor ya que el Miembro no esta activo.");
                }

                // Validar si el privilegio esta activo
                if (!await IsPrivilegeActive(server.IdPrivilege, contextDB))
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

        #region METODO PARA MOSTRAR
        // Metodo Para Mostrar La Lista De Registros
        public static async Task<List<Server>> GetAllAsync()
        {
            var servers = new List<Server>();
            using (var dbContext = new ContextDB())
            {
                servers = await dbContext.Server.ToListAsync();
            }
            return servers;
        }
        #endregion

        #region METODO PARA OBTENER POR ID
        // Metodo Para Mostrar Un Registro En Base A Su Id
        public static async Task<Server> GetByIdAsync(Server server)
        {
            var serverDB = new Server();
            using (var dbContext = new ContextDB())
            {
                serverDB = await dbContext.Server.Include(m => m.Membership).Include(p => p.Privilege).FirstOrDefaultAsync(c => c.Id == server.Id);
            }
            return serverDB!;
        }
        #endregion

        #region METODO PARA BUSCAR REGISTROS MEDIANTE EL USO DE FILTROS
        // Metodo Para Buscar Por Filtros
        // IQueryable es una interfaz que toma un coleccion a la cual se le pueden implementar multiples consultas (Filtros)
        internal static IQueryable<Server> QuerySelect(IQueryable<Server> query, Server server)
        {
            // Por ID
            if (server.Id > 0)
                query = query.Where(c => c.Id == server.Id);

            // Por Miembro
            if (server.IdMembership > 0)
                query = query.Where(c => c.IdMembership == server.IdMembership);

            // Por Privilegio
            if (server.IdPrivilege > 0)
                query = query.Where(c => c.IdPrivilege == server.IdPrivilege);

            // Ordenamos de Manera Desendente
            query = query.OrderByDescending(c => c.Id).AsQueryable();

            return query;
        }
        #endregion

        #region METODO PARA BUSCAR
        // Metodo para Buscar Registros Existentes
        public static async Task<List<Server>> SearchAsync(Server server)
        {
            var servers = new List<Server>();
            using (var dbContext = new ContextDB())
            {
                var select = dbContext.Server.AsQueryable();
                select = QuerySelect(select, server);
                servers = await select.ToListAsync();
            }
            return servers;
        }
        #endregion

        #region METODO PARA INCLUIR MEMBRESIA Y PRIVILEGIOS
        // Método que incluye el membresia y el privilegio para la búsqueda
        public static async Task<List<Server>> SearchIncludeAsync(Server server)
        {
            var servers = new List<Server>();
            using (var dbContext = new ContextDB())
            {
                var select = dbContext.Server.AsQueryable();
                select = QuerySelect(select, server).Include(c => c.Membership).Include(c => c.Privilege).AsQueryable();
                servers = await select.ToListAsync();
            }
            return servers;
        }
        #endregion

        #region METODO PARA MODIFICAR
        public static async Task<int> UpdateAsync(Server server)
        {
            if (server == null)
            {
                throw new ArgumentNullException(nameof(server), "El Servidor no puede ser nula.");
            }

            int result = 0;

            using (var dbContext = new ContextDB())
            {
                var serverDB = await dbContext.Server.FirstOrDefaultAsync(c => c.Id == server.Id);
                if (serverDB == null)
                {
                    throw new Exception("Servidor no encontrado para actualizar.");
                }

                // Validar si ya existe el servidor
                if (await ExistServer(server, dbContext))
                {
                    throw new Exception("Servidor ya existente, vuelve a intentarlo nuevamente.");
                }

                // Validar si el miembro está activo
                if (!await IsMembershipActive(server.IdMembership, dbContext))
                {
                    throw new Exception("No se puede modificar el servidor ya que el Miembro no está activo.");
                }

                // Validar si el privilegio está activo
                if (!await IsPrivilegeActive(server.IdPrivilege, dbContext))
                {
                    throw new Exception("No se puede modificar el servidor ya que el Privilegio no está activo.");
                }

                // Actualizar la asignación en la base de datos
                serverDB.IdMembership = server.IdMembership;
                serverDB.IdPrivilege = server.IdPrivilege;
                serverDB.Status = server.Status;
                serverDB.DateModification = server.DateModification;

                dbContext.Update(serverDB);
                result = await dbContext.SaveChangesAsync();
            }

            return result;
        }
        #endregion

        #region METODO PARA ELIMINAR
        // Metodo Para Eliminar Un Registro Existente En La Base De Datos
        public static async Task<int> DeleteAsync(Server server)
        {
            int result = 0;
            // Un bloque de conexion que mientras se permanezca en el bloque la base de datos permanecera abierta y al terminar se destruira
            using (var dbContext = new ContextDB())
            {
                var serverDB = await dbContext.Server.FirstOrDefaultAsync(c => c.Id == server.Id);
                if (serverDB != null)
                {
                    dbContext.Server.Remove(serverDB);
                    result = await dbContext.SaveChangesAsync();
                }
            }
            return result;
        }
        #endregion

        #region METODO PARA OBTENER DICCIONARIO DE SERVIDORES TOTALES Y AGRUPADOS POR PRIVILEGIO
        public static async Task<Dictionary<int, List<Server>>> GetActiveServersGroupedByPrivilegeAsync()
        {
            using (var dbContext = new ContextDB())
            {
                var activeServers = await dbContext.Server
                    .Where(s => s.Status == 1) // Filtrar por status = 1
                    .Include(s => s.Membership) // Incluir propiedades relacionadas si es necesario
                    .Include(s => s.Privilege)
                    .ToListAsync();

                // Agrupar por PrivilegeId
                var groupedServers = activeServers
                    .GroupBy(s => s.IdPrivilege) // Agrupar por IdPrivilege
                    .ToDictionary(g => g.Key, g => g.ToList()); // Convertir a un diccionario

                return groupedServers;
            }
        }
        #endregion

        #region METODOS PARA OBTENCION DE DATOS PARA EL DASHBOARD
        // Metodo para obtener el total de servidores
        public static async Task<int> GetTotalCountAsync()
        {
            using (var dbContext = new ContextDB())
            {
                return await dbContext.Server.CountAsync();
            }
        }

        // Metodo para obtener el total de servidores por genero masculino y femenino
        public static async Task<(int masculino, int femenino)> GetServerByGenderAsync()
        {
            using (var dbContext = new ContextDB())
            {
                var masculino = await dbContext.Server.Where(s => s.Membership!.Gender == "MASCULINO").CountAsync();
                var femenino = await dbContext.Server.Where(s => s.Membership!.Gender == "FEMENINO").CountAsync();

                return (masculino, femenino);
            }
        }

        // Metodo para obtener el total de servidores bautisados y no bautisados por el espiritu santo
        public static async Task<(int si, int no)> GetBautizadosEspirituSantoAsync()
        {
            using ( var dbContext = new ContextDB())
            {
                var si = await dbContext.Server.Where(s => s.Membership!.BaptismOfTheHolySpirit == "SI").CountAsync();
                var no = await dbContext.Server.Where(s => s.Membership!.BaptismOfTheHolySpirit == "NO").CountAsync();

                return (si, no);
            }
        }

        // Metodo para obtener y mostrar los servidores por su privilegio asignado
        public async Task<Dictionary<string, int>> GetServerCountByPrivilegeAsync()
        {
            using (var context = new ContextDB())
            {
                var allPrivileges = await context.Privilege.ToListAsync();

                var serversGrouped = await context.Server
                    .GroupBy(s => s.IdPrivilege)
                    .Select(g => new { PrivilegeId = g.Key, Count = g.Count() })
                    .ToListAsync();

                var result = allPrivileges
                    .Select(p => new
                    {
                        PrivilegeName = p.Name,
                        Count = serversGrouped.FirstOrDefault(g => g.PrivilegeId == p.Id)?.Count ?? 0
                    })
                    .ToDictionary(x => x.PrivilegeName, x => x.Count);

                return result;
            }
        }
        #endregion
    }
}
