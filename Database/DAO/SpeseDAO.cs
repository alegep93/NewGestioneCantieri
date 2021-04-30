using Dapper;
using Database.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Database.DAO
{
    public class SpeseDAO : BaseDAO
    {
        //SELECT
        public static List<Spese> GetAll()
        {
            List<Spese> ret = new List<Spese>();
            StringBuilder sql = new StringBuilder("SELECT * FROM TblSpese ORDER BY Descrizione");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Spese>(sql.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetAll in SpeseDAO", ex);
            }
            return ret;
        }

        public static Spese GetDettagliSpesa(string idSpesa)
        {
            Spese ret = new Spese();
            StringBuilder sql = new StringBuilder("SELECT * FROM TblSpese where IdSpesa = @idSpesa");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Spese>(sql.ToString(), new { idSpesa }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetSingle in SpeseDAO", ex);
            }
            return ret;
        }

        public static List<Spese> GetByDescription(string descrizione)
        {
            List<Spese> ret = new List<Spese>();
            StringBuilder sql = new StringBuilder($"SELECT * FROM TblSpese where Descrizione LIKE '%{descrizione}%'");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Spese>(sql.ToString(), new { descrizione }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetByDescription in SpeseDAO", ex);
            }
            return ret;
        }

        //INSERT
        public static bool InsertSpesa(Spese s)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder("INSERT INTO TblSpese (Descrizione, Prezzo) VALUES (@Descrizione, @Prezzo)");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), s) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'inserimento di una spesa", ex);
            }
            return ret;
        }

        //UPDATE
        public static bool UpdateSpesa(Spese s)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder("UPDATE TblSpese SET Descrizione = @Descrizione, Prezzo = @Prezzo WHERE idSpesa = @idSpesa");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), s) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'aggiornamento di una spesa", ex);
            }
            return ret;
        }

        //DELETE
        public static bool DeleteSpesa(int idSpesa)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder("DELETE FROM TblSpese WHERE idSpesa = @idSpesa");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), new { idSpesa }) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'eliminazione di una spesa", ex);
            }
            return ret;
        }
    }
}