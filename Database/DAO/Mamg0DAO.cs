using ClosedXML.Excel;
using Dapper;
using Database.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Utils;
using Z.Dapper.Plus;

namespace Database.DAO
{
    public class Mamg0DAO : BaseDAO
    {
        // SELECT
        //public static List<Mamg0> GetAll()
        //{
        //    List<Mamg0> ret = new List<Mamg0>();
        //    StringBuilder sql = new StringBuilder();
        //    sql.AppendLine($"SELECT TOP 500 (AA_SIGF + AA_CODF) AS CodArt, AA_DES AS 'Desc', AA_UM AS UnitMis, AA_PZ AS Pezzo, AA_PRZ AS PrezzoListino,");
        //    sql.AppendLine($"AA_SCONTO1 AS Sconto1, AA_SCONTO2 AS Sconto2, AA_SCONTO3 AS Sconto3, AA_PRZ1 AS PrezzoNetto, CodiceListinoUnivoco");
        //    sql.AppendLine($"FROM MAMG0");
        //    sql.AppendLine($"ORDER BY CodArt ASC");
        //    try
        //    {
        //        using (SqlConnection cn = GetConnection())
        //        {
        //            ret = cn.Query<Mamg0>(sql.ToString()).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Errore durante il recupero del listino", ex);
        //    }
        //    return ret;
        //}

        //public static List<Mamg0> GetAll()
        //{
        //    List<Mamg0> ret = new List<Mamg0>();
        //    StringBuilder sql = new StringBuilder();
        //    sql.AppendLine($"SELECT TOP 500 CodArt, 'Desc', Pezzo, PrezzoListino, PrezzoNetto");
        //    sql.AppendLine($"FROM TblListinoMef");
        //    sql.AppendLine($"ORDER BY CodArt ASC");
        //    try
        //    {
        //        using (SqlConnection cn = GetConnection())
        //        {
        //            ret = cn.Query<Mamg0>(sql.ToString()).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Errore durante il recupero del listino", ex);
        //    }
        //    return ret;
        //}

