using Dapper;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GestioneCantieri.DAO
{
    public class AmministratoriDAO : BaseDAO
    {
        public static List<Amministratore> GetAll()
        {
            List<Amministratore> ret = new List<Amministratore>();
            StringBuilder sql = new StringBuilder("SELECT * FROM TblAmministratori ORDER BY nome");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Amministratore>(sql.ToString()).ToList();
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
            StringBuilder sql = new StringBuilder("SELECT * FROM TblAmministratori WHERE id_amministratori = @idAmministratore ");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Amministratore>(sql.ToString(), new { idAmministratore }).FirstOrDefault();
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
            StringBuilder sql = new StringBuilder("INSERT INTO TblAmministratori (nome) VALUES (@nome)");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql.ToString(), new { nome });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Insert in AmministratoriDAO", ex);
            }
        }

        public static void Update(Amministratore item)
        {
            StringBuilder sql = new StringBuilder("UPDATE TblAmministratori SET nome = @Nome WHERE id_amministratori = @IdAmministratori");
            try
            {

                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql.ToString(), item);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Update in AmministratoriDAO", ex);
            }
        }

        public static void Delete(long idAmministratore)
        {
            StringBuilder sql = new StringBuilder("DELETE FROM TblAmministratori WHERE id_amministratori = @idAmministratore");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql.ToString(), new { idAmministratore });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Delete in AmministratoriDAO", ex);
            }
        }
    }
}