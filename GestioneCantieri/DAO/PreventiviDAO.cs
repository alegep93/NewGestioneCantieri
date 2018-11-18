using Dapper;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GestioneCantieri.DAO
{
    public class PreventiviDAO : BaseDAO
    {
        public static List<Preventivo> GetPreventivi(string anno, string numero, string descrizione)
        {
            SqlConnection cn = GetConnection();
            List<Preventivo> list = new List<Preventivo>();
            string sql = "";

            anno = "%" + anno + "%";
            numero = "%" + numero + "%";
            descrizione = "%" + descrizione + "%";

            try
            {
                sql = "SELECT A.Id, A.Anno, A.Numero, B.NomeOp, A.Descrizione, A.Data " +
                      "FROM TblPreventivi AS A " +
                      "INNER JOIN TblOperaio AS B ON A.IdOperaio = B.IdOperaio " +
                      "WHERE A.Anno LIKE @anno " +
                      "AND A.Descrizione LIKE @descrizione " +
                      "AND A.Numero LIKE @numero " +
                      "ORDER BY A.Numero ";

                return cn.Query<Preventivo>(sql, new { anno, numero, descrizione }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero della lista dei preventivi", ex);
            }
            finally { CloseResouces(cn, null); }
        }

        internal static long GetLastNumber()
        {
            SqlConnection cn = GetConnection();
            string sql = "";

            try
            {
                sql = "SELECT MAX(Numero)+1 FROM TblPreventivi";

                return cn.Query<long>(sql).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero dell'ultimo numero preventivo", ex);
            }
            finally { CloseResouces(cn, null); }
        }

        internal static Preventivo GetSingle(int idPreventivo)
        {
            SqlConnection cn = GetConnection();
            Preventivo prev = new Preventivo();
            string sql = "";

            try
            {
                sql = "SELECT *, B.NomeOp " +
                      "FROM TblPreventivi AS A " +
                      "INNER JOIN TblOperaio AS B ON A.IdOperaio = B.IdOperaio " +
                      "WHERE Id = @idPreventivo " +
                      "ORDER BY A.Numero ";

                return cn.Query<Preventivo>(sql, new { idPreventivo }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero del singolo preventivo, id = " + idPreventivo, ex);
            }
            finally { CloseResouces(cn, null); }
        }

        internal static bool Delete(int idPreventivo)
        {
            SqlConnection cn = GetConnection();
            string sql = "";

            try
            {
                sql = "DELETE FROM TblPreventivi WHERE Id = @idPreventivo";

                int rows = cn.Execute(sql, new { idPreventivo });

                if (rows > 0)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'eliminazione del preventivo con id " + idPreventivo, ex);
            }
            finally { CloseResouces(cn, null); }
        }

        internal static bool Insert(Preventivo p)
        {
            SqlConnection cn = GetConnection();
            string sql = "";

            try
            {
                sql = "INSERT INTO TblPreventivi(Anno,Numero,IdOperaio,Descrizione,Data) VALUES(@Anno,@Numero,@IdOperaio,@Descrizione,@Data) ";

                int rows = cn.Execute(sql, p);

                if (rows > 0)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'inserimento del preventivo " + p.Numero, ex);
            }
            finally { CloseResouces(cn, null); }
        }

        internal static bool Update(Preventivo p)
        {
            SqlConnection cn = GetConnection();
            string sql = "";

            try
            {
                sql = "UPDATE TblPreventivi " +
                      "SET IdOperaio = @IdOperaio, Descrizione = @Descrizione, Data = @Data " +
                      "WHERE Id = @Id ";

                int rows = cn.Execute(sql, p);

                if (rows > 0)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'aggiornamento del preventivo " + p.Numero, ex);
            }
            finally { CloseResouces(cn, null); }
        }
    }
}