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
    }
}
