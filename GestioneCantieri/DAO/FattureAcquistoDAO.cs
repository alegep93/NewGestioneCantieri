using Dapper;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace GestioneCantieri.DAO
{
    public class FattureAcquistoDAO : BaseDAO
    {
        public static List<FatturaAcquisto> GetFattureAcquisto(string anno, string dataDa, string dataA, string fornitore, int numeroFattura)
        {
            fornitore = "%" + fornitore + "%";

            string whereData = (dataDa == "" && dataA == "") ? "" : ((dataDa != "" && dataA == "") ? " AND A.data >= @dataDa " : ((dataDa == "" && dataA != "") ? " AND A.data <= @dataA " : " AND A.Data BETWEEN @dataDa AND @dataA "));

            string sql = "SELECT DISTINCT A.id_fatture_acquisto, B.RagSocForni AS RagioneSocialeFornitore, A.numero, A.data, A.imponibile, A.iva, A.ritenuta_acconto, A.reverse_charge, A.is_nota_di_credito " +
                         "FROM TblFattureAcquisto AS A " +
                         "INNER JOIN TblForitori AS B ON A.id_fornitore = B.IdFornitori " +
                         "WHERE B.RagSocForni LIKE '%%' " + whereData;
            sql += anno != "" ? "AND DATEPART(YEAR, A.data) = @anno " : " ";
            sql += numeroFattura > 0 ? "AND A.numero = @numeroFattura " : "";
            sql += "ORDER BY A.data, A.numero ";

            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    return cn.Query<FatturaAcquisto>(sql, new { anno, dataDa, dataA, fornitore, numeroFattura }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetFattureAcquisto in FattureAcquistoDAO", ex);
            }
        }

        internal static long GetLastNumber(int anno)
        {
            try
            {
                string sql = "SELECT ISNULL(MAX(numero)+1, 0) FROM TblFattureAcquisto WHERE DATEPART(YEAR, data) = @anno ";
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

        internal static Fattura GetSingle(long idFatturaAcquisto)
        {
            Fattura prev = new Fattura();
            try
            {
                string sql = "SELECT * FROM TblFattureAcquisto WHERE id_fatture_acquisto = @idFatturaAcquisto ORDER BY numero ";
                using (SqlConnection cn = GetConnection())
                {
                    return cn.Query<Fattura>(sql, new { idFatturaAcquisto }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero del singolo Fattura, id = " + idFatturaAcquisto, ex);
            }
        }

        public static List<(string quarter, double totaleIva)> GetTotaliIvaPerQuarter(int anno)
        {
            try
            {
                string sql = "SELECT (CASE WHEN DATEPART(QUARTER, data) = 1 THEN 'Gen-Feb-Mar' WHEN DATEPART(QUARTER, data) = 2 THEN 'Apr-Mag-Giu' WHEN DATEPART(QUARTER, data) = 3 THEN 'Lug-Ago-Set' WHEN DATEPART(QUARTER, data) = 4 THEN 'Ott-Nov-Dic' END) Trimestre, " +
                             "       SUM(imponibile * iva / 100) TotaleIva " +
                             $"FROM TblFattureAcquisto WHERE DATEPART(YEAR, data) = {anno} GROUP BY DATEPART(QUARTER, data)";
                using (SqlConnection cn = GetConnection())
                {
                    return cn.Query<(string, double)>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetTotaliIvaPerQuarter", ex);
            }
        }

        public static List<(string quarter, double totaleIva)> GetTotaliImponibilePerQuarter(int anno)
        {
            try
            {
                string sql = "SELECT (CASE WHEN DATEPART(QUARTER, data) = 1 THEN 'Gen-Feb-Mar' WHEN DATEPART(QUARTER, data) = 2 THEN 'Apr-Mag-Giu' WHEN DATEPART(QUARTER, data) = 3 THEN 'Lug-Ago-Set' WHEN DATEPART(QUARTER, data) = 4 THEN 'Ott-Nov-Dic' END) Trimestre, " +
                             "       SUM(imponibile) TotaleIva " +
                             $"FROM TblFattureAcquisto WHERE DATEPART(YEAR, data) = {anno} GROUP BY DATEPART(QUARTER, data)";
                using (SqlConnection cn = GetConnection())
                {
                    return cn.Query<(string, double)>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetTotaliIvaPerQuarter", ex);
            }
        }

        public static List<(string quarter, double totaleIva)> GetTotaliImportoPerQuarter(int anno)
        {
            try
            {
                string sql = "SELECT (CASE WHEN DATEPART(QUARTER, data) = 1 THEN 'Gen-Feb-Mar' WHEN DATEPART(QUARTER, data) = 2 THEN 'Apr-Mag-Giu' WHEN DATEPART(QUARTER, data) = 3 THEN 'Lug-Ago-Set' WHEN DATEPART(QUARTER, data) = 4 THEN 'Ott-Nov-Dic' END) Trimestre, " +
                             "       SUM(imponibile + (imponibile * iva / 100) - (imponibile * ritenuta_acconto / 100)) TotaleIva " +
                             $"FROM TblFattureAcquisto WHERE DATEPART(YEAR, data) = {anno} GROUP BY DATEPART(QUARTER, data)";
                using (SqlConnection cn = GetConnection())
                {
                    return cn.Query<(string, double)>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetTotaliIvaPerQuarter", ex);
            }
        }

        public static List<(string titolo, double valore)> GetTotaliFatture(string fornitore, string annoString, int numeroFattura, string dataDa, string dataA)
        {
            try
            {
                string whereData = (dataDa == "" && dataA == "") ? "" : ((dataDa != "" && dataA == "") ? " AND A.data >= @dataDa " : ((dataDa == "" && dataA != "") ? " AND A.data <= @dataA " : " AND A.Data BETWEEN @dataDa AND @dataA "));
                string whereNumeroFattura = numeroFattura > 0 ? " AND A.numero = @numeroFattura " : "";
                string where = $"WHERE B.RagSocForni LIKE '%{fornitore}%' " + (annoString == "" ? "" : $"AND DATEPART(YEAR, A.data) = {Convert.ToInt32(annoString)} {whereNumeroFattura} {whereData} ");

                string sql = "SELECT 'Totale Iva' as Titolo, ISNULL(SUM(imponibile * iva / 100), 0) AS Valore, 1 as Ordine " +
                             "FROM TblFattureAcquisto AS A " +
                             "INNER JOIN TblForitori AS B ON A.id_fornitore = B.IdFornitori " + where;
                sql += "UNION " +
                             "SELECT 'Totale Imponibile' as Titolo, ISNULL(SUM(imponibile), 0) AS Valore, 2 as Ordine " +
                             "FROM TblFattureAcquisto AS A " +
                             "INNER JOIN TblForitori AS B ON A.id_fornitore = B.IdFornitori" + where;
                sql += "UNION " +
                             "SELECT 'Totale Importo' as Titolo, ISNULL(SUM(imponibile + (imponibile * iva / 100) - (imponibile * ritenuta_acconto / 100)), 0) AS Valore, 3 as Ordine " +
                             "FROM TblFattureAcquisto AS A " +
                             "INNER JOIN TblForitori AS B ON A.id_fornitore = B.IdFornitori" + where;
                sql += "ORDER BY Ordine ";

                using (SqlConnection cn = GetConnection())
                {
                    return cn.Query<(string, double)>(sql, new { dataDa, dataA, numeroFattura }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetTotaliFatture", ex);
            }
        }

        public static void Delete(long idFatturaAcquisto)
        {
            try
            {
                string sql = "DELETE FROM TblFattureAcquisto WHERE id_fatture_acquisto = @idFatturaAcquisto";
                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql, new { idFatturaAcquisto });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'eliminazione della FatturaAcquisto con id = " + idFatturaAcquisto, ex);
            }
        }

        internal static long Insert(FatturaAcquisto fa)
        {
            long ret = 0;
            try
            {
                string sql = "INSERT INTO TblFattureAcquisto (id_fornitore,numero,data,imponibile,iva,ritenuta_acconto,reverse_charge,is_nota_di_credito) " +
                             "VALUES (@IdFornitore,@Numero,@Data,@Imponibile,@Iva,@RitenutaAcconto,@ReverseCharge,@IsNotaDiCredito) " +
                             "SELECT CAST(scope_identity() AS bigint) ";
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<long>(sql, fa).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'inserimento della FatturaAcquisto " + fa.Numero, ex);
            }
            return ret;
        }

        internal static bool Update(FatturaAcquisto fa)
        {
            try
            {
                string sql = "UPDATE TblFattureAcquisto " +
                             "SET id_fornitore = @IdFornitore, numero = @Numero, data = @Data, imponibile = @Imponibile, " +
                             "iva = @Iva, ritenuta_acconto = @RitenutaAcconto, reverse_charge = @ReverseCharge, is_nota_di_credito = @IsNotaDiCredito " +
                             "WHERE id_fatture_acquisto = @IdFattureAcquisto ";

                using (SqlConnection cn = GetConnection())
                {
                    return cn.Execute(sql, fa) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'aggiornamento della FatturaAcquisto " + fa.Numero, ex);
            }
        }
    }
}