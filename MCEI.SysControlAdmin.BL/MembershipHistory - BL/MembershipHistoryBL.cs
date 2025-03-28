#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.DAL.MembershipHistory___DAL;
using MCEI.SysControlAdmin.EN.MembershipHistory___EN;

#endregion

namespace MCEI.SysControlAdmin.BL.MembershipHistory___BL
{
    public class MembershipHistoryBL
    {
        #region METODO PARA CREAR
        // Metodo para guardar un nuevo registro
        public async Task<int> CreateAsync(MembershipHistory membership)
        {
            return await MembershipHistoryDAL.CreateAsync(membership);
        }
        #endregion

        #region METODO PARA MOSTRAR
        // Metodo Para Mostrar Una Lista De Registros
        public async Task<List<MembershipHistory>> GetAllAsync()
        {
            return await MembershipHistoryDAL.GetAllAsync();
        }
        #endregion

        #region METODO PARA MOSTRAR POR ID
        // Metodo Para Mostrar Un Registro Especifico Bajo Un Id
        public async Task<MembershipHistory> GetByIdAsync(MembershipHistory membership)
        {
            return await MembershipHistoryDAL.GetByIdAsync(membership);
        }
        #endregion

        #region METODO PARA BUSCAR
        // Metodo Para Buscar Registros Existentes
        public async Task<List<MembershipHistory>> SearchAsync(MembershipHistory membership)
        {
            return await MembershipHistoryDAL.SearchAsync(membership);
        }
        #endregion
    }
}
