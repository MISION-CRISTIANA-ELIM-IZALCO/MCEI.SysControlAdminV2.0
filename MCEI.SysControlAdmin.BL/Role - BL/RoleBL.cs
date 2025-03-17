#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.DAL.Role___DAL;
using MCEI.SysControlAdmin.EN.Role___EN;

#endregion

namespace MCEI.SysControlAdmin.BL.Role___BL
{
    public class RoleBL
    {
        #region METODO PARA GUARDAR
        // Metodo Para Guardar Un Nuevo Registro a La Base De Datos
        public async Task<int> CreateAsync(Role role)
        {
            return await RoleDAL.CreateAsync(role);
        }
        #endregion
    }
}
