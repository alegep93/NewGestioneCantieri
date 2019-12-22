using Dapper;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace GestioneCantieri.DAO
{
    public class AmministratoriDAO : BaseDAO
    {
        public static List<Amministratore> GetAll()
        {
            List<Amministratore> ret = new List<Amministratore>();

            try
            {
                string sql = "SELECT * FROM TblAmministratori ";
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Amministratore>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetAll in FattureCantieriDAO", ex);
            }
            return ret;
        }

        public static Amministratore GetSingle(long idAmministratore)
        {
            Amministratore ret = new Amministratore();

            try
            {
                string sql = "SELECT * FROM TblAmministratori WHERE id_amministratori = @idAmministratore ";

                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Amministratore>(sql, new { idAmministratore }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetSingle in FattureCantieriDAO", ex);
            }
            return ret;
        }
    }
}