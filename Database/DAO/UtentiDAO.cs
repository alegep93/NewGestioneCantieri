using Dapper;
using Database.Models;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Database.DAO
{
    public class UtentiDAO : BaseDAO
    {
        public static Utente Login(string username, string password)
        {
            Utente ret = new Utente();
            StringBuilder sql = new StringBuilder();
            try
            {
                sql.AppendLine($"SELECT * FROM TblUtenti WHERE username = @username and password = @password");
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Utente>(sql.ToString(), new { username, password }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Login in UtentiDAO", ex);
            }
            return ret;
        }
    }
}
