using Dapper;
using Database.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Database.DAO
{
    public class NoteDAO : BaseDAO
    {
        public static List<Nota> GetAll()
        {
            List<Nota> ret = new List<Nota>();
            StringBuilder sql = new StringBuilder();
            try
            {
                sql.AppendLine($"SELECT * FROM TblNote");
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Nota>(sql.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetAll in NoteDAO", ex);
            }
            return ret;
        }

        public static long Insert(string nota)
        {
            long ret = 0;
            StringBuilder sql = new StringBuilder();
            try
            {
                sql.AppendLine($"INSERT INTO TblNote (IdCantiere,NoteText) VALUES (0,@nota)");
                sql.AppendLine($"SELECT CAST(scope_identity() AS bigint)");
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<long>(sql.ToString(), new { nota }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Insert in NoteDAO", ex);
            }
            return ret;
        }

        public static void Delete(long idNota)
        {
            StringBuilder sql = new StringBuilder();
            try
            {
                sql.AppendLine($"DELETE FROM TblNote WHERE IdNota = @idNota");
                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql.ToString(), new { idNota });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Delete in NoteDAO", ex);
            }
        }
    }
}
