using Dapper;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace GestioneCantieri.DAO
{
    public class PreventiviDAO : BaseDAO
    {
        public static List<Preventivo> GetAll()
        {
            List<Preventivo> ret = new List<Preventivo>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT A.*, B.NomeOp");
            sql.AppendLine($"FROM TblPreventivi AS A");
            sql.AppendLine($"INNER JOIN TblOperaio AS B ON A.IdOperaio = B.IdOperaio");
            sql.AppendLine($"ORDER BY A.Numero");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Preventivo>(sql.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetAll in PreventiviDAO", ex);
            }
            return ret;

        }
        public static List<Preventivo> GetPreventivi(string anno, string numero, string descrizione)
        {
            List<Preventivo> ret = new List<Preventivo>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT A.*, B.NomeOp");
            sql.AppendLine($"FROM TblPreventivi AS A");
            sql.AppendLine($"INNER JOIN TblOperaio AS B ON A.IdOperaio = B.IdOperaio");
            sql.AppendLine($"WHERE A.Anno LIKE '%{anno}%' AND A.Descrizione LIKE '%{descrizione}%' AND A.Numero LIKE '%{numero}%'");
            sql.AppendLine($"ORDER BY A.Numero");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Preventivo>(sql.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero della lista dei preventivi", ex);
            }
            return ret;
        }

        internal static Preventivo GetSingle(int idPreventivo)
        {
            Preventivo ret = new Preventivo();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT *, B.NomeOp");
            sql.AppendLine($"FROM TblPreventivi AS A");
            sql.AppendLine($"INNER JOIN TblOperaio AS B ON A.IdOperaio = B.IdOperaio");
            sql.AppendLine($"WHERE Id = @idPreventivo");
            sql.AppendLine($"ORDER BY A.Numero");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Preventivo>(sql.ToString(), new { idPreventivo }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero del singolo preventivo", ex);
            }
            return ret;
        }

        internal static bool Insert(Preventivo p)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder("INSERT INTO TblPreventivi(Anno,Numero,IdOperaio,Descrizione,Data) VALUES(@Anno,@Numero,@IdOperaio,@Descrizione,@Data)");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), p) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'inserimento del preventivo", ex);
            }
            return ret;
        }

        internal static bool Update(Preventivo p)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            try
            {
                sql.AppendLine($"UPDATE TblPreventivi");
                sql.AppendLine($"SET IdOperaio = @IdOperaio, Descrizione = @Descrizione, Data = @Data");
                sql.AppendLine($"WHERE Id = @Id");

                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), p) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'aggiornamento del preventivo", ex);
            }
            return ret;
        }

        internal static bool Delete(int idPreventivo)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder("DELETE FROM TblPreventivi WHERE Id = @idPreventivo");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), new { idPreventivo }) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'eliminazione del preventivo", ex);
            }
            return ret;
        }
    }
}