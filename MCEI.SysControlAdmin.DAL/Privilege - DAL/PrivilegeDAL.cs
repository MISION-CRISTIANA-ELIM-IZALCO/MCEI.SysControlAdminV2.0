#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.EN.Privilege___EN;
using Microsoft.EntityFrameworkCore;


#endregion

namespace MCEI.SysControlAdmin.DAL.Privilege___DAL
{
    public class PrivilegeDAL
    {
        #region METODO PARA VALIDAR UNICA EXISTENCIA DEL REGISTRO
        // Metodo Para Validar La Unica Existencia De Un Registro y No Haber Duplicidad
        private static async Task<bool> ExistPrivilege(Privilege privilege, ContextDB contextDB)
        {
            bool result = false;
            var privileges = await contextDB.Privilege.FirstOrDefaultAsync(p => p.Name == privilege.Name && p.Id != privilege.Id);
            if (privileges != null && privileges.Id > 0 && privileges.Name == privilege.Name)
                result = true;
            return result;
        }
        #endregion

        #region METODO PARA GUARDAR
        //Metodo Para Guardar Un Nuevo Registro En La Base De Datos
        public static async Task<int> CreateAsync(Privilege privilege)
        {
            int result = 0;
            using (var contextDB = new ContextDB())
            {
                // Verificamos si el privilegio ya existe
                bool existPrivilege = await ExistPrivilege(privilege, contextDB);
                if (existPrivilege)
                {
                    throw new Exception("Privilegio ya existente, vuelve a intentarlo nuevamente.");
                }

                // Guardar el privilegio en la base de datos
                contextDB.Privilege.Add(privilege);
                result = await contextDB.SaveChangesAsync();
            }
            return result;
        }
        #endregion
    }
}
