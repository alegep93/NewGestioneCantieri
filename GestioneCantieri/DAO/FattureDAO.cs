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
        public static List<Fattura> GetFatture(string anno, string dataDa, string dataA, string cliente, string cantiere, string amministratore, int numeroFattura, int riscosso)
        {
            SqlConnection cn = GetConnection();
            cliente = "%" + cliente + "%";
            cantiere = "%" + cantiere + "%";
            amministratore = "%" + amministratore + "%";

            string whereData = (dataDa == "" && dataA == "") ? "" : ((dataDa != "" && dataA == "") ? " AND A.data >= @dataDa " : ((dataDa == "" && dataA != "") ? " AND A.data <= @dataA " : " AND A.Data BETWEEN @dataDa AND @dataA "));
            string whereAmministratore = "(" + (amministratore == "%%" ? "D.nome IS NULL OR " : "") + $" D.nome LIKE '{amministratore}') ";

            string sql = "SELECT DISTINCT A.id_fatture, E.RagSocCli AS RagioneSocialeCliente, B.Cantieri, C.Acconti, D.nome AS NomeAmministratore, A.numero, A.data, A.imponibile, A.iva, A.ritenuta_acconto, A.reverse_charge, A.riscosso, A.is_nota_di_credito " +
                         "FROM TblFatture AS A " +
                         "LEFT JOIN ( " +
                         "  SELECT id_fatture, " +
                         "  (SELECT STUFF((SELECT '; ' + Cant.CodCant FROM TblCantieri AS Cant INNER JOIN TblFattureCantieri AS Fc ON Cant.IdCantieri = Fc.id_cantieri WHERE Fc.id_fatture = FatCant.id_fatture FOR XML PATH('')),1,1,'')) AS Cantieri " +
                         "  FROM TblFattureCantieri AS FatCant " +
                         ") AS B ON A.id_fatture = B.id_fatture " +
                         "LEFT JOIN ( " +
                         "  SELECT id_fatture, " +
                         "  (SELECT STUFF((SELECT '; ' + CONVERT(nvarchar, valore_acconto) FROM TblFattureAcconti AS Fa WHERE FatAcc.id_fatture = Fa.id_fatture FOR XML PATH('')),1,1,'')) AS Acconti " +
                         "  FROM TblFattureAcconti AS FatAcc " +
                         ") AS C ON A.id_fatture = C.id_fatture " +
                         "LEFT JOIN TblAmministratori AS D ON A.id_amministratori = D.id_amministratori " +
                         "INNER JOIN TblClienti AS E ON A.id_clienti = E.IdCliente " +
                         "WHERE E.RagSocCli LIKE @cliente " + whereData + " AND " + whereAmministratore;
            sql += anno != "" ? "AND DATEPART(YEAR, A.data) = @anno " : " ";
            sql += numeroFattura > 0 ? "AND A.numero = @numeroFattura " : "";
            sql += riscosso == 1 ? "" : (riscosso == 2 ? " AND A.riscosso = 1 " : " AND A.riscosso = 0 ");
            sql += "ORDER BY A.data, A.numero ";

            try
            {
                return cn.Query<Fattura>(sql, new { anno, dataDa, dataA, cliente, cantiere, amministratore, numeroFattura }).ToList();
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

        internal static double GetTotaleImponibile()
        {
            try
            {
                string sql = "SELECT ISNULL(SUM(imponibile), 0) FROM TblFatture";
                using (SqlConnection cn = GetConnection())
                {
                    return cn.Query<double>(sql).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetTotaleImponibile in FattureDAO", ex);
            }
        }

        internal static double GetTotaleFatturato()
        {
            try
            {
                string sql = "SELECT ISNULL(SUM(imponibile + (imponibile * iva / 100) - (imponibile * ritenuta_acconto / 100)), 0) FROM TblFatture";
                using (SqlConnection cn = GetConnection())
                {
                    return cn.Query<double>(sql).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetTotaleFatturato in FattureDAO", ex);
            }
        }

        public static List<(string quarter, double totaleIva)> GetTotaliIvaPerQuarter(int anno)
        {
            try
            {
                string sql = "SELECT (CASE WHEN DATEPART(QUARTER, data) = 1 THEN 'Gen-Feb-Mar' WHEN DATEPART(QUARTER, data) = 2 THEN 'Apr-Mag-Giu' WHEN DATEPART(QUARTER, data) = 3 THEN 'Lug-Ago-Set' WHEN DATEPART(QUARTER, data) = 4 THEN 'Ott-Nov-Dic' END) Trimestre, " +
                             "       SUM(imponibile * iva / 100) TotaleIva " +
                             $"FROM TblFatture WHERE DATEPART(YEAR, data) = {anno} GROUP BY DATEPART(QUARTER, data)";
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
                             $"FROM TblFatture WHERE DATEPART(YEAR, data) = {anno} GROUP BY DATEPART(QUARTER, data)";
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
                             $"FROM TblFatture WHERE DATEPART(YEAR, data) = {anno} GROUP BY DATEPART(QUARTER, data)";
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

        public static List<(string titolo, double valore)> GetTotaliFatture(string cliente, string amministratore, string annoString, int numeroFattura, int riscosso, string dataDa, string dataA)
        {
            try
            {
                string whereData = (dataDa == "" && dataA == "") ? "" : ((dataDa != "" && dataA == "") ? " AND A.data >= @dataDa " : ((dataDa == "" && dataA != "") ? " AND A.data <= @dataA " : " AND A.Data BETWEEN @dataDa AND @dataA "));
                string whereAmministratore = "(" + (amministratore == "" ? "C.nome IS NULL OR " : "") + $"C.nome LIKE '%{amministratore}%') ";
                string whereRiscosso = riscosso == 1 ? "" : (riscosso == 2 ? " AND A.riscosso = 1 " : " AND A.riscosso = 0 ");
                string whereNumeroFattura = numeroFattura > 0 ? " AND A.numero = @numeroFattura " : "";
                string where = $"WHERE B.RagSocCli LIKE '%{cliente}%' AND {whereAmministratore} " + (annoString == "" ? "" : $"AND DATEPART(YEAR, A.data) = {Convert.ToInt32(annoString)} {whereNumeroFattura} {whereData} {whereRiscosso} ");

                string sql = "SELECT 'Totale Iva' as Titolo, ISNULL(SUM(imponibile * iva / 100), 0) AS Valore, 1 as Ordine " +
                             "FROM TblFatture AS A " +
                             "INNER JOIN TblClienti AS B ON A.id_clienti = B.IdCliente " +
                             "LEFT JOIN TblAmministratori AS C ON A.id_amministratori = C.id_amministratori " + where;
                sql += "UNION " +
                             "SELECT 'Totale Imponibile' as Titolo, ISNULL(SUM(imponibile), 0) AS Valore, 2 as Ordine " +
                             "FROM TblFatture AS A " +
                             "INNER JOIN TblClienti AS B ON A.id_clienti = B.IdCliente " +
                             "LEFT JOIN TblAmministratori AS C ON A.id_amministratori = C.id_amministratori " + where;
                sql += "UNION " +
                             "SELECT 'Totale Importo' as Titolo, ISNULL(SUM(imponibile + (imponibile * iva / 100) - (imponibile * ritenuta_acconto / 100)), 0) AS Valore, 3 as Ordine " +
                             "FROM TblFatture AS A " +
                             "INNER JOIN TblClienti AS B ON A.id_clienti = B.IdCliente " +
                             "LEFT JOIN TblAmministratori AS C ON A.id_amministratori = C.id_amministratori " + where;
                sql += "UNION " +
                             "SELECT 'Totale Da Riscuotere' as Titolo, ISNULL(SUM(imponibile + (imponibile * iva / 100) - (imponibile * ritenuta_acconto / 100)), 0) AS Valore, 4 as Ordine " +
                             "FROM TblFatture AS A " +
                             "INNER JOIN TblClienti AS B ON A.id_clienti = B.IdCliente " +
                             "LEFT JOIN TblAmministratori AS C ON A.id_amministratori = C.id_amministratori " + where + " AND riscosso = 0 ";
                sql += "ORDER BY Ordine ";

                using (SqlConnection cn = GetConnection())
                {
                    return cn.Query<(string, double)>(sql, new { dataDa, dataA, numeroFattura, riscosso }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetTotaliFatture", ex);
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