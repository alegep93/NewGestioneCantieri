using Dapper;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace GestioneCantieri.DAO
{
    public class MatOrdFrutGroupDAO : BaseDAO
    {
        public static List<MatOrdFrutGroup> GetAll()
        {
            SqlConnection cn = GetConnection();
            string sql = "";

            try
            {
                sql = "SELECT IdMatOrdFrutGroup, Descrizione FROM TblMatOrdFrutGroup ORDER BY Descrizione";
                return cn.Query<MatOrdFrutGroup>(sql).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero dei nomi dei gruppi ordine", ex);
            }
            finally
            {
                CloseResouces(cn, null);
            }
        }

        public static int Insert(string descrizione)
        {
            SqlConnection cn = GetConnection();
            string sql = "";
            int rows = 0;

            try
            {
                sql = "IF NOT EXISTS (SELECT * FROM TblMatOrdFrutGroup WHERE Descrizione = @descrizione)" +
                        "INSERT INTO TblMatOrdFrutGroup (Descrizione) VALUES (@descrizione)";
                rows = cn.Execute(sql, new { descrizione });
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'inserimento di un nuovo nome gruppo ordine", ex);
            }
            finally
            {
                CloseResouces(cn, null);
            }

            return rows;
        }
    }
}