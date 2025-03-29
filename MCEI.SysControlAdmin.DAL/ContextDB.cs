#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento
using Microsoft.EntityFrameworkCore;
using MCEI.SysControlAdmin.EN.Role___EN;
using MCEI.SysControlAdmin.EN.User___EN;
using MCEI.SysControlAdmin.EN.Membership___EN;
using MCEI.SysControlAdmin.EN.MembershipHistory___EN;
using MCEI.SysControlAdmin.EN.Privilege___EN;

#endregion

namespace MCEI.SysControlAdmin.DAL
{
    public class ContextDB : DbContext
    {
        #region REFERENCIAS DE TABLAS DE LA BD
        //Coleccion que hace referencia a las tablas de la base de datos
        public DbSet<Role> Role { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Membership> Membership { get; set; }
        public DbSet<MembershipHistory> MembershipHistory { get; set; }
        public DbSet<Privilege> Privilege { get; set; }
        #endregion

        #region STRING DE CONEXION
        // Metodo de Conexion a la Base de Datos
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=localhost;Initial Catalog=MCEISysControlAdminDB;Integrated Security=True;Trust Server Certificate=True"); // String de Conexion
        }
        #endregion
    }
}
