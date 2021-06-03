using Dapper;
using Database.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Database.DAO
{
    public class FattureAcquistoDAO : BaseDAO
    {
        public static List<FatturaAcquisto> GetAll()
        {
            List<FatturaAcquisto> ret = new List<FatturaAcquisto>();
            StringBuilder sql = new StringBuilder("SELECT * FROM TblFattureAcquisto ORDER BY numero");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<FatturaAcquisto>(sql.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetAll in FattureAcquistoDAO", ex);
            }
            return ret;
        }

        public static List<FatturaAcquisto> GetFattureAcquisto(string anno, string dataDa, string dataA, string fornitore, int numeroFattura)
        {
            List<FatturaAcquisto> ret = new List<FatturaAcquisto>();
            StringBuilder sql = new StringBuilder();
            string whereData = (dataDa == "" && dataA == "") ? "" : ((dataDa != "" && dataA == "") ? " AND A.data >= @dataDa " : ((dataDa == "" && dataA != "") ? " AND A.data <= @dataA " : " AND A.Data BETWEEN @dataDa AND @dataA "));

            sql.AppendLine($"SELECT DISTINCT A.id_fatture_acquisto, B.RagSocForni AS RagioneSocialeFornitore, A.numero, A.data, YEAR(A.data),");
            sql.AppendLine($"                A.imponibile, A.iva, A.ritenuta_acconto, A.reverse_charge, A.is_nota_di_credito");
            sql.AppendLine($"FROM TblFattureAcquisto AS A");
            sql.AppendLine($"INNER JOIN TblForitori AS B ON A.id_fornitore = B.IdFornitori");
            sql.AppendLine($"WHERE B.RagSocForni LIKE '%{fornitore}%' {whereData}");
            sql.AppendLine(anno != "" ? "AND DATEPART(YEAR, A.data) = @anno" : "");
            sql.AppendLine(numeroFattura > 0 ? "AND A.numero = @numeroFattura" : "");
            sql.AppendLine($"ORDER BY YEAR(A.data), A.numero");

            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<FatturaAcquisto>(sql.ToString(), new { anno, dataDa, dataA, numeroFattura }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetFattureAcquisto in FattureAcquistoDAO", ex);
            }
            return ret;
        }

        public static List<FatturaAcquisto> GetByAnnoNumero(int anno, int numeroFattura)
        {
            List<FatturaAcquisto> ret = new List<FatturaAcquisto>();
            StringBuilder sql = new StringBuilder();
            try
            {
                anno = anno == 0 ? DateTime.Now.Year : anno;
                sql.AppendLine($"SELECT A.*, B.RagSocForni AS RagioneSocialeFornitore");
                sql.AppendLine($"FROM TblFattureAcquisto AS A");
                sql.AppendLine($"INNER JOIN TblForitori AS B ON A.id_fornitore = B.IdFornitori");
                sql.AppendLine($"WHERE DATEPART(YEAR, A.data) = @anno");
                sql.AppendLine(numeroFattura > 0 ? "AND A.numero = @numeroFattura " : "");
                sql.AppendLine($"ORDER BY numero");
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<FatturaAcquisto>(sql.ToString(), new { anno, numeroFattura }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetByAnnoNumero in FattureDAO", ex);
            }
            return ret;
        }

        public static FatturaAcquisto GetSingle(long idFatturaAcquisto)
        {
            FatturaAcquisto ret = new FatturaAcquisto();
            StringBuilder sql = new StringBuilder("SELECT * FROM TblFattureAcquisto WHERE id_fatture_acquisto = @idFatturaAcquisto ORDER BY numero");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<FatturaAcquisto>(sql.ToString(), new { idFatturaAcquisto }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore durante il recupero del singolo Fattura, id = {idFatturaAcquisto}", ex);
            }
            return ret;
        }

        public static List<(string quarter, double totaleIva)> GetTotaliIvaPerQuarter(int anno)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT (CASE WHEN DATEPART(QUARTER, data) = 1 THEN 'Gen-Feb-Mar' WHEN DATEPART(QUARTER, data) = 2 THEN 'Apr-Mag-Giu' WHEN DATEPART(QUARTER, data) = 3 THEN 'Lug-Ago-Set' WHEN DATEPART(QUARTER, data) = 4 THEN 'Ott-Nov-Dic' END) Trimestre,");
            sql.AppendLine($"       SUM(imponibile * iva / 100) TotaleIva");
            sql.AppendLine($"FROM TblFattureAcquisto WHERE DATEPART(YEAR, data) = @anno");
            sql.AppendLine($"GROUP BY DATEPART(QUARTER, data)");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    return cn.Query<(string, double)>(sql.ToString(), new { anno }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetTotaliIvaPerQuarter", ex);
            }
        }

        public static List<(string quarter, double totaleIva)> GetTotaliImponibilePerQuarter(int anno)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT (CASE WHEN DATEPART(QUARTER, data) = 1 THEN 'Gen-Feb-Mar' WHEN DATEPART(QUARTER, data) = 2 THEN 'Apr-Mag-Giu' WHEN DATEPART(QUARTER, data) = 3 THEN 'Lug-Ago-Set' WHEN DATEPART(QUARTER, data) = 4 THEN 'Ott-Nov-Dic' END) Trimestre,");
            sql.AppendLine($"       SUM(imponibile) TotaleIva");
            sql.AppendLine($"FROM TblFattureAcquisto WHERE DATEPART(YEAR, data) = @anno");
            sql.AppendLine($"GROUP BY DATEPART(QUARTER, data)");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    return cn.Query<(string, double)>(sql.ToString(), new { anno }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetTotaliIvaPerQuarter", ex);
            }
        }

        public static List<(string quarter, double totaleIva)> GetTotaliImportoPerQuarter(int anno)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT (CASE WHEN DATEPART(QUARTER, data) = 1 THEN 'Gen-Feb-Mar' WHEN DATEPART(QUARTER, data) = 2 THEN 'Apr-Mag-Giu' WHEN DATEPART(QUARTER, data) = 3 THEN 'Lug-Ago-Set' WHEN DATEPART(QUARTER, data) = 4 THEN 'Ott-Nov-Dic' END) Trimestre,");
            sql.AppendLine($"       SUM(imponibile + (imponibile * iva / 100) - (imponibile * ritenuta_acconto / 100)) TotaleIva");
            sql.AppendLine($"FROM TblFattureAcquisto WHERE DATEPART(YEAR, data) = @anno");
            sql.AppendLine($"GROUP BY DATEPART(QUARTER, data)");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    return cn.Query<(string, double)>(sql.ToString(), new { anno }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetTotaliIvaPerQuarter", ex);
            }
        }

        public static List<(string titolo, double valore)> GetTotaliFatture(string fornitore, string annoString, int numeroFattura, string dataDa, string dataA)
        {
            StringBuilder sql = new StringBuilder();
            string whereData = (dataDa == "" && dataA == "") ? "" : ((dataDa != "" && dataA == "") ? " AND A.data >= @dataDa " : ((dataDa == "" && dataA != "") ? " AND A.data <= @dataA " : " AND A.Data BETWEEN @dataDa AND @dataA "));
            string whereNumeroFattura = numeroFattura > 0 ? " AND A.numero = @numeroFattura " : "";
            string where = $"WHERE B.RagSocForni LIKE '%{fornitore}%'" + (annoString == "" ? "" : $"AND DATEPART(YEAR, A.data) = {Convert.ToInt32(annoString)} {whereNumeroFattura} {whereData} ");

            sql.AppendLine($"SELECT 'Totale Iva' as Titolo, ISNULL(SUM(imponibile * iva / 100), 0) AS Valore, 1 as Ordine");
            sql.AppendLine($"FROM TblFattureAcquisto AS A");
            sql.AppendLine($"INNER JOIN TblForitori AS B ON A.id_fornitore = B.IdFornitori {where}");
            sql.AppendLine($"UNION");
            sql.AppendLine($"SELECT 'Totale Imponibile' as Titolo, ISNULL(SUM(imponibile), 0) AS Valore, 2 as Ordine");
            sql.AppendLine($"FROM TblFattureAcquisto AS A");
            sql.AppendLine($"INNER JOIN TblForitori AS B ON A.id_fornitore = B.IdFornitori {where}");
            sql.AppendLine($"UNION");
            sql.AppendLine($"SELECT 'Totale Importo' as Titolo, ISNULL(SUM(imponibile + (imponibile * iva / 100) - (imponibile * ritenuta_acconto / 100)), 0) AS Valore, 3 as Ordine");
            sql.AppendLine($"FROM TblFattureAcquisto AS A");
            sql.AppendLine($"INNER JOIN TblForitori AS B ON A.id_fornitore = B.IdFornitori {where}");
            sql.AppendLine($"ORDER BY Ordine");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    return cn.Query<(string, double)>(sql.ToString(), new { dataDa, dataA, numeroFattura }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetTotaliFatture", ex);
            }
        }

        public static List<(string quarter, double totaleAcquisto, double totaleEmesso, double saldo)> GetTotaliFatture(int anno)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT Acquisto.Trimestre, Acquisto.TotaleIvaAcquisto, Emesso.TotaleIvaEmesso, (Acquisto.TotaleIvaAcquisto - Emesso.TotaleIvaEmesso) Saldo");
            sql.AppendLine($"FROM (");
            sql.AppendLine($"  SELECT (CASE WHEN DATEPART(QUARTER, data) = 1 THEN 'Gen-Feb-Mar'");
            sql.AppendLine($"               WHEN DATEPART(QUARTER, data) = 2 THEN 'Apr-Mag-Giu'");
            sql.AppendLine($"               WHEN DATEPART(QUARTER, data) = 3 THEN 'Lug-Ago-Set'");
            sql.AppendLine($"               WHEN DATEPART(QUARTER, data) = 4 THEN 'Ott-Nov-Dic' END) Trimestre,");
            sql.AppendLine($"         SUM(imponibile * iva / 100) TotaleIvaAcquisto");
            sql.AppendLine($"  FROM TblFattureAcquisto");
            sql.AppendLine($"  WHERE DATEPART(YEAR, data) = @anno");
            sql.AppendLine($"  GROUP BY DATEPART(YEAR, data), DATEPART(QUARTER, data)");
            sql.AppendLine($") AS Acquisto");
            sql.AppendLine($"INNER JOIN (");
            sql.AppendLine($"  SELECT (CASE WHEN DATEPART(QUARTER, data) = 1 THEN 'Gen-Feb-Mar'");
            sql.AppendLine($"               WHEN DATEPART(QUARTER, data) = 2 THEN 'Apr-Mag-Giu'");
            sql.AppendLine($"               WHEN DATEPART(QUARTER, data) = 3 THEN 'Lug-Ago-Set'");
            sql.AppendLine($"               WHEN DATEPART(QUARTER, data) = 4 THEN 'Ott-Nov-Dic' END) Trimestre,");
            sql.AppendLine($"  SUM(imponibile * iva / 100) TotaleIvaEmesso");
            sql.AppendLine($"  FROM TblFatture");
            sql.AppendLine($"  WHERE DATEPART(YEAR, data) = @anno");
            sql.AppendLine($"  GROUP BY DATEPART(YEAR, data), DATEPART(QUARTER, data)");
            sql.AppendLine($") AS Emesso ON Acquisto.Trimestre = Emesso.Trimestre");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    return cn.Query<(string, double, double, double)>(sql.ToString(), new { anno }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetTotaliFatture", ex);
            }
        }

        public static void Delete(long idFatturaAcquisto)
        {
            StringBuilder sql = new StringBuilder($"DELETE FROM TblFattureAcquisto WHERE id_fatture_acquisto = @idFatturaAcquisto");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql.ToString(), new { idFatturaAcquisto });
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore durante l'eliminazione della FatturaAcquisto con id = {idFatturaAcquisto}", ex);
            }
        }

        public static long Insert(FatturaAcquisto fa)
        {
            long ret = 0;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"INSERT INTO TblFattureAcquisto (id_fornitore,numero,data,imponibile,iva,ritenuta_acconto,reverse_charge,is_nota_di_credito,file_path)");
            sql.AppendLine($"VALUES (@IdFornitore,@Numero,@Data,@Imponibile,@Iva,@RitenutaAcconto,@ReverseCharge,@IsNotaDiCredito,@FilePath)");
            sql.AppendLine($"SELECT CAST(scope_identity() AS bigint)");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<long>(sql.ToString(), fa).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore durante l'inserimento della FatturaAcquisto {fa.Numero}", ex);
            }
            return ret;
        }

        public static bool Update(FatturaAcquisto fa)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"UPDATE TblFattureAcquisto");
            sql.AppendLine($"SET id_fornitore = @IdFornitore, numero = @Numero, data = @Data, imponibile = @Imponibile, iva = @Iva,");
            sql.AppendLine($"ritenuta_acconto = @RitenutaAcconto, reverse_charge = @ReverseCharge, is_nota_di_credito = @IsNotaDiCredito, file_path = @FilePath");
            sql.AppendLine($"WHERE id_fatture_acquisto = @IdFattureAcquisto");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    return cn.Execute(sql.ToString(), fa) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore durante l'aggiornamento della FatturaAcquisto {fa.Numero}", ex);
            }
        }
    }
}