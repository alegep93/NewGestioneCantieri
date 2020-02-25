using Dapper;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace GestioneCantieri.DAO
{
    public class FattureAccontiDAO : BaseDAO
    {
        public static List<FatturaAcconto> GetAll()
        {
            List<FatturaAcconto> ret = new List<FatturaAcconto>();

            try
            {
                string sql = "SELECT * FROM TblFattureAcconti ";
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<FatturaAcconto>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetAll in FattureAccontiDAO", ex);
            }
            return ret;
        }

        public static FatturaAcconto GetSingle(long idFattura)
        {
            FatturaAcconto ret = new FatturaAcconto();

            try
            {
                string sql = "SELECT * " +
                             "FROM TblFatture AS A " +
                             "INNER JOIN TblFattureAcconti AS B " +
                             "ON A.id_fatture = B.id_fatture " +
                             "WHERE A.id_fatture = @idFattura ";

                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<FatturaAcconto>(sql, new { idFattura }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetSingle in FattureAccontiDAO", ex);
            }
            return ret;
        }

        public static List<FatturaAcconto> GetByIdFattura(long idFattura)
        {
            List<FatturaAcconto> ret = new List<FatturaAcconto>();

            try
            {
                string sql = "SELECT * FROM TblFattureAcconti WHERE id_fatture = @idFattura ";

                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<FatturaAcconto>(sql, new { idFattura }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetByIdFattura in FattureAccontiDAO", ex);
            }
            return ret;
        }

        public static double GetTotaleAccontiFattura(long idFattura)
        {
            double ret = 0;

            try
            {
                string sql = "SELECT ISNULL(SUM(valore_acconto), 0) FROM TblFattureAcconti WHERE id_fatture = @idFattura";
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<double>(sql, new { idFattura }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetTotaleAccontiFattura in FattureAccontiDAO", ex);
            }
            return ret;
        }

        public static decimal GetTotaleAccontiNonRiscossi()
        {
            try
            {
                string sql = "SELECT ISNULL(SUM(valore_acconto), 0) FROM TblFattureAcconti AS A INNER JOIN TblFatture AS B ON A.id_fatture = B.id_fatture WHERE B.riscosso = 0";
                using (SqlConnection cn = GetConnection())
                {
                    return cn.Query<decimal>(sql).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetTotaleAccontiNonRiscossi in FattureAccontiDAO", ex);
            }
        }

        public static void Insert(long idFattura, double valoreAcconto)
        {
            try
            {
                string sql = "INSERT INTO TblFattureAcconti (id_fatture, valore_acconto) VALUES (@idFattura,@valoreAcconto) ";

                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql, new { idFattura, valoreAcconto });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Insert in FattureAccontiDAO", ex);
            }
        }

        public static void Delete(int idFattura)
        {
            try
            {
                string sql = "DELETE FROM TblFattureAcconti WHERE id_fatture = @idFattura ";

                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql, new { idFattura });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Delete in FattureAccontiDAO", ex);
            }
        }
    }
}