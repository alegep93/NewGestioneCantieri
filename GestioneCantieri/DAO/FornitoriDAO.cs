using Dapper;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GestioneCantieri.DAO
{
    public class FornitoriDAO : BaseDAO
    {
        public static List<Fornitori> GetFornitori(string ragioneSociale = "")
        {
            List<Fornitori> ret = new List<Fornitori>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT *");
            sql.AppendLine($"FROM TblForitori");
            sql.AppendLine($"WHERE RagSocForni LIKE '%{ragioneSociale}%'");
            sql.AppendLine($"ORDER BY RagSocForni");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Fornitori>(sql.ToString(),new { ragioneSociale}).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetFornitori in FornitoriDAO", ex);
            }
            return ret;
        }

        public static Fornitori GetSingle(int idFornitore)
        {
            Fornitori ret = new Fornitori();
            StringBuilder sql = new StringBuilder("SELECT * FROM TblForitori WHERE IdFornitori = @idFornitore");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Fornitori>(sql.ToString(), new { idFornitore }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero della Ragione Sociale del Fornitore", ex);
            }
            return ret;
        }

        public static bool InserisciFornitore(Fornitori f)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"INSERT INTO TblForitori(RagSocForni, Indirizzo, cap, Città, Tel1, Cell1, PartitaIva, CodFiscale, Abbreviato)");
            sql.AppendLine($"VALUES (@RagSocForni, @Indirizzo, @cap, @Città, @Tel1, @Cell1, @PartitaIva, @CodFiscale, @Abbreviato)");
            try
            {

                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), f) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'inserimento di un nuovo Fornitore", ex);
            }
            return ret;
        }

        public static bool UpdateFornitore(Fornitori f)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"UPDATE TblForitori");
            sql.AppendLine($"SET RagSocForni = @RagSocForni,");
            sql.AppendLine($"Indirizzo = @Indirizzo,");
            sql.AppendLine($"cap = @cap,");
            sql.AppendLine($"Città = @Città,");
            sql.AppendLine($"Tel1 = @Tel1,");
            sql.AppendLine($"Cell1 = @Cell1,");
            sql.AppendLine($"PartitaIva = @PartitaIva,");
            sql.AppendLine($"CodFiscale = @CodFiscale,");
            sql.AppendLine($"Abbreviato = @Abbreviato");
            sql.AppendLine($"WHERE IdFornitori = @IdFornitori");
            try
            {

                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), f) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'update di un fornitore", ex);
            }
            return ret;
        }

        public static bool EliminaFornitore(int idFornitore)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder("DELETE FROM TblForitori WHERE IdFornitori = @idFornitore");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), new { idFornitore }) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'eliminazione di un fornitore", ex);
            }
            return ret;
        }
    }
}