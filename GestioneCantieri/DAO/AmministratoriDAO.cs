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
                string sql = "SELECT * FROM TblAmministratori ORDER BY nome";
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Amministratore>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetAll in AmministratoriDAO", ex);
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
                throw new Exception("Errore durante la GetSingle in AmministratoriDAO", ex);
            }
            return ret;
        }

        public static void Insert(string nome)
        {
            try
            {
                string sql = "INSERT INTO TblAmministratori (nome) VALUES (@nome)";

                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql, new { nome });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Insert in AmministratoriDAO", ex);
            }
        }

        public static void Update(Amministratore item)
        {
            try
            {
                string sql = "UPDATE TblAmministratori SET nome = @Nome WHERE id_amministratori = @IdAmministratori";

                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql, item);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Update in AmministratoriDAO", ex);
            }
        }

        public static void Delete(long idAmministratore)
        {
            try
            {
                string sql = "DELETE FROM TblAmministratori WHERE id_amministratori = @idAmministratore ";

                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql, new { idAmministratore });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Delete in AmministratoriDAO", ex);
            }
        }
    }
}