        public static List<Mamg0> GetAll()
        {
            List<Mamg0> ret = new List<Mamg0>();
            StringBuilder sql = new StringBuilder();
            try
            {
                sql.AppendLine($"SELECT * FROM TblListinoMef");
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Mamg0>(sql.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetAll del listino", ex);
            }
            return ret;
        }

        public static List<Mamg0> GetListino(string codArt1, string codArt2, string codArt3, string desc1, string desc2, string desc3, bool limitResults = true)
        {
            List<Mamg0> ret = new List<Mamg0>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT {(limitResults ? "TOP 500" : "")} *");
            sql.AppendLine($"FROM TblListinoMef");
            sql.AppendLine($"WHERE CodArt LIKE '%{codArt1}%' AND CodArt LIKE '%{codArt2}%' AND CodArt LIKE '%{codArt3}%'");
            sql.AppendLine($"AND [Desc] LIKE '%{desc1}%' AND [Desc] LIKE '%{desc2}%' AND [Desc] LIKE '%{desc3}%'");
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

        public static Mamg0 GetSingle(string codiceListinoUnivoco)
        {
            Mamg0 ret = new Mamg0();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT (AA_SIGF + AA_COD) AS CodArt, AA_DES AS 'Desc', AA_UM AS UnitMis, AA_PZ AS Pezzo, AA_PRZ AS PrezzoListino,");
            sql.AppendLine($"AA_SCONTO1 AS Sconto1, AA_SCONTO2 AS Sconto2, AA_SCONTO3 AS Sconto3, AA_PRZ1 AS PrezzoNetto, CodiceListinoUnivoco");
            sql.AppendLine($"FROM MAMG0");
            sql.AppendLine($"WHERE (AA_COD + AA_SIGF) = @codiceListinoUnivoco");
            sql.AppendLine($"ORDER BY CodArt ASC");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Mamg0>(sql.ToString(), new { codiceListinoUnivoco }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero del listino", ex);
            }
            return ret;
        }

        public static double GetPrezzoDiListino(string sigf, string codf)
        {
            double ret = 0;
            StringBuilder sql = new StringBuilder($"SELECT PrezzoListino FROM TblListinoMef WHERE CodArt = (@sigf + @codf)");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<double>(sql.ToString(), new { sigf, codf }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetPrezzoDiListino in TblListinoMef", ex);
            }
            return ret;
        }

        public static double GetPrezzoDiListino(string codArt)
        {
            double ret = 0;
            StringBuilder sql = new StringBuilder($"SELECT PrezzoListino FROM TblListinoMef WHERE CodArt = @codArt");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<double>(sql.ToString(), new { codArt }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetPrezzoDiListino in TblListinoMef", ex);
            }
            return ret;
        }

        //public static void GetDataFromExcelAndInsertBulkCopy(string filePath, DBTransaction tr)
        //{
        //    try
        //    {
        //        var workbook = new XLWorkbook(filePath);
        //        var ws1 = workbook.Worksheet(1);
        //        int rowNumber = 2;
        //        var row = ws1.Row(rowNumber);
        //        bool empty = row.IsEmpty();
        //        List<Mamg0ForDBF> items = new List<Mamg0ForDBF>();
        //        while (!empty)
        //        {
        //            items.Add(new Mamg0ForDBF
        //            {
        //                AA_COD = row.Cell(1).GetString(),
        //                AA_SIGF = row.Cell(2).GetString(),
        //                AA_CODF = row.Cell(3).GetString(),
        //                AA_DES = row.Cell(4).GetString(),
        //                AA_UM = row.Cell(5).GetString(),
        //                AA_PZ = row.Cell(6).GetDouble(),
        //                AA_IVA = Convert.ToInt32(row.Cell(7).GetDouble()),
        //                AA_VAL = row.Cell(8).GetString(),
        //                AA_PRZ = row.Cell(9).GetDouble(),
        //                AA_CODFSS = row.Cell(10).GetString(),
        //                AA_GRUPPO = row.Cell(11).GetString(),
        //                AA_SCONTO1 = row.Cell(12).GetDouble(),
        //                AA_SCONTO2 = row.Cell(13).GetDouble(),
        //                AA_SCONTO3 = row.Cell(14).GetDouble(),
        //                AA_CFZMIN = row.Cell(15).GetDouble(),
        //                AA_MGZ = row.Cell(16).GetString(),
        //                AA_CUB = row.Cell(17).GetDouble(),
        //                AA_PRZ1 = row.Cell(18).GetDouble(),
        //                AA_DATA1 = row.Cell(19).GetDateTime(),
        //                AA_EAN = row.Cell(20).GetString(),
        //                WOMAME = row.Cell(21).GetString(),
        //                WOFOME = row.Cell(22).GetString(),
        //                WOPDME = row.Cell(23).GetString(),
        //                WOFMSC = row.Cell(24).GetString(),
        //                WOFMST = row.Cell(25).GetString(),
        //                RAME = row.Cell(26).GetDouble()
        //            });
        //            row = ws1.Row(rowNumber += 1);
        //            empty = row.IsEmpty();
        //        }

        //        InsertAll(items, tr);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Errore durante l'importazione del listino Mamg0", ex);
        //    }
        //}

        // INSERT
        //public static bool InserisciListino(Mamg0ForDBF mmgDbf)
        //{
        //    bool ret = false;
        //    StringBuilder sql = new StringBuilder();
        //    sql.AppendLine($"INSERT INTO MAMG0 (AA_COD, AA_SIGF, AA_CODF, AA_DES, AA_UM, AA_PZ, AA_IVA, AA_VAL, AA_PRZ, AA_CODFSS, AA_GRUPPO,");
        //    sql.AppendLine($"AA_SCONTO1, AA_SCONTO2, AA_SCONTO3, AA_CFZMIN, AA_MGZ, AA_CUB, AA_PRZ1, AA_DATA1, AA_EAN, WOMAME, WOFOME,");
        //    sql.AppendLine($"WOPDME, WOFMSC, WOFMST, RAME)");
        //    sql.AppendLine($"VALUES (@AA_COD, @AA_SIGF, @AA_CODF, @AA_DES, @AA_UM, @AA_PZ, @AA_IVA, @AA_VAL, @AA_PRZ, @AA_CODFSS, @AA_GRUPPO,");
        //    sql.AppendLine($"@AA_SCONTO1, @AA_SCONTO2, @AA_SCONTO3, @AA_CFZMIN, @AA_MGZ, @AA_CUB, @AA_PRZ1, @AA_DATA1, @AA_EAN, @WOMAME, @WOFOME,");
        //    sql.AppendLine($"@WOPDME, @WOFMSC, @WOFMST, @RAME)");
        //    try
        //    {
        //        using (SqlConnection cn = GetConnection())
        //        {
        //            ret = cn.Execute(sql.ToString()) > 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Errore durante l'inserimento del listino", ex);
        //    }
        //    return ret;
        //}

        //public static void InsertAll(List<Mamg0ForDBF> items, DBTransaction tr)
        //{
        //    StringBuilder sql = new StringBuilder();
        //    sql.AppendLine($"INSERT INTO MAMG0 (AA_COD, AA_SIGF, AA_CODF, AA_DES, AA_UM, AA_PZ, AA_IVA, AA_VAL, AA_PRZ, AA_CODFSS, AA_GRUPPO,");
        //    sql.AppendLine($"AA_SCONTO1, AA_SCONTO2, AA_SCONTO3, AA_CFZMIN, AA_MGZ, AA_CUB, AA_PRZ1, AA_DATA1, AA_EAN, WOMAME, WOFOME,");
        //    sql.AppendLine($"WOPDME, WOFMSC, WOFMST, RAME)");
        //    sql.AppendLine($"VALUES (@AA_COD, @AA_SIGF, @AA_CODF, @AA_DES, @AA_UM, @AA_PZ, @AA_IVA, @AA_VAL, @AA_PRZ, @AA_CODFSS, @AA_GRUPPO,");
        //    sql.AppendLine($"@AA_SCONTO1, @AA_SCONTO2, @AA_SCONTO3, @AA_CFZMIN, @AA_MGZ, @AA_CUB, @AA_PRZ1, @AA_DATA1, @AA_EAN, @WOMAME, @WOFOME,");
        //    sql.AppendLine($"@WOPDME, @WOFMSC, @WOFMST, @RAME)");
        //    try
        //    {
        //        tr.Connection.Execute(sql.ToString(), items, tr.Transaction, commandTimeout: 600);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Errore durante l'inserimento del listino", ex);
        //    }
        //}

        public static void Insert(List<Mamg0> items, bool isForTmp, SqlConnection cn)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"INSERT INTO TblListinoMef{(isForTmp ? "Tmp" : "")} (CodArt, [Desc], Pezzo, PrezzoListino, PrezzoNetto, CodiceFornitore)");
            sql.AppendLine($"VALUES (@CodArt, @Desc, @Pezzo, @PrezzoListino, @PrezzoNetto, @CodiceFornitore)");
            try
            {
                //DapperPlusManager.Entity<Mamg0>().Table("TblListinoMef")
                //                                 .Map(item => item.CodArt, "CodArt")
                //                                 .Map(item => item.Desc, "Desc")
                //                                 .Map(item => item.Pezzo, "Pezzo")
                //                                 .Map(item => item.PrezzoListino, "PrezzoListino")
                //                                 .Map(item => item.PrezzoNetto, "PrezzoNetto")
                //                                 .Map(item => item.CodiceFornitore, "CodiceFornitore")
                //                                 .Ignore(item => item.Sconto1)
                //                                 .Ignore(item => item.Sconto2)
                //                                 .Ignore(item => item.Sconto3)
                //                                 .Ignore(item => item.UnitMis)
                //                                 .Ignore(item => item.CodiceListinoUnivoco);
                //tr.Connection.UseBulkOptions(opt => opt.Transaction = tr.Transaction).BulkInsert(items);
                cn.Execute(sql.ToString(), items);
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'inserimento del listino 'nuovo'", ex);
            }
        }

