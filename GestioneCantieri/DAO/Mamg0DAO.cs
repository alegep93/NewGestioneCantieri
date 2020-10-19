using Dapper;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Utils;

namespace GestioneCantieri.DAO
{
    public class Mamg0DAO : BaseDAO
    {
        // SELECT
        public static List<Mamg0> GetAll()
        {
            List<Mamg0> ret = new List<Mamg0>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT TOP 500 (AA_SIGF + AA_CODF) AS CodArt, AA_DES AS 'Desc', AA_UM AS UnitMis, AA_PZ AS Pezzo, AA_PRZ AS PrezzoListino,");
            sql.AppendLine($"AA_SCONTO1 AS Sconto1, AA_SCONTO2 AS Sconto2, AA_SCONTO3 AS Sconto3, AA_PRZ1 AS PrezzoNetto");
            sql.AppendLine($"FROM MAMG0");
            sql.AppendLine($"ORDER BY CodArt ASC");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Mamg0>(sql.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero del listino", ex);
            }
            return ret;
        }

        public static List<Mamg0> GetListino(string codArt1, string codArt2, string codArt3, string desc1, string desc2, string desc3)
        {
            List<Mamg0> ret = new List<Mamg0>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT (AA_SIGF + AA_CODF) AS CodArt, AA_DES AS 'Desc', AA_UM AS UnitMis, AA_PZ AS Pezzo, AA_PRZ AS PrezzoListino,");
            sql.AppendLine($"AA_SCONTO1 AS Sconto1, AA_SCONTO2 AS Sconto2, AA_SCONTO3 AS Sconto3, AA_PRZ1 AS PrezzoNetto");
            sql.AppendLine($"FROM MAMG0");
            sql.AppendLine($"WHERE (AA_SIGF + AA_CODF) LIKE '%{codArt1}%' AND (AA_SIGF + AA_CODF) LIKE '%{codArt2}%' AND (AA_SIGF + AA_CODF) LIKE '%{codArt3}%'");
            sql.AppendLine($"AND AA_DES LIKE '%{desc1}%' AND AA_DES LIKE '%{desc2}%' AND AA_DES LIKE '%{desc3}%'");
            sql.AppendLine($"ORDER BY CodArt ASC");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Mamg0>(sql.ToString(), new { codArt1, codArt2, codArt3, desc1, desc2, desc3 }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero del listino con i filtri", ex);
            }
            return ret;
        }

        public static double GetPrezzoDiListino(string sigf, string codf)
        {
            double ret = 0;
            StringBuilder sql = new StringBuilder($"SELECT AA_PRZ AS prezzoListino FROM MAMG0 WHERE AA_SIGF = @sigf AND AA_CODF = @codf");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<double>(sql.ToString(), new { sigf, codf }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetPrezzoDiListino in Mamg0DAO", ex);
            }
            return ret;
        }

