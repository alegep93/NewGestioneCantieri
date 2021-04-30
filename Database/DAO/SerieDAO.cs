using Dapper;
using Database.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Database.DAO
{
    public class SerieDAO : BaseDAO
    {
        public static List<Serie> GetAll()
        {
            List<Serie> ret = new List<Serie>();
            StringBuilder sql = new StringBuilder($"SELECT * FROM TblSerie");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Serie>(sql.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetAll in SerieDAO", ex);
            }
            return ret;
        }

        public static void Insert(string nomeSerie)
        {
            StringBuilder sql = new StringBuilder($"INSERT INTO TblSerie (NomeSerie) VALUES (@nomeSerie)");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql.ToString(), new { nomeSerie });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Insert in SerieDAO", ex);
            }
        }

        public static void Update(Serie serie)
        {
            StringBuilder sql = new StringBuilder($"UPDATE TblSerie SET NomeSerie = @nomeSerie WHERE IdSerie = @IdSerie");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql.ToString(), serie);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Update in SerieDAO", ex);
            }
        }

        public static void Delete(long idSerie)
        {
            StringBuilder sql = new StringBuilder($"DELETE FROM TblSerie WHERE IdSerie = @IdSerie");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql.ToString(), new { idSerie });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Delete in SerieDAO", ex);
            }
        }
    }
}