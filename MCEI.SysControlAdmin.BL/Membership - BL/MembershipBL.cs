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

        #region METODO PARA MOSTRAR
        // Metodo Para Mostrar Una Lista De Registros
        public async Task<List<Membership>> GetAllAsync()
        {
            return await MembershipDAL.GetAllAsync();
        }
        #endregion

        #region METODO PARA MOSTRAR POR ID
        // Metodo Para Mostrar Un Registro Especifico Bajo Un Id
        public async Task<Membership> GetByIdAsync(Membership membership)
        {
            return await MembershipDAL.GetByIdAsync(membership);
        }
        #endregion

        #region METODO PARA BUSCAR
        // Metodo Para Buscar Registros Existentes
        public async Task<List<Membership>> SearchAsync(Membership membership)
        {
            return await MembershipDAL.SearchAsync(membership);
        }
        #endregion
    }
}
