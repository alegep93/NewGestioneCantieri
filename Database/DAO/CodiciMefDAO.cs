using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Database.DAO
{
    public class CodiciMefDAO : BaseDAO
    {
        public static List<string> GetAll()
        {
            List<string> ret = new List<string>();
            StringBuilder sql = new StringBuilder();
            try
            {
                sql.AppendLine($"SELECT Codice FROM CodiciMef");
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<string>(sql.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetAll in CodiciMefDAO", ex);
            }
            return ret;
        }
    }
}
