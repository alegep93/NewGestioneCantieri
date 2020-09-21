using Dapper;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GestioneCantieri.DAO
{
    public class ClientiDAO : BaseDAO
    {
        //SELECT
        public static List<Clienti> GetClienti(string ragSocCli = "")
        {
            List<Clienti> ret = new List<Clienti>();
            StringBuilder sql = new StringBuilder($"SELECT IdCliente, RagSocCli FROM TblClienti WHERE RagSocCli LIKE '%@ragSocCli%'");

            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Clienti>(sql.ToString(), new { ragSocCli }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetClienti in ClientiDAO", ex);
            }
            return ret;
        }

        public static Clienti GetSingle(int idCliente)
        {
            Clienti ret = new Clienti();
            StringBuilder sql = new StringBuilder();
            try
            {
                sql.AppendLine($"SELECT IdCliente, IdAmministratore, RagSocCli, Indirizzo, cap, Città, Tel1,");
                sql.AppendLine($"Cell1, PartitaIva, CodFiscale, Data, Provincia, Note");
                sql.AppendLine($"FROM TblClienti");
                sql.AppendLine($"WHERE IdCliente = @idCliente");
                sql.AppendLine($"ORDER BY RagSocCli ASC");

                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Clienti>(sql.ToString(), new { idCliente }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetSingle in ClientiDAO", ex);
            }
            return ret;
        }

        // INSERT
        public static bool InserisciCliente(Clienti c)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"INSERT INTO TblClienti");
            sql.AppendLine($"(IdAmministratore,RagSocCli,Indirizzo,Cap,Città,Provincia,Tel1,Cell1,PartitaIva,CodFiscale,Data,Note)");
            sql.AppendLine($"VALUES (@IdAmministratore,@RagSocCli,@Indirizzo,@Cap,@Città,@Provincia,@Tel1,@Cell1,@PartitaIva,@CodFiscale,CONVERT(date,@Data),@Note)");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), c) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'inserimento di un nuovo cliente", ex);
            }
            return ret;
        }

        // UPDATE
        public static bool UpdateCliente(Clienti c)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            try
            {
                sql.AppendLine($"UPDATE TblClienti");
                sql.AppendLine($"SET IdAmministratore = @IdAmministratore,");
                sql.AppendLine($"RagSocCli = @RagSocCli,");
                sql.AppendLine($"Indirizzo = @Indirizzo,");
                sql.AppendLine($"cap = @cap,");
                sql.AppendLine($"Città = @Città,");
                sql.AppendLine($"Tel1 = @Tel1,");
                sql.AppendLine($"Cell1 = @Cell1,");
                sql.AppendLine($"PartitaIva = @PartitaIva,");
                sql.AppendLine($"CodFiscale = @CodFiscale,");
                sql.AppendLine($"Data = CONVERT(date,@Data),");
                sql.AppendLine($"Provincia = @Provincia,");
                sql.AppendLine($"Note = @Note");
                sql.AppendLine($"WHERE IdCliente = @IdCliente");

                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), c) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'update di un cliente", ex);
            }
            return ret;
        }

        // DELETE
        public static bool EliminaCliente(int idCliente)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"IF NOT EXISTS(SELECT IdTblClienti FROM TblCantieri where IdTblClienti = @idCliente)");
            sql.AppendLine($"DELETE FROM TblClienti WHERE IdCliente = @idCliente");
            try
            {

                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), new { idCliente }) > 0;
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