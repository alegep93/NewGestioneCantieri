using Dapper;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GestioneCantieri.DAO
{
    public class StampeDAO : BaseDAO
    {
        public static List<Stampe> GetAll()
        {
            List<Stampe> ret = new List<Stampe>();
            StringBuilder sql = new StringBuilder($"SELECT * FROM TblStampe ORDER BY id");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Stampe>(sql.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetAll in StampeDAO", ex);
            }
            return ret;
        }
    }
}