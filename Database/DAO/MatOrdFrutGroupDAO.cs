using Dapper;
using Database.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Database.DAO
{
    public class MatOrdFrutGroupDAO : BaseDAO
    {
        public static List<MatOrdFrutGroup> GetAll()
        {
            List<MatOrdFrutGroup> ret = new List<MatOrdFrutGroup>();
            StringBuilder sql = new StringBuilder("SELECT IdMatOrdFrutGroup, Descrizione FROM TblMatOrdFrutGroup ORDER BY Descrizione");

            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<MatOrdFrutGroup>(sql.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetAll in MatOrdFrutGroupDAO", ex);
            }
            return ret;
        }

        public static int Insert(string descrizione)
        {
            int ret = 0;
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.AppendLine($"IF NOT EXISTS (SELECT * FROM TblMatOrdFrutGroup WHERE Descrizione = @descrizione)");
                sql.AppendLine($"INSERT INTO TblMatOrdFrutGroup (Descrizione) VALUES (@descrizione)");

                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), new { descrizione });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Insert in MatOrdFrutGroupDAO", ex);
            }
            return ret;
        }
    }
}