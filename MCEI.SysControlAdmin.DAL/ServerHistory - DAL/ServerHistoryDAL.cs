#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.EN.ServerHistory___EN;
using Microsoft.EntityFrameworkCore;


#endregion

namespace MCEI.SysControlAdmin.DAL.ServerHistory___DAL
{
    public class ServerHistoryDAL
    {
        #region METODO PARA CREAR
        // Metodo para crear agregar un nuevo registro a la base de datos
        public static async Task<int> CreateAsync(ServerHistory serverHistory)
        {
            if (serverHistory == null)
            {
                throw new ArgumentNullException(nameof(serverHistory), "El Servidor no puede ser nulo.");
            }

            int result = 0;
            using (var contextDB = new ContextDB())
            {
                contextDB.ServerHistory.Add(serverHistory);
                result = await contextDB.SaveChangesAsync();
            }
            return result;
        }
        #endregion

        #region METODO PARA MOSTRAR
        // Metodo Para Mostrar La Lista De Registros
        public static async Task<List<ServerHistory>> GetAllAsync()
        {
            var serverHistory = new List<ServerHistory>();
            using (var dbContext = new ContextDB())
            {
                serverHistory = await dbContext.ServerHistory.ToListAsync();
            }
            return serverHistory;
        }
        #endregion

        #region METODO PARA OBTENER POR ID
        // Metodo Para Mostrar Un Registro En Base A Su Id
        public static async Task<ServerHistory> GetByIdAsync(ServerHistory serverHistory)
        {
            var serverHistoryDB = new ServerHistory();
            using (var dbContext = new ContextDB())
            {
                serverHistoryDB = await dbContext.ServerHistory.FirstOrDefaultAsync(m => m.Id == serverHistory.Id);
            }
            return serverHistoryDB!; // Retornamos el Registro Encontrado
        }
        #endregion

        #region METODO PARA BUSCAR REGISTROS MEDIANTE EL USO DE FILTROS
        // Metodo Para Buscar Por Filtros
        // IQueryable es una interfaz que toma un coleccion a la cual se le pueden implementar multiples consultas (Filtros)
        internal static IQueryable<ServerHistory> QuerySelect(IQueryable<ServerHistory> query, ServerHistory serverHistory)
        {
            // Por ID
            if (serverHistory.Id > 0)
                query = query.Where(m => m.Id == serverHistory.Id);

            // Por Membresia
            if (serverHistory.IdMembership > 0)
                query = query.Where(c => c.IdMembership == serverHistory.IdMembership);

            // Por Privilegio
            if (serverHistory.IdPrivilege > 0)
                query = query.Where(c => c.IdPrivilege == serverHistory.IdPrivilege);

            // Ordenamos de Manera Desendente
            query = query.OrderByDescending(c => c.Id).AsQueryable();

            return query;
        }
        #endregion

        #region METODO PARA BUSCAR
        // Metodo para Buscar Registros Existentes
        public static async Task<List<ServerHistory>> SearchAsync(ServerHistory serverHistory)
        {
            var serverHistorys = new List<ServerHistory>();
            using (var dbContext = new ContextDB())
            {
                var select = dbContext.ServerHistory.AsQueryable();
                select = QuerySelect(select, serverHistory);
                serverHistorys = await select.ToListAsync();
            }
            return serverHistorys;
        }
        #endregion
    }
}
