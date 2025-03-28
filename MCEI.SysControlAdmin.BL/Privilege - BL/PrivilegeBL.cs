﻿#region REFERENCIAS
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

        #region METODO PARA OBTENER POR ID
        // Metodo para listar y mostrar todos los resultados
        public async Task<List<Privilege>> GetAllAsync()
        {
            return await PrivilegeDAL.GetAllAsync();
        }
        #endregion

        #region METODO PARA OBTENER POR ID
        // Metodo para obtener un registro por su id
        public async Task<Privilege> GetByIdAsync(Privilege privilege)
        {
            return await PrivilegeDAL.GetByIdAsync(privilege);
        }
        #endregion

        #region METODO PARA BUSCAR
        // Metodo para buscar registros existentes en la base de datos
        public async Task<List<Privilege>> SearchAsync(Privilege privilege)
        {
            return await PrivilegeDAL.SearchAsync(privilege);
        }
        #endregion

        #region METODO PARA MODIFICAR
        // Metodo para modificar un resgistro existente
        public async Task<int> UpdateAsync(Privilege privilege)
        {
            return await PrivilegeDAL.UpdateAsync(privilege);
        }
        #endregion
    }
}
