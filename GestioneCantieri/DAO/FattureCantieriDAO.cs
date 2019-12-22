using Dapper;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace GestioneCantieri.DAO
{
    public class FattureCantieriDAO : BaseDAO
    {
        public static List<FatturaCantiere> GetAll()
        {
            List<FatturaCantiere> ret = new List<FatturaCantiere>();

            try
            {
                string sql = "SELECT * FROM TblFattureCantieri ";
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<FatturaCantiere>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetAll in FattureCantieriDAO", ex);
            }
            return ret;
        }

        public static FatturaCantiere GetSingle(long idFattura)
        {
            FatturaCantiere ret = new FatturaCantiere();

            try
            {
                string sql = "SELECT * FROM TblFattureCantieri WHERE id_fatture = @idFattura ";

                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<FatturaCantiere>(sql, new { idFattura }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetSingle in FattureCantieriDAO", ex);
            }
            return ret;
        }

        public static List<FatturaCantiere> GetByIdFattura(long idFattura)
        {
            List<FatturaCantiere> ret = new List<FatturaCantiere>();

            try
            {
                string sql = "SELECT * FROM TblFattureCantieri WHERE id_fatture = @idFattura ";

                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<FatturaCantiere>(sql, new { idFattura }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetByIdFattura in FattureCantieriDAO", ex);
            }
            return ret;
        }

        public static void Insert(long idFattura, int idCantiere)
        {
            try
            {
                string sql = "INSERT INTO TblFattureCantieri (id_fatture, id_cantieri) VALUES (@idFattura,@idCantiere) ";

                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql, new { idFattura, idCantiere });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Insert in FattureCantieriDAO", ex);
            }
        }

        public static void Delete(int idFattura)
        {
            try
            {
                string sql = "DELETE FROM TblFattureCantieri WHERE id_fatture = @idFattura ";

                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql, new { idFattura });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Delete in FattureCantieriDAO", ex);
            }
        }
    }
}