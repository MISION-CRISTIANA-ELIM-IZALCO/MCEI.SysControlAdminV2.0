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

        // Metodo para que admita int al hacer uso del metodo antecesor para automatizacion
        public async Task<Membership> GetByIdAsync(int id)
        {
            // Crear una instancia de Membership y asignarle el ID
            var membership = new Membership { Id  = id };

            // Llamar al método existente con el objeto Membership
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

        #region METODO PARA MODIFICAR
        // Metodo Para Guardar Un Nuevo Registro
        public async Task<int> UpdateAsync(Membership membership)
        {
            return await MembershipDAL.UpdateAsync(membership);
        }
        #endregion

        #region METODO PARA ELIMINAR
        // Metodo Para Eliminar Un Registro Existente En La Base De Datos
        public async Task<int> DeleteAsync(Membership membership)
        {
            return await MembershipDAL.DeleteAsync(membership);
        }
        #endregion

        #region METODOS PARA OBTENCION DE DATOS PARA EL DASHBOARD
        // Metodo para obtener el total de miembros
        public async Task<int> GetTotalCountAsync()
        {
            return await MembershipDAL.GetTotalCountAsync();
        }

        // Metodo para obtener la edad de los miembros y categorizarla
        public Dictionary<string, int> GetMembershipsAgeCategories()
        {
            MembershipDAL membershipDAL = new MembershipDAL();
            return membershipDAL.GetMembershipsByAgeCategory();
        }

        // Metodo para obtener el total de miembros por genero masculino y femenino
        public async Task<(int masculino, int femenino)> GetMembershipsByGenderAsync()
        {
            return await MembershipDAL.GetMembershipsByGenderAsync();
        }

        // Metodo para obtener el total de miembros segun su estado civil
        public async Task<Dictionary<string, int>> GetTotalByEstadoCivilAsync()
        {
            MembershipDAL membershipDAL = new MembershipDAL();
            return await membershipDAL.GetTotalByEstadoCivilAsync();
        }

        // Metodo para obtener el total de miembros segun si son o no bautisados por el espiritu santo
        public async Task<(int bautizados, int noBautizados)> GetBautizadosEspirituSantoAsync()
        {
            MembershipDAL membershipDAL = new MembershipDAL();
            return await membershipDAL.GetBautizadosEspirituSantoAsync();
        }

        // Método para obtener el total de miembros por estado (Activo/Inactivo)
        public async Task<(int totalActivos, int totalInactivos)> GetTotalByStatusAsync()
        {
            return await MembershipDAL.GetTotalByStatusAsync();
        }
        #endregion
    }
}
