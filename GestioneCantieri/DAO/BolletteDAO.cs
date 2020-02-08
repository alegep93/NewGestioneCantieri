using Dapper;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace GestioneCantieri.DAO
{
    public class BolletteDAO : BaseDAO
    {
        public static List<Bolletta> GetAll()
        {
            List<Bolletta> ret = new List<Bolletta>();

            try
            {
                string sql = "SELECT A.*, B.RagSocForni FROM TblBollette AS A INNER JOIN TblForitori AS B ON A.id_fornitori = B.IdFornitori ORDER BY data_scadenza DESC";
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Bolletta>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetAll in BolletteDAO", ex);
            }
            return ret;
        }

        public static Bolletta GetSingle(long idBolletta)
        {
            Bolletta ret = new Bolletta();

            try
            {
                string sql = "SELECT * FROM TblBollette WHERE id_bollette = @idBolletta ";

                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Bolletta>(sql, new { idBolletta }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetSingle in BolletteDAO", ex);
            }
            return ret;
        }

        public static decimal GetTotale()
        {
            try
            {
                string sql = "SELECT SUM(totale_bolletta) FROM TblBollette";

                using (SqlConnection cn = GetConnection())
                {
                    return cn.Query<decimal>(sql).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetTotale in BolletteDAO", ex);
            }
        }

        public static void Insert(Bolletta item)
        {
            try
            {
                string sql = "INSERT INTO TblBollette (id_fornitori, data_scadenza, data_pagamento, totale_bolletta) VALUES (@IdFornitori, @DataScadenza, @DataPagamento, @TotaleBolletta)";

                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql, item);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Insert in BolletteDAO", ex);
            }
        }

        public static void Update(Bolletta item)
        {
            try
            {
                string sql = "UPDATE TblBollette SET id_fornitori = @IdFornitori, data_scadenza = @DataScadenza, data_pagamento = @DataPagamento, totale_bolletta = @TotaleBolletta WHERE id_bollette = @IdBollette";

                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql, item);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Update in BolletteDAO", ex);
            }
        }

        public static void Delete(long idBolletta)
        {
            try
            {
                string sql = "DELETE FROM TblBollette WHERE id_bollette = @idBolletta ";

                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql, new { idBolletta });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Delete in BolletteDAO", ex);
            }
        }
    }
}