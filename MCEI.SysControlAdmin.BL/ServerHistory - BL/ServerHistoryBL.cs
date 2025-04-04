#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.DAL.ServerHistory___DAL;
using MCEI.SysControlAdmin.EN.ServerHistory___EN;

#endregion

namespace MCEI.SysControlAdmin.BL.ServerHistory___BL
{
    public class ServerHistoryBL
    {
        #region METODO PARA CREAR
        // Metodo para guardar un nuevo registro
        public async Task<int> CreateAsync(ServerHistory serverHistory)
        {
            return await ServerHistoryDAL.CreateAsync(serverHistory);
        }
        #endregion

        #region METODO PARA MOSTRAR
        // Metodo Para Mostrar Una Lista De Registros
        public async Task<List<ServerHistory>> GetAllAsync()
        {
            return await ServerHistoryDAL.GetAllAsync();
        }
        #endregion

        #region METODO PARA MOSTRAR POR ID
        // Metodo Para Mostrar Un Registro Especifico Bajo Un Id
        public async Task<ServerHistory> GetByIdAsync(ServerHistory serverHistory)
        {
            return await ServerHistoryDAL.GetByIdAsync(serverHistory);
        }
        #endregion

        #region METODO PARA BUSCAR
        // Metodo Para Buscar Registros Existentes
        public async Task<List<ServerHistory>> SearchAsync(ServerHistory serverHistory)
        {
            return await ServerHistoryDAL.SearchAsync(serverHistory);
        }
        #endregion
    }
}
