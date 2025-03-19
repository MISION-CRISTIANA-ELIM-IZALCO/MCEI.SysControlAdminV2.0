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
    }
}
