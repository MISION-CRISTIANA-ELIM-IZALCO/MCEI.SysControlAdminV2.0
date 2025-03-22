#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.EN.Membership___EN;
using MCEI.SysControlAdmin.DAL.Membership___DAL;


#endregion

namespace MCEI.SysControlAdmin.BL.Membership___BL
{
    public class MembershipBL
    {
        #region METODO PARA CREAR
        // Metodo para guardar un nuevo registro
        public async Task<int> CreateAsync(Membership membership)
        {
            return await MembershipDAL.CreateAsync(membership);
        }
        #endregion
    }
}