        public static void Update(List<Mamg0> items, DBTransaction tr)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"UPDATE TblListinoMef");
            sql.AppendLine($"SET CodArt = @CodArt, [Desc] = @Desc, PrezzoListino = @PrezzoListino, PrezzoNetto = @PrezzoNetto, CodiceFornitore = @CodiceFornitore, Pezzo = 0");
            sql.AppendLine($"WHERE IdListinoMef = @IdListinoMef");
            try
            {
                tr.Connection.Execute(sql.ToString(), items, tr.Transaction);
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'aggiornamento del listino", ex);
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

        public static void MergeListino(List<Mamg0> items, DBTransaction tr)
        {
            StringBuilder sql = new StringBuilder();
            try
            {
                sql.AppendLine($"MERGE TblListinoMef as prod");
                sql.AppendLine($"USING (SELECT @CodArt as CodArt, @CodiceFornitore as CodiceFornitore) AS Source");
                sql.AppendLine($"ON prod.CodArt = Source.CodArt AND prod.CodiceFornitore = Source.CodiceFornitore");
                sql.AppendLine($"WHEN MATCHED THEN");
                sql.AppendLine($"   UPDATE SET CodArt = @CodArt, [Desc] = @Desc, PrezzoListino = @PrezzoListino, PrezzoNetto = @PrezzoNetto, CodiceFornitore = @CodiceFornitore, Pezzo = 0");
                sql.AppendLine($"WHEN NOT MATCHED THEN");
                sql.AppendLine($"   INSERT(CodArt, [Desc], PrezzoListino, PrezzoNetto, CodiceFornitore, Pezzo) VALUES (@CodArt, @Desc, @PrezzoListino, @PrezzoNetto, @CodiceFornitore, 0);");
                tr.Connection.Execute(sql.ToString(), items, tr.Transaction, commandTimeout: 60 * 60 * 60);
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la MergeListino in Mamg0DAO", ex);
            }
        }

        public static List<Mamg0> GetDifferencesFromTmp(SqlConnection cn)
        {
            List<Mamg0> ret = new List<Mamg0>();
            StringBuilder sql = new StringBuilder();
            try
            {
                sql.AppendLine($"SELECT CodArt FROM TblListinoMefTmp");
                sql.AppendLine($"EXCEPT");
                sql.AppendLine($"SELECT CodArt FROM TblListinoMef");
                ret = cn.Query<Mamg0>(sql.ToString()).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetDifferencesFromTmp in Mamg0DAO", ex);
            }
            return ret;
        }

        // DELETE

        public static void DeleteListino(bool isForTmp, SqlConnection cn)
        {
            StringBuilder sql = new StringBuilder($"DELETE FROM TblListinoMef{(isForTmp ? "Tmp" : "")}");
            try
            {
                cn.Execute(sql.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'eliminazione del listino 'nuovo'", ex);
            }
        }
    }
}