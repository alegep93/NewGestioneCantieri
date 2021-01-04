using Dapper;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GestioneCantieri.DAO
{
    public class FruttiDAO : BaseDAO
    {
        public static List<Frutto> GetFrutti(string filtroDescr1 = "", string filtroDescr2 = "", string filtroDescr3 = "")
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT ID1, descr001");
            sql.AppendLine($"FROM TblFrutti");
            sql.AppendLine($"WHERE descr001 LIKE '%{filtroDescr1}%' AND descr001 LIKE '%{filtroDescr2}%' AND descr001 LIKE '%{filtroDescr3}%'");
            sql.AppendLine($"ORDER BY descr001 ASC");

            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    return cn.Query<Frutto>(sql.ToString(), new { filtroDescr1, filtroDescr2, filtroDescr3 }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetFrutti in FruttiDAO", ex);
            }
        }

        public static Frutto GetSingle(int idFrutto)
        {
            StringBuilder sql = new StringBuilder($"SELECT * FROM TblFrutti WHERE ID1 = @idFrutto");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    return cn.Query<Frutto>(sql.ToString(), new { idFrutto }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetSingle in FruttiDAO", ex);
            }
        }

        public static bool InserisciFrutto(string nomeFrutto)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("IF NOT EXISTS (SELECT descr001 FROM TblFrutti WHERE descr001 = @nomeFrutto)");
            sql.AppendLine("INSERT INTO TblFrutti(descr001) VALUES (@nomeFrutto)");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), new { nomeFrutto }) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'inserimento di un frutto", ex);
            }
            return ret;
        }

        public static bool UpdateFrutto(Frutto item)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder("UPDATE TblFrutti SET descr001 = @Descr001 WHERE ID1 = @Id1");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), item) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'aggiornamento di un frutto", ex);
            }
            return ret;
        }

        public static bool DeleteFrutto(int idFrutto)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("IF NOT EXISTS(SELECT Id FROM TblCompGruppoFrut WHERE IdTblFrutto = @idFrutto)");
            sql.AppendLine("DELETE FROM TblFrutti WHERE ID1 = @idFrutto ");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), new { idFrutto }) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la DeleteFrutto in FruttiDAO", ex);
            }
            return ret;
        }
    }
}