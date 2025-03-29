#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.EN.Privilege___EN;
using MCEI.SysControlAdmin.DAL.Privilege___DAL;

#endregion

namespace MCEI.SysControlAdmin.BL.Privilege___BL
{
    public class PrivilegeBL
    {
        #region METODO PARA CREAR
        // Metodo para guardar un nuevo registro
        public async Task<int> CreateAsync(Privilege privilege)
        {
            return await PrivilegeDAL.CreateAsync(privilege);
        }
        #endregion
    }
}
