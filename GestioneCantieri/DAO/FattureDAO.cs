using Dapper;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace GestioneCantieri.DAO
{
    public class FattureDAO : BaseDAO
    {
        public static List<Fattura> GetFatture(string anno, string cliente, string cantiere, string amministratore)
        {
            SqlConnection cn = GetConnection();
            cliente = "%" + cliente + "%";
            cantiere = "%" + cantiere + "%";
            amministratore = "%" + amministratore + "%";

            string sql = "SELECT A.id_fatture, F.RagSocCli AS RagioneSocialeCliente,  " +
                         "(SELECT STUFF((SELECT ';' + CONVERT(nvarchar, valore_acconto) AS 'data()' FROM TblFattureAcconti AS FatAcc WHERE FatAcc.id_fatture = A.id_fatture FOR XML PATH('')),1,1,'')) AS Acconti, " +
                         "(SELECT STUFF((SELECT ';' + Cant.CodCant AS 'data()' FROM TblCantieri AS Cant INNER JOIN TblFattureCantieri AS FattCant ON Cant.IdCantieri = FattCant.id_cantieri WHERE Cant.IdCantieri = FattCant.id_cantieri FOR XML PATH('')),1,1,'')) AS Cantieri, " +
                         "D.nome AS NomeAmministratore, A.numero, A.data, A.imponibile, A.iva, A.ritenuta_acconto, A.reverse_charge, A.riscosso, A.is_nota_di_credito " +
                         "FROM TblFatture AS A " +
                         "LEFT JOIN TblAmministratori AS D ON A.id_amministratori = D.id_amministratori " +
                         "INNER JOIN TblClienti AS F ON A.id_clienti = F.IdCliente " +
                         "WHERE F.RagSocCli LIKE @cliente AND D.nome LIKE @amministratore ";

            ///////////////////// TODO - CAPIRE COME FARE /////////////////////
            //AND E.CodCant LIKE @cantiere 
            ///////////////////// TODO - CAPIRE COME FARE /////////////////////

            sql += anno != "" ? "AND DATEPART(YEAR, A.data) = @anno " : " ";
            sql += "ORDER BY A.data, A.numero ";

            try
            {
                return cn.Query<Fattura>(sql, new { anno, cliente, cantiere, amministratore }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetFatture in FattureDAO", ex);
            }
            finally { CloseResouces(cn, null); }
        }

        internal static long GetLastNumber(int anno)
        {
            try
            {
                string sql = "SELECT ISNULL(MAX(numero)+1, 0) FROM TblFatture WHERE DATEPART(YEAR, data) = @anno ";
                using (SqlConnection cn = GetConnection())
                {
                    return cn.Query<long>(sql, new { anno }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero dell'ultimo numero Fattura", ex);
            }
        }

        internal static Fattura GetSingle(long idFattura)
        {
            Fattura prev = new Fattura();
            try
            {
                string sql = "SELECT * FROM TblFatture WHERE id_fatture = @idFattura ORDER BY numero ";
                using (SqlConnection cn = GetConnection())
                {
                    return cn.Query<Fattura>(sql, new { idFattura }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero del singolo Fattura, id = " + idFattura, ex);
            }
        }

        public static void Delete(long idFattura)
        {
            try
            {
                string sql = "DELETE FROM TblFatture WHERE id_fatture = @idFattura";
                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql, new { idFattura });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'eliminazione del Fattura con id " + idFattura, ex);
            }
        }

        internal static long Insert(Fattura p)
        {
            long ret = 0;
            try
            {
                string sql = "INSERT INTO TblFatture (id_clienti,id_amministratori,numero,data,riscosso,imponibile,iva,ritenuta_acconto,reverse_charge,is_nota_di_credito) " +
                             "VALUES (@IdClienti,@IdAmministratori,@Numero,@Data,@Riscosso,@Imponibile,@Iva,@RitenutaAcconto,@ReverseCharge,@IsNotaDiCredito) " +
                             "SELECT CAST(scope_identity() AS bigint) ";
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<long>(sql, p).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'inserimento del Fattura " + p.Numero, ex);
            }
            return ret;
        }

        internal static bool Update(Fattura p)
        {
            try
            {
                string sql = "UPDATE TblFatture " +
                             "SET id_clienti = @IdClienti, id_amministratori = @IdAmministratori, numero = @Numero, data = @Data, riscosso = @Riscosso, imponibile = @Imponibile, " +
                             "iva = @Iva, ritenuta_acconto = @RitenutaAcconto, reverse_charge = @ReverseCharge, is_nota_di_credito = @IsNotaDiCredito " +
                             "WHERE id_fatture = @IdFatture ";

                using (SqlConnection cn = GetConnection())
                {
                    return cn.Execute(sql, p) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'aggiornamento del Fattura " + p.Numero, ex);
            }
        }
    }
}