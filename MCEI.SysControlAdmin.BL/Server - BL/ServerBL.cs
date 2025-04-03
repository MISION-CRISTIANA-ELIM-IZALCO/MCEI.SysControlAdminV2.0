#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.DAL.Server___DAL;
using MCEI.SysControlAdmin.EN.Server___EN;

#endregion

namespace MCEI.SysControlAdmin.BL.Server___BL
{
    public class ServerBL
    {
        #region METODO PARA CREAR
        // Metodo Para Guardar Un Nuevo Registro
        public async Task<int> CreateAsync(Server server)
        {
            return await ServerDAL.CreateAsync(server);
        }
        #endregion

        #region METODO PARA MOSTRAR
        // Metodo Para Mostrar Una Lista De Registros
        public async Task<List<Server>> GetAllAsync()
        {
            return await ServerDAL.GetAllAsync();
        }
        #endregion

        #region METODO PARA MOSTRAR POR ID
        // Metodo Para Mostrar Un Registro Especifico Bajo Un Id
        public async Task<Server> GetByIdAsync(Server server)
        {
            return await ServerDAL.GetByIdAsync(server);
        }
        #endregion

        #region METODO PARA BUSCAR
        // Metodo Para Buscar Registros Existentes
        public async Task<List<Server>> SearchAsync(Server server)
        {
            return await ServerDAL.SearchAsync(server);
        }
        #endregion

        #region METODO PARA INCLUIR PRIVILEGIO Y PRIVILEGIO
        public async Task<List<Server>> SearchIncludeAsync(Server server)
        {
            return await ServerDAL.SearchIncludeAsync(server);
        }
        #endregion

        #region METODO PARA MODIFICAR
        // Metodo Para Modificar Un Registro Existente
        public async Task<int> UpdateAsync(Server server)
        {
            return await ServerDAL.UpdateAsync(server);
        }
        #endregion

        #region METODO PARA ELIMINAR
        // Metodo Para Eliminar Un Registro Existente En La Base De Datos
        public async Task<int> DeleteAsync(Server server)
        {
            return await ServerDAL.DeleteAsync(server);
        }
        #endregion
    }
}
