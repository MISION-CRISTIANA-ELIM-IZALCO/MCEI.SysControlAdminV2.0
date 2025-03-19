﻿#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.DAL.User___DAL;
using MCEI.SysControlAdmin.EN.User___EN;


#endregion

namespace MCEI.SysControlAdmin.BL.User___BL
{
    public class UserBL
    {
        #region METODO PARA CREAR
        // Metodo Para Guardar Un Nuevo Registro a La Base De Datos
        public async Task<int> CreateAsync(User user)
        {
            return await UserDAL.CreateAsync(user);
        }
        #endregion

        #region METODO PARA OBTENER TODOS
        // Metodo Para Listar y Mostrar Todos Los Resultados
        public async Task<List<User>> GetAllAsync()
        {
            return await UserDAL.GetAllAsync();
        }
        #endregion

        #region METODO PARA OBTENER POR ID
        // Metodo Para Obtener Un Registro En Base a Su Id
        public async Task<User> GetByIdAsync(User user)
        {
            return await UserDAL.GetByIdAsync(user);
        }
        #endregion

        #region METODO PARA BUSCAR
        // Metodo Para Buscar Registros En La Base De Datos
        public async Task<List<User>> SearchAsync(User user)
        {
            return await UserDAL.SearchAsync(user);
        }
        #endregion

        #region METODO PARA BUSCAR INCLUYENGO LA LLAVE FORANEA
        // Metodo Para Buscar Registros Incluyendo Las Llaves Foraneas
        public async Task<List<User>> SearchIncludeRoleAsync(User user)
        {
            return await UserDAL.SearchIncludeRoleAsync(user);
        }
        #endregion

        #region METODO PARA MODIFICAR
        // Metodo Para Modificar Un Registro Existente En La Base De Datos
        public async Task<int> UpdateAsync(User user)
        {
            return await UserDAL.UpdateAsync(user);
        }
        #endregion

        #region METODO PARA ELIMINAR
        // Metodo Para Eliminar Un Registro Existente En La Base De Datos
        public async Task<int> DeleteAsync(User user)
        {
            return await UserDAL.DeleteAsync(user);
        }
        #endregion
    }
}
