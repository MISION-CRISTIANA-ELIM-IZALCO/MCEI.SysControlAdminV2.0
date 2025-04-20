#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.EN.Privilege___EN;
using MCEI.SysControlAdmin.EN.Role___EN;
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

        #region METODO PARA MOSTRAR TODOS
        // Metodo para listar y mostrar todos los resultados
        public static async Task<List<Privilege>> GetAllAsync()
        {
            var privileges = new List<Privilege>();
            using (var contextDB = new ContextDB())
            {
                privileges = await contextDB.Privilege.ToListAsync();
            }
            return privileges;
        }
        #endregion

        #region METODO PARA OBTENER POR ID
        // Metodo para obtener un registro por su id
        public static async Task<Privilege> GetByIdAsync(Privilege privilege)
        {
            var privilegeDb = new Privilege();
            using (var contextDB = new ContextDB())
            {
                privilegeDb = await contextDB.Privilege.FirstOrDefaultAsync(p => p.Id == privilege.Id);
            }
            return privilegeDb!;
        }
        #endregion

        #region METODO PARA FILTRAR BUSQUEDA
        // Metodo para filtrar la busqueda por parametros
        internal static IQueryable<Privilege> QuerySelect(IQueryable<Privilege> query, Privilege privilege)
        {
            if (privilege.Id > 0)
                query = query.Where(r => r.Id == privilege.Id);

            if (!string.IsNullOrWhiteSpace(privilege.Name))
                query = query.Where(r => r.Name.Contains(privilege.Name));

            query = query.OrderByDescending(r => r.Id).AsQueryable();

            return query;
        }
        #endregion

        #region METODO PARA BUSCAR
        // Metodo para buscar registros existentes en la base de datos
        public static async Task<List<Privilege>> SearchAsync(Privilege privilege)
        {
            var privileges = new List<Privilege>();
            using (var contextDB = new ContextDB())
            {
                var select = contextDB.Privilege.AsQueryable();
                select = QuerySelect(select, privilege);
                privileges = await select.ToListAsync();
            }
            return privileges;
        }
        #endregion

        #region METODO PARA MODIFICAR
        // Metodo para modifciar un registro
        public static async Task<int> UpdateAsync(Privilege privilege)
        {
            int result = 0;

            using (var contextDB = new ContextDB())
            {
                var privilegeDB = await contextDB.Privilege.FirstOrDefaultAsync(p => p.Id == privilege.Id);
                if (privilegeDB == null)
                {
                    throw new Exception("Privilegio no encontrada para actualizar.");
                }

                // Verificamos si ya existe otro privilegio con el mismo nombre
                bool privilegeExists = await contextDB.Privilege.AnyAsync(p => p.Id != privilege.Id && p.Name == privilege.Name);
                if (privilegeExists)
                {
                    throw new Exception("Ya existe otro privilegio con el mismo nombre. Vuelve a intentarlo.");
                }

                // Actualizar los datos del privilegio
                privilegeDB.Name = privilege.Name;
                privilegeDB.Status = privilege.Status;
                privilegeDB.DateModification = privilege.DateModification;

                contextDB.Update(privilegeDB);
                result = await contextDB.SaveChangesAsync();
            }
            return result;
        }
        #endregion

        #region METODO PARA ELIMINAR
        // Metodo para eliminar un registro existente en la base de datos
        public static async Task<int> DeleteAsync(Privilege privilege)
        {
            int result = 0;
            using (var contextDB = new ContextDB())
            {
                var privilegeDB = await contextDB.Privilege.FirstOrDefaultAsync(p => p.Id == privilege.Id);
                if (privilegeDB != null)
                {
                    contextDB.Privilege.Remove(privilegeDB);
                    result = await contextDB.SaveChangesAsync();
                }
                else
                    throw new Exception("Privilegio no encontrado");
            }
            return result;
        }
        #endregion

        #region METODOS PARA OBTENCION DE DATOS PARA EL DASHBOARD
        // Metodo para obtener el total de privilegios
        public static async Task<int> GetTotalCountAsync()
        {
            using (var dbContext = new ContextDB())
            {
                return await dbContext.Privilege.CountAsync();
            }
        }
        #endregion
    }
}
