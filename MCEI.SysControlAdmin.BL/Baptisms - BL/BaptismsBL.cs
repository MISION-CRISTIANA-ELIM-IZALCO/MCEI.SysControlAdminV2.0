#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.EN.Baptisms___EN;
using MCEI.SysControlAdmin.DAL.Baptisms___DAL;


#endregion

namespace MCEI.SysControlAdmin.BL.Baptisms___BL
{
    public class BaptismsBL
    {
        #region METODO PARA CREAR
        // Metodo para guardar un nuevo registro
        public async Task<int> CreateAsync(Baptisms baptisms)
        {
            return await BaptismsDAL.CreateAsync(baptisms);
        }
        #endregion

        #region METODO PARA MOSTRAR
        // Metodo para mostrar una lista de registros
        public async Task<List<Baptisms>> GetAllAsync()
        {
            return await BaptismsDAL.GetAllAsync();
        }
        #endregion

        #region METODO PARA MOSTRAR POR ID
        // Metodo para mostrar un registro especifico bajo un id
        public async Task<Baptisms> GetByIdAsync(Baptisms baptisms)
        {
            return await BaptismsDAL.GetByIdAsync(baptisms);
        }
        #endregion

        #region METODO PARA BUSCAR
        // Metodo para buscar registros existentes
        public async Task<List<Baptisms>> SearchAsync(Baptisms baptisms)
        {
            return await BaptismsDAL.SearchAsync(baptisms);
        }
        #endregion

        #region METODO PARA MODIFICAR
        // Metodo para modificar un registro existente
        public async Task<int> UpdateAsync(Baptisms baptisms)
        {
            return await BaptismsDAL.UpdateAsync(baptisms);
        }
        #endregion

        #region METODO PARA ELIMINAR
        // Metodo para eliminar un registro existente
        public async Task<int> DeleteAsync(Baptisms baptisms)
        {
            return await BaptismsDAL.DeleteAsync(baptisms);
        }
        #endregion
    }
}
