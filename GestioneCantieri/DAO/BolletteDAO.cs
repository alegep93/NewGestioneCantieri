using Dapper;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GestioneCantieri.DAO
{
    public class BolletteDAO : BaseDAO
    {
        public static List<Bolletta> GetAll(int anno = 0, int idFornitore = 0)
        {
            List<Bolletta> ret = new List<Bolletta>();
            StringBuilder sql = new StringBuilder();
            try
            {
                sql.AppendLine($"SELECT A.*, B.RagSocForni");
                sql.AppendLine($"FROM TblBollette AS A");
                sql.AppendLine($"INNER JOIN TblForitori AS B ON A.id_fornitori = B.IdFornitori");
                sql.AppendLine(anno > 0 || idFornitore > 0 ? "WHERE" : "");
                sql.AppendLine(anno > 0 ? $" DATEPART(YEAR, data_bolletta) = {anno} " : "");
                sql.AppendLine(anno > 0 && idFornitore > 0 ? $" AND {(idFornitore > 0 ? $"A.id_fornitori = {idFornitore}" : "")}" : "");
                sql.AppendLine($"ORDER BY DATEPART(YEAR, data_bolletta), progressivo");
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Bolletta>(sql.ToString()).ToList();
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
            StringBuilder sql = new StringBuilder("SELECT * FROM TblBollette WHERE id_bollette = @idBolletta");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Bolletta>(sql.ToString(), new { idBolletta }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetSingle in BolletteDAO", ex);
            }
            return ret;
        }

        public static decimal GetTotale(int anno)
        {
            decimal ret = 0;
            StringBuilder sql = new StringBuilder("SELECT SUM(totale_bolletta) FROM TblBollette");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<decimal>(sql.ToString()).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetTotale in BolletteDAO", ex);
            }
            return ret;
        }

        public static void Insert(Bolletta item)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"INSERT INTO TblBollette (id_fornitori, data_bolletta, data_scadenza, data_pagamento, totale_bolletta, progressivo)");
            sql.AppendLine($"VALUES (@IdFornitori, @DataBolletta, @DataScadenza, @DataPagamento, @TotaleBolletta, @Progressivo)");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql.ToString(), item);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Insert in BolletteDAO", ex);
            }
        }

        public static void Update(Bolletta item)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"UPDATE TblBollette SET id_fornitori = @IdFornitori, data_bolletta = @DataBolletta, data_scadenza = @DataScadenza,");
            sql.AppendLine($"data_pagamento = @DataPagamento, totale_bolletta = @TotaleBolletta, progressivo = @Progressivo WHERE id_bollette = @IdBollette");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql.ToString(), item);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Update in BolletteDAO", ex);
            }
        }

        public static void Delete(long idBolletta)
        {
            StringBuilder sql = new StringBuilder("DELETE FROM TblBollette WHERE id_bollette = @idBolletta");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql.ToString(), new { idBolletta });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Delete in BolletteDAO", ex);
            }
        }
    }
}