        // INSERT
        public static bool InserisciListino(Mamg0ForDBF mmgDbf)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"INSERT INTO MAMG0 (AA_COD, AA_SIGF, AA_CODF, AA_DES, AA_UM, AA_PZ, AA_IVA, AA_VAL, AA_PRZ, AA_CODFSS, AA_GRUPPO,");
            sql.AppendLine($"AA_SCONTO1, AA_SCONTO2, AA_SCONTO3, AA_CFZMIN, AA_MGZ, AA_CUB, AA_PRZ1, AA_DATA1, AA_EAN, WOMAME, WOFOME,");
            sql.AppendLine($"WOPDME, WOFMSC, WOFMST, RAME)");
            sql.AppendLine($"VALUES (@AA_COD, @AA_SIGF, @AA_CODF, @AA_DES, @AA_UM, @AA_PZ, @AA_IVA, @AA_VAL, @AA_PRZ, @AA_CODFSS, @AA_GRUPPO,");
            sql.AppendLine($"@AA_SCONTO1, @AA_SCONTO2, @AA_SCONTO3, @AA_CFZMIN, @AA_MGZ, @AA_CUB, @AA_PRZ1, @AA_DATA1, @AA_EAN, @WOMAME, @WOFOME,");
            sql.AppendLine($"@WOPDME, @WOFMSC, @WOFMST, @RAME)");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString()) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'inserimento del listino", ex);
            }
            return ret;
        }
        public static void GetDataFromExcelAndInsertBulkCopy(string filePath, DBTransaction tr)
        {
            try
            {
                OleDbConnectionStringBuilder OleStringBuilder = new OleDbConnectionStringBuilder(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source =" + filePath + ";Extended Properties = 'Excel 12.0;HDR=Yes;IMEX=1';Persist Security Info=False;")
                {
                    DataSource = filePath
                };

                using (OleDbConnection ExcelConection = new OleDbConnection(OleStringBuilder.ConnectionString))
                {

                    using (OleDbDataAdapter adaptor = new OleDbDataAdapter("SELECT * FROM [Mamg0$]", ExcelConection))
                    {
                        DataSet ds = new DataSet();
                        adaptor.Fill(ds);

                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(tr.Connection, SqlBulkCopyOptions.KeepIdentity, tr.Transaction))
                        {
                            bulkCopy.DestinationTableName = "dbo.MAMG0";

                            try
                            {
                                // Write from the source to the destination.
                                bulkCopy.WriteToServer(ds.Tables[0]);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("Errore durante la copia massiva dei dati", ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'importazione del listino Mamg0", ex);
            }
        }
        public static void InsertAll(List<Mamg0ForDBF> items)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"INSERT INTO MAMG0 (AA_COD, AA_SIGF, AA_CODF, AA_DES, AA_UM, AA_PZ, AA_IVA, AA_VAL, AA_PRZ, AA_CODFSS, AA_GRUPPO,");
            sql.AppendLine($"AA_SCONTO1, AA_SCONTO2, AA_SCONTO3, AA_CFZMIN, AA_MGZ, AA_CUB, AA_PRZ1, AA_DATA1, AA_EAN, WOMAME, WOFOME,");
            sql.AppendLine($"WOPDME, WOFMSC, WOFMST, RAME)");
            sql.AppendLine($"VALUES (@AA_COD, @AA_SIGF, @AA_CODF, @AA_DES, @AA_UM, @AA_PZ, @AA_IVA, @AA_VAL, @AA_PRZ, @AA_CODFSS, @AA_GRUPPO,");
            sql.AppendLine($"@AA_SCONTO1, @AA_SCONTO2, @AA_SCONTO3, @AA_CFZMIN, @AA_MGZ, @AA_CUB, @AA_PRZ1, @AA_DATA1, @AA_EAN, @WOMAME, @WOFOME,");
            sql.AppendLine($"@WOPDME, @WOFMSC, @WOFMST, @RAME)");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql.ToString(), items);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'inserimento del listino", ex);
            }
        }

        //public static void InsertIntoDBF(string pathFile)
        //{
        //    string excelConnectionString = "Provider = vfpoledb; Data Source ="); pathFile + "); Collating Sequence = machine");
        //    string commandText = "SELECT AA_COD, AA_SIGF, AA_CODF, AA_DES, AA_UM, AA_PZ, AA_IVA, AA_VAL, AA_PRZ, AA_CODFSS, AA_GRUPPO,");
        //    "AA_SCONTO1, AA_SCONTO2, AA_SCONTO3, AA_CFZMIN, AA_MGZ, AA_CUB, AA_PRZ1, AA_DATA1, AA_EAN, WOMAME, WOFOME, WOPDME, WOFMSC, WOFMST, RAME");
        //    "INTO MAMG0 FROM OPENROWSET('vfpoledb','"); pathFile + "Mamg0.DBF';'';");
        //    "'','SELECT AA_COD, AA_SIGF, AA_CODF, AA_DES, AA_UM, AA_PZ, AA_IVA, AA_VAL, AA_PRZ, AA_CODFSS, AA_GRUPPO,");
        //    "AA_SCONTO1, AA_SCONTO2, AA_SCONTO3, AA_CFZMIN, AA_MGZ, AA_CUB, AA_PRZ1, AA_DATA1, AA_EAN, WOMAME, WOFOME, WOPDME, WOFMSC, WOFMST, RAME");
        //    "FROM"); pathFile + "Mamg0.DBF')");

        //    //OleDbConnection ExcelConection = null;

        //    try
        //    {
        //        using (SqlConnection cn = GetConnection())
        //        {
        //            cn.Query(commandText);
        //        }

        //        //OleDbConnectionStringBuilder OleStringBuilder = new OleDbConnectionStringBuilder(excelConnectionString);
        //        //OleStringBuilder.DataSource = pathFile;
        //        //ExcelConection = new OleDbConnection();
        //        //ExcelConection.ConnectionString = OleStringBuilder.ConnectionString;
        //        //
        //        //using (OleDbDataAdapter adaptor = new OleDbDataAdapter(commandText, ExcelConection))
        //        //{
        //        //    DataSet ds = new DataSet();
        //        //    adaptor.Fill(ds);
        //        //    ExcelConection.Open();

        //        //foreach (DataRow row in ds.Tables[0].Rows)
        //        //{
        //        //    Mamg0ForDBF mmg = new Mamg0ForDBF();
        //        //    mmg.AA_COD = row.ItemArray[0].ToString().Trim();
        //        //    mmg.AA_SIGF = row.ItemArray[1].ToString().Trim();
        //        //    mmg.AA_CODF = row.ItemArray[2].ToString().Trim();
        //        //    mmg.AA_DES = row.ItemArray[3].ToString().Trim();
        //        //    mmg.AA_UM = row.ItemArray[4].ToString().Trim();
        //        //    mmg.AA_PZ = Convert.ToDouble(row.ItemArray[5]);
        //        //    mmg.AA_IVA = Convert.ToInt32(row.ItemArray[6]);
        //        //    mmg.AA_VAL = row.ItemArray[7].ToString().Trim();
        //        //    mmg.AA_PRZ = Convert.ToDouble(row.ItemArray[8]);
        //        //    mmg.AA_CODFSS = row.ItemArray[9].ToString().Trim();
        //        //    mmg.AA_GRUPPO = row.ItemArray[10].ToString().Trim();
        //        //    mmg.AA_SCONTO1 = Convert.ToDouble(row.ItemArray[11]);
        //        //    mmg.AA_SCONTO2 = Convert.ToDouble(row.ItemArray[12]);
        //        //    mmg.AA_SCONTO3 = Convert.ToDouble(row.ItemArray[13]);
        //        //    mmg.AA_CFZMIN = Convert.ToDouble(row.ItemArray[14]);
        //        //    mmg.AA_MGZ = row.ItemArray[15].ToString().Trim();
        //        //    mmg.AA_CUB = Convert.ToDouble(row.ItemArray[16]);
        //        //    mmg.AA_PRZ1 = Convert.ToDouble(row.ItemArray[17]);
        //        //    mmg.AA_DATA1 = Convert.ToDateTime(row.ItemArray[18]);
        //        //    mmg.AA_EAN = row.ItemArray[19].ToString().Trim();
        //        //    mmg.WOMAME = row.ItemArray[20].ToString().Trim();
        //        //    mmg.WOFOME = row.ItemArray[21].ToString().Trim();
        //        //    mmg.WOPDME = row.ItemArray[22].ToString().Trim();
        //        //    mmg.WOFMSC = row.ItemArray[23].ToString().Trim();
        //        //    mmg.WOFMST = row.ItemArray[24].ToString().Trim();
        //        //    mmg.RAME = Convert.ToDouble(row.ItemArray[25]);
        //        //
        //        //    list.Add(mmg);
        //        //}
        //        //}
        //        //return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Errore durante l'importazione del Listino in Mamg0", ex);
        //    }
        //    finally
        //    {
        //        //ExcelConection.Close();
        //    }
        //}

        // DELETE

        public static bool EliminaListino(DBTransaction tr)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder("DELETE FROM MAMG0");
            try
            {
                ret = tr.Connection.Execute(sql.ToString(), transaction: tr.Transaction) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'eliminazione del listino", ex);
            }
            return ret;
        }
    }
}