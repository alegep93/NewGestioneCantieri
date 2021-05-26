using Dapper;
using Database.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Database.DAO
{
    public class FattureDAO : BaseDAO
    {
        public static List<Fattura> GetFatture(string anno, string dataDa, string dataA, string cliente, string cantiere, string amministratore, int numeroFattura, int riscosso)
        {
            List<Fattura> ret = new List<Fattura>();
            StringBuilder sql = new StringBuilder();
            string whereData = (dataDa == "" && dataA == "") ? "" : ((dataDa != "" && dataA == "") ? " AND A.data >= @dataDa " : ((dataDa == "" && dataA != "") ? " AND A.data <= @dataA " : " AND A.Data BETWEEN @dataDa AND @dataA "));
            string whereAmministratore = $"({(amministratore == "" ? "D.nome IS NULL OR " : "")} D.nome LIKE '%{amministratore}%')";

            sql.AppendLine($"SELECT DISTINCT A.id_fatture, E.RagSocCli AS RagioneSocialeCliente, B.Cantieri, C.Acconti, D.nome AS NomeAmministratore, A.numero,");
            sql.AppendLine($"                A.data, YEAR(A.data), A.imponibile, A.iva, A.ritenuta_acconto, A.reverse_charge, A.riscosso, A.is_nota_di_credito");
            sql.AppendLine($"FROM TblFatture AS A");
            sql.AppendLine($"LEFT JOIN (");
            sql.AppendLine($"  SELECT id_fatture,");
            sql.AppendLine($"  (SELECT STUFF((SELECT '; ' + Cant.CodCant FROM TblCantieri AS Cant INNER JOIN TblFattureCantieri AS Fc ON Cant.IdCantieri = Fc.id_cantieri WHERE Fc.id_fatture = FatCant.id_fatture FOR XML PATH('')),1,1,'')) AS Cantieri");
            sql.AppendLine($"  FROM TblFattureCantieri AS FatCant");
            sql.AppendLine($") AS B ON A.id_fatture = B.id_fatture");
            sql.AppendLine($"LEFT JOIN (");
            sql.AppendLine($"  SELECT id_fatture,");
            sql.AppendLine($"  (SELECT STUFF((SELECT '; ' + CONVERT(nvarchar, valore_acconto) FROM TblFattureAcconti AS Fa WHERE FatAcc.id_fatture = Fa.id_fatture FOR XML PATH('')),1,1,'')) AS Acconti");
            sql.AppendLine($"  FROM TblFattureAcconti AS FatAcc");
            sql.AppendLine($") AS C ON A.id_fatture = C.id_fatture");
            sql.AppendLine($"LEFT JOIN TblAmministratori AS D ON A.id_amministratori = D.id_amministratori");
            sql.AppendLine($"INNER JOIN TblClienti AS E ON A.id_clienti = E.IdCliente");
            sql.AppendLine($"WHERE E.RagSocCli LIKE '%{cliente}%' {whereData} AND {whereAmministratore}");
            sql.AppendLine(anno != "" ? "AND DATEPART(YEAR, A.data) = @anno " : " ");
            sql.AppendLine(numeroFattura > 0 ? "AND A.numero = @numeroFattura " : "");
            sql.AppendLine(riscosso == 1 ? "" : (riscosso == 2 ? " AND A.riscosso = 1 " : " AND A.riscosso = 0"));
            sql.AppendLine($"ORDER BY YEAR(A.data), A.numero");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Fattura>(sql.ToString(), new { anno, dataDa, dataA, cliente, cantiere, amministratore, numeroFattura }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetFatture in FattureDAO", ex);
            }
            return ret;
        }

        public static List<Fattura> GetAll()
        {
            List<Fattura> ret = new List<Fattura>();
            StringBuilder sql = new StringBuilder("SELECT * FROM TblFatture ORDER BY numero");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Fattura>(sql.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetAll in FattureDAO", ex);
            }
            return ret;
        }

        public static Fattura GetSingle(long idFattura)
        {
            Fattura ret = new Fattura();
            StringBuilder sql = new StringBuilder("SELECT * FROM TblFatture WHERE id_fatture = @idFattura ORDER BY numero");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Fattura>(sql.ToString(), new { idFattura }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore durante il recupero del singolo Fattura, id = {idFattura}", ex);
            }
            return ret;
        }

        public static List<(string quarter, double totaleIva)> GetTotaliIvaPerQuarter(int anno)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT (CASE WHEN DATEPART(QUARTER, data) = 1 THEN 'Gen-Feb-Mar' WHEN DATEPART(QUARTER, data) = 2 THEN 'Apr-Mag-Giu' WHEN DATEPART(QUARTER, data) = 3 THEN 'Lug-Ago-Set' WHEN DATEPART(QUARTER, data) = 4 THEN 'Ott-Nov-Dic' END) Trimestre,");
            sql.AppendLine($"       SUM(imponibile * iva / 100) TotaleIva");
            sql.AppendLine($"FROM TblFatture WHERE DATEPART(YEAR, data) = @anno");
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
            sql.AppendLine($"FROM TblFatture WHERE DATEPART(YEAR, data) = @anno");
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
            sql.AppendLine($"FROM TblFatture WHERE DATEPART(YEAR, data) = @anno");
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

        public static List<(string titolo, double valore)> GetTotaliFatture(string cliente, string amministratore, string annoString, int numeroFattura, int riscosso, string dataDa, string dataA, decimal valoreAccontiDaRiscuotere)
        {
            StringBuilder sql = new StringBuilder();
            string whereData = (dataDa == "" && dataA == "") ? "" : ((dataDa != "" && dataA == "") ? " AND A.data >= @dataDa " : ((dataDa == "" && dataA != "") ? " AND A.data <= @dataA " : " AND A.Data BETWEEN @dataDa AND @dataA "));
            string whereAmministratore = "(" + (amministratore == "" ? "C.nome IS NULL OR " : "") + $"C.nome LIKE '%{amministratore}%') ";
            string whereRiscosso = riscosso == 1 ? "" : (riscosso == 2 ? " AND A.riscosso = 1 " : " AND A.riscosso = 0 ");
            string whereNumeroFattura = numeroFattura > 0 ? " AND A.numero = @numeroFattura " : "";
            string where = $"WHERE B.RagSocCli LIKE '%{cliente}%' AND {whereAmministratore}" + (annoString == "" ? "" : $"AND DATEPART(YEAR, A.data) = {Convert.ToInt32(annoString)} {whereNumeroFattura} {whereData} {whereRiscosso} ");

            sql.AppendLine($"SELECT 'Totale Iva' as Titolo, ISNULL(SUM(imponibile * iva / 100), 0) AS Valore, 1 as Ordine");
            sql.AppendLine($"FROM TblFatture AS A");
            sql.AppendLine($"INNER JOIN TblClienti AS B ON A.id_clienti = B.IdCliente");
            sql.AppendLine($"LEFT JOIN TblAmministratori AS C ON A.id_amministratori = C.id_amministratori");
            sql.AppendLine(where);
            sql.AppendLine($"UNION");
            sql.AppendLine($"SELECT 'Totale Imponibile' as Titolo, ISNULL(SUM(imponibile), 0) AS Valore, 2 as Ordine");
            sql.AppendLine($"FROM TblFatture AS A");
            sql.AppendLine($"INNER JOIN TblClienti AS B ON A.id_clienti = B.IdCliente");
            sql.AppendLine($"LEFT JOIN TblAmministratori AS C ON A.id_amministratori = C.id_amministratori");
            sql.AppendLine(where);
            sql.AppendLine($"UNION");
            sql.AppendLine($"SELECT 'Totale Importo' as Titolo, ISNULL(SUM(imponibile + (imponibile * iva / 100) - (imponibile * ritenuta_acconto / 100)), 0) AS Valore, 3 as Ordine");
            sql.AppendLine($"FROM TblFatture AS A");
            sql.AppendLine($"INNER JOIN TblClienti AS B ON A.id_clienti = B.IdCliente");
            sql.AppendLine($"LEFT JOIN TblAmministratori AS C ON A.id_amministratori = C.id_amministratori");
            sql.AppendLine(where);
            sql.AppendLine($"UNION");
            sql.AppendLine($"SELECT 'Totale Da Riscuotere' as Titolo, ISNULL(SUM(imponibile + (imponibile * iva / 100) - (imponibile * ritenuta_acconto / 100)) - @valoreAccontiDaRiscuotere, 0) AS Valore, 4 as Ordine");
            sql.AppendLine($"FROM TblFatture AS A");
            sql.AppendLine($"INNER JOIN TblClienti AS B ON A.id_clienti = B.IdCliente");
            sql.AppendLine($"LEFT JOIN TblAmministratori AS C ON A.id_amministratori = C.id_amministratori");
            sql.AppendLine($"{where} AND riscosso = 0 ");
            sql.AppendLine($"ORDER BY Ordine ");
            try
            {

                using (SqlConnection cn = GetConnection())
                {
                    return cn.Query<(string, double)>(sql.ToString(), new { dataDa, dataA, numeroFattura, riscosso, valoreAccontiDaRiscuotere }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetTotaliFatture", ex);
            }
        }

        public static long Insert(Fattura p)
        {
            long ret = 0;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"INSERT INTO TblFatture (id_clienti,id_amministratori,numero,data,riscosso,imponibile,iva,ritenuta_acconto,reverse_charge,is_nota_di_credito)");
            sql.AppendLine($"VALUES (@IdClienti,@IdAmministratori,@Numero,@Data,@Riscosso,@Imponibile,@Iva,@RitenutaAcconto,@ReverseCharge,@IsNotaDiCredito)");
            sql.AppendLine($"SELECT CAST(scope_identity() AS bigint)");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<long>(sql.ToString(), p).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore durante l'inserimento del Fattura = {p.Numero}", ex);
            }
            return ret;
        }

        public static bool Update(Fattura p)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"UPDATE TblFatture");
            sql.AppendLine($"SET id_clienti = @IdClienti, id_amministratori = @IdAmministratori, numero = @Numero, data = @Data, riscosso = @Riscosso, imponibile = @Imponibile,");
            sql.AppendLine($"iva = @Iva, ritenuta_acconto = @RitenutaAcconto, reverse_charge = @ReverseCharge, is_nota_di_credito = @IsNotaDiCredito");
            sql.AppendLine($"WHERE id_fatture = @IdFatture");
            try
            {

                using (SqlConnection cn = GetConnection())
                {
                    return cn.Execute(sql.ToString(), p) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore durante l'aggiornamento del Fattura = {p.Numero}", ex);
            }
        }

        public static void Delete(long idFattura)
        {
            StringBuilder sql = new StringBuilder("DELETE FROM TblFatture WHERE id_fatture = @idFattura");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql.ToString(), new { idFattura });
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore durante l'eliminazione del Fattura con id = {idFattura}", ex);
            }
        }
    }
}