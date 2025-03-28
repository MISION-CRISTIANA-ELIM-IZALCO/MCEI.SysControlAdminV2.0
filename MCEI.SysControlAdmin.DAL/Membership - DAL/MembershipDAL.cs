﻿#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.EN.Membership___EN;
using Microsoft.EntityFrameworkCore;


#endregion

namespace MCEI.SysControlAdmin.DAL.Membership___DAL
{
    public class MembershipDAL
    {
        #region METODO PARA VALIDAR UNICA EXISTENCIA DEL REGISTRO
        // Metodo para validar la unica existencia del registro y no haber duplicidad
        private static async Task<bool> ExistMembership(Membership membership, ContextDB contextDB)
        {
            bool result = false;
            var memberships = await contextDB.Membership.FirstOrDefaultAsync(m => m.InternalIdentityCode == membership.InternalIdentityCode && m.Id != membership.Id);
            if (memberships != null && memberships.Id > 0 && memberships.InternalIdentityCode == membership.InternalIdentityCode)
                result = true;
            return result;
        }
        #endregion

        #region METODO PARA CREAR
        // Metodo para crear agregar un nuevo registro a la base de datos
        public static async Task<int> CreateAsync(Membership membership)
        {
            if (membership == null)
            {
                throw new ArgumentNullException(nameof(membership), "La Membresia no puede ser nulo.");
            }

            int result = 0;
            using (var contextDB = new ContextDB())
            {
                // Verificamos si la membresia ya existe
                bool membershipExists = await ExistMembership(membership, contextDB);
                if (membershipExists)
                {
                    throw new Exception("Membresia Ya Existente, Vuelve a Intentarlo Nuevamente.");
                }

                // Guardar la membresi en la base de datos
                contextDB.Membership.Add(membership);
                result = await contextDB.SaveChangesAsync();
            }
            return result;
        }
        #endregion

        #region METODO PARA MOSTRAR
        // Metodo Para Mostrar La Lista De Registros
        public static async Task<List<Membership>> GetAllAsync()
        {
            var membership = new List<Membership>();
            using (var dbContext = new ContextDB())
            {
                membership = await dbContext.Membership.ToListAsync();
            }
            return membership;
        }
        #endregion

        #region METODO PARA OBTENER POR ID
        // Metodo Para Mostrar Un Registro En Base A Su Id
        public static async Task<Membership> GetByIdAsync(Membership membership)
        {
            var membershipDB = new Membership();
            // Un bloque de conexion que mientras se permanezca en el bloque la base de datos permanecera abierta y al terminar se destruira
            using (var dbContext = new ContextDB())
            {
                membershipDB = await dbContext.Membership.FirstOrDefaultAsync(m => m.Id == membership.Id);
            }
            return membershipDB!; // Retornamos el Registro Encontrado
        }
        #endregion

        #region METODO PARA BUSCAR REGISTROS MEDIANTE EL USO DE FILTROS
        // Metodo Para Buscar Por Filtros
        // IQueryable es una interfaz que toma un coleccion a la cual se le pueden implementar multiples consultas (Filtros)
        internal static IQueryable<Membership> QuerySelect(IQueryable<Membership> query, Membership membership)
        {
            // Por ID
            if (membership.Id > 0)
                query = query.Where(m => m.Id == membership.Id);

            if (!string.IsNullOrWhiteSpace(membership.Name))
                query = query.Where(m => m.Name.Contains(membership.Name));

            if (!string.IsNullOrWhiteSpace(membership.LastName))
                query = query.Where(m => m.LastName.Contains(membership.LastName));

            if (!string.IsNullOrWhiteSpace(membership.Dui))
                query = query.Where(m => m.Dui!.Contains(membership.Dui));

            if (!string.IsNullOrWhiteSpace(membership.InternalIdentityCode))
                query = query.Where(m => m.InternalIdentityCode.Contains(membership.InternalIdentityCode));

            if (!string.IsNullOrWhiteSpace(membership.Gender))
                query = query.Where(m => m.Gender.Contains(membership.Gender));

            // Ordenamos de Manera Desendente
            query = query.OrderByDescending(c => c.Id).AsQueryable();

            return query;
        }
        #endregion

        #region METODO PARA BUSCAR
        // Metodo para Buscar Registros Existentes
        public static async Task<List<Membership>> SearchAsync(Membership membership)
        {
            var memberships = new List<Membership>();
            using (var dbContext = new ContextDB())
            {
                var select = dbContext.Membership.AsQueryable();
                select = QuerySelect(select, membership);
                memberships = await select.ToListAsync();
            }
            return memberships;
        }
        #endregion

        #region METODO PARA MODIFICAR
        // Metodo para modificar un registro
        public static async Task<int> UpdateAsync(Membership membership)
        {
            int result = 0;

            using (var contextDb = new ContextDB())
            {
                var membershipDB = await contextDb.Membership.FirstOrDefaultAsync(c => c.Id == membership.Id);
                if (membershipDB == null)
                {
                    throw new Exception("Membresia no encontrada para actualizar.");
                }

                // Verificar si ya existe otra membresia con el mismo código
                bool membershipExists = await contextDb.Membership.AnyAsync(c => c.Id != membership.Id && c.InternalIdentityCode == membership.InternalIdentityCode);
                if (membershipExists)
                {
                    throw new Exception("Ya existe otro curso con el mismo código. Vuelve a intentarlo.");
                }

                // Actualizar los datos de la membresia
                membershipDB.Name = membership.Name;
                membershipDB.LastName = membership.LastName;
                membershipDB.Dui = membership.Dui;
                membershipDB.DateOfBirth = membership.DateOfBirth;
                membershipDB.Age = membership.Age;
                membershipDB.Gender = membership.Gender;
                membershipDB.CivilStatus = membership.CivilStatus;
                membershipDB.Phone = membership.Phone;
                membershipDB.Address = membership.Address;
                membershipDB.ProfessionOrStudy = membership.ProfessionOrStudy;
                membershipDB.PlaceOfWorkOrStudy = membership.PlaceOfWorkOrStudy;
                membershipDB.WorkOrStudyPhone = membership.WorkOrStudyPhone;
                membershipDB.ConversionDate = membership.ConversionDate;
                membershipDB.PlaceOfConversion = membership.PlaceOfConversion;
                membershipDB.WaterBaptism = membership.WaterBaptism;
                membershipDB.BaptismOfTheHolySpirit = membership.BaptismOfTheHolySpirit;
                membershipDB.PastorsName = membership.PastorsName;
                membershipDB.SupervisorsName = membership.SupervisorsName;
                membershipDB.LeadersName = membership.LeadersName;
                membershipDB.TimeToGather = membership.TimeToGather;
                membershipDB.Zone = membership.Zone;
                membershipDB.District = membership.District;
                membershipDB.Sector = membership.Sector;
                membershipDB.Cell = membership.Cell;
                membershipDB.InternalIdentityCode = membership.InternalIdentityCode;
                membershipDB.Status = membership.Status;
                membershipDB.ImageData = membership.ImageData;
                membershipDB.CommentsOrObservations = membership.CommentsOrObservations;
                membershipDB.DateCreated = membership.DateCreated;
                membershipDB.DateModification = membership.DateModification;
                membershipDB.NameOfSpouse = membership.NameOfSpouse;
                membershipDB.LastNameOfSpouse = membership.LastNameOfSpouse;
                membershipDB.DateOfBirthOfSpouse = membership.DateOfBirthOfSpouse;
                membershipDB.AgeOfSpouse = membership.AgeOfSpouse;
                membershipDB.GenderOfSpouse = membership.GenderOfSpouse;
                membershipDB.PhoneOfSpouse = membership.PhoneOfSpouse;

                contextDb.Update(membershipDB);
                result = await contextDb.SaveChangesAsync();
            }
            return result;
        }
        #endregion

        #region METODO PARA ELIMINAR
        // Metodo Para Eliminar Un Registro Existente En La Base De Datos
        public static async Task<int> DeleteAsync(Membership membership)
        {
            int result = 0;
            // Un bloque de conexion que mientras se permanezca en el bloque la base de datos permanecera abierta y al terminar se destruira
            using (var dbContext = new ContextDB())
            {
                var membershipDB = await dbContext.Membership.FirstOrDefaultAsync(m => m.Id == membership.Id);
                if (membershipDB != null)
                {
                    dbContext.Membership.Remove(membershipDB);
                    result = await dbContext.SaveChangesAsync();
                }
            }
            return result;  // Si se realizo con exito devuelve 1 sino devuelve 0
        }
        #endregion
    }
}
