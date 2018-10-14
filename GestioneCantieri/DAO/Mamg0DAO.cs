using Dapper;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;

namespace GestioneCantieri.DAO
{
    public class Mamg0DAO : BaseDAO
    {
        // SELECT
        public static List<Mamg0> getAll()
        {
            string sql = "";
            SqlConnection cn = GetConnection();

            try
            {
                sql = "SELECT TOP 500 (AA_SIGF + AA_CODF) AS CodArt, AA_DES AS 'Desc', AA_UM AS UnitMis, AA_PZ AS Pezzo, AA_PRZ AS PrezzoListino, " +
                      "AA_SCONTO1 AS Sconto1, AA_SCONTO2 AS Sconto2, AA_SCONTO3 AS Sconto3, AA_PRZ1 AS PrezzoNetto " +
                      "FROM MAMG0 " +
                      "ORDER BY CodArt ASC ";

                return cn.Query<Mamg0>(sql).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero del listino", ex);
            }
            finally
            {
                CloseResouces(cn, null);
            }
        }
        public static List<Mamg0> GetListino(string codArt1, string desc1)
        {
            string sql = "";
            SqlConnection cn = GetConnection();

            codArt1 = "%" + codArt1 + "%";
            desc1 = "%" + desc1 + "%";

            try
            {
                if (codArt1 == "%%" && desc1 == "%%")
                {
                    sql = "SELECT TOP 300 (AA_SIGF + AA_CODF) AS CodArt, AA_DES AS 'desc', AA_UM AS unitMis, AA_PZ AS pezzo, AA_PRZ AS prezzoListino, " +
                          "AA_SCONTO1 AS sconto1, AA_SCONTO2 AS sconto2, AA_SCONTO3 AS sconto3, AA_PRZ1 AS prezzoNetto " +
                          "FROM MAMG0 " +
                          "ORDER BY CodArt ASC ";
                }
                else
                {
                    sql = "SELECT (AA_SIGF + AA_CODF) AS CodArt, AA_DES AS 'desc', AA_UM AS unitMis, AA_PZ AS pezzo, AA_PRZ AS prezzoListino, " +
                          "AA_SCONTO1 AS sconto1, AA_SCONTO2 AS sconto2, AA_SCONTO3 AS sconto3, AA_PRZ1 AS prezzoNetto " +
                          "FROM MAMG0 " +
                          "WHERE (AA_SIGF + AA_CODF) LIKE @CodArt " +
                          "AND AA_DES LIKE @desc " +
                          "ORDER BY CodArt ASC ";
                }

                return cn.Query<Mamg0>(sql, new { CodArt = codArt1, desc = desc1 }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero del listino con i filtri", ex);
            }
            finally
            {
                CloseResouces(cn, null);
            }
        }
        public static List<Mamg0ForDBF> GetListinoFromDBF(string pathFile)
        {
            string excelConnectionString = "Provider = vfpoledb; Data Source = " + pathFile + "; Collating Sequence = machine";
            string commandText = "SELECT AA_COD, AA_SIGF, AA_CODF, AA_DES, AA_UM, AA_PZ, AA_IVA, AA_VAL, AA_PRZ, AA_CODFSS, AA_GRUPPO, " +
                                 "AA_SCONTO1, AA_SCONTO2, AA_SCONTO3, AA_CFZMIN, AA_MGZ, AA_CUB, AA_PRZ1, AA_DATA1, AA_EAN, WOMAME, WOFOME, WOPDME, WOFMSC, WOFMST, RAME " +
                                 "FROM " + pathFile + "\\Mamg0.DBF";

            OleDbConnection ExcelConection = null;
            List<Mamg0ForDBF> list = new List<Mamg0ForDBF>();

            try
            {
                OleDbConnectionStringBuilder OleStringBuilder = new OleDbConnectionStringBuilder(excelConnectionString);
                OleStringBuilder.DataSource = pathFile;
                ExcelConection = new OleDbConnection();
                ExcelConection.ConnectionString = OleStringBuilder.ConnectionString;

                using (OleDbDataAdapter adaptor = new OleDbDataAdapter(commandText, ExcelConection))
                {
                    DataSet ds = new DataSet();
                    adaptor.Fill(ds);
                    ExcelConection.Open();

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        Mamg0ForDBF mmg = new Mamg0ForDBF();
                        mmg.AA_COD = row.ItemArray[0].ToString().Trim();
                        mmg.AA_SIGF = row.ItemArray[1].ToString().Trim();
                        mmg.AA_CODF = row.ItemArray[2].ToString().Trim();
                        mmg.AA_DES = row.ItemArray[3].ToString().Trim();
                        mmg.AA_UM = row.ItemArray[4].ToString().Trim();
                        mmg.AA_PZ = Convert.ToDouble(row.ItemArray[5]);
                        mmg.AA_IVA = Convert.ToInt32(row.ItemArray[6]);
                        mmg.AA_VAL = row.ItemArray[7].ToString().Trim();
                        mmg.AA_PRZ = Convert.ToDouble(row.ItemArray[8]);
                        mmg.AA_CODFSS = row.ItemArray[9].ToString().Trim();
                        mmg.AA_GRUPPO = row.ItemArray[10].ToString().Trim();
                        mmg.AA_SCONTO1 = Convert.ToDouble(row.ItemArray[11]);
                        mmg.AA_SCONTO2 = Convert.ToDouble(row.ItemArray[12]);
                        mmg.AA_SCONTO3 = Convert.ToDouble(row.ItemArray[13]);
                        mmg.AA_CFZMIN = Convert.ToDouble(row.ItemArray[14]);
                        mmg.AA_MGZ = row.ItemArray[15].ToString().Trim();
                        mmg.AA_CUB = Convert.ToDouble(row.ItemArray[16]);
                        mmg.AA_PRZ1 = Convert.ToDouble(row.ItemArray[17]);
                        mmg.AA_DATA1 = Convert.ToDateTime(row.ItemArray[18]);
                        mmg.AA_EAN = row.ItemArray[19].ToString().Trim();
                        mmg.WOMAME = row.ItemArray[20].ToString().Trim();
                        mmg.WOFOME = row.ItemArray[21].ToString().Trim();
                        mmg.WOPDME = row.ItemArray[22].ToString().Trim();
                        mmg.WOFMSC = row.ItemArray[23].ToString().Trim();
                        mmg.WOFMST = row.ItemArray[24].ToString().Trim();
                        mmg.RAME = Convert.ToDouble(row.ItemArray[25]);

                        list.Add(mmg);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'importazione del Listino in Mamg0", ex);
            }
            finally
            {
                ExcelConection.Close();
            }
        }
        public static List<Mamg0> GetListino(string codArt1, string codArt2, string codArt3, string desc1, string desc2, string desc3)
        {
            string sql = "";
            SqlConnection cn = GetConnection();

            codArt1 = "%" + codArt1 + "%";
            codArt2 = "%" + codArt2 + "%";
            codArt3 = "%" + codArt3 + "%";
            desc1 = "%" + desc1 + "%";
            desc2 = "%" + desc2 + "%";
            desc3 = "%" + desc3 + "%";

            try
            {
                if (codArt1 == "%%" && codArt2 == "%%" && codArt3 == "%%" && desc1 == "%%" && desc2 == "%%" && desc3 == "%%")
                {
                    sql = "SELECT TOP 500 (AA_SIGF + AA_CODF) AS CodArt, AA_DES AS 'Desc', AA_UM AS UnitMis, AA_PZ AS Pezzo, AA_PRZ AS PrezzoListino, " +
                          "AA_SCONTO1 AS Sconto1, AA_SCONTO2 AS Sconto2, AA_SCONTO3 AS Sconto3, AA_PRZ1 AS PrezzoNetto " +
                          "FROM MAMG0 " +
                          "ORDER BY CodArt ASC ";
                }
                else
                {
                    sql = "SELECT (AA_SIGF + AA_CODF) AS CodArt, AA_DES AS 'Desc', AA_UM AS UnitMis, AA_PZ AS Pezzo, AA_PRZ AS PrezzoListino, " +
                          "AA_SCONTO1 AS Sconto1, AA_SCONTO2 AS Sconto2, AA_SCONTO3 AS Sconto3, AA_PRZ1 AS PrezzoNetto " +
                          "FROM MAMG0 " +
                          "WHERE (AA_SIGF + AA_CODF) LIKE @pCodArt1 AND (AA_SIGF + AA_CODF) LIKE @pCodArt2 AND (AA_SIGF + AA_CODF) LIKE @pCodArt3 " +
                          "AND AA_DES LIKE @pDescriCodArt1 AND AA_DES LIKE @pDescriCodArt2 AND AA_DES LIKE @pDescriCodArt3 " +
                          "ORDER BY CodArt ASC ";
                }

                return cn.Query<Mamg0>(sql, new { pCodArt1 = codArt1, pCodArt2 = codArt2, pCodArt3 = codArt3, pDescriCodArt1 = desc1, pDescriCodArt2 = desc2, pDescriCodArt3 = desc3 }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero del listino con i filtri", ex);
            }
            finally
            {
                CloseResouces(cn, null);
            }
        }

        // INSERT
        public static bool InserisciListino(Mamg0ForDBF mmgDbf)
        {
            SqlConnection cn = GetConnection();
            string sql = "";

            try
            {
                sql = "INSERT INTO MAMG0 (AA_COD, AA_SIGF, AA_CODF, AA_DES, AA_UM, AA_PZ, AA_IVA, AA_VAL, AA_PRZ, AA_CODFSS, AA_GRUPPO, " +
                      "AA_SCONTO1, AA_SCONTO2, AA_SCONTO3, AA_CFZMIN, AA_MGZ, AA_CUB, AA_PRZ1, AA_DATA1, AA_EAN, WOMAME, WOFOME, " +
                      "WOPDME, WOFMSC, WOFMST, RAME)" +
                      "VALUES (@AA_COD, @AA_SIGF, @AA_CODF, @AA_DES, @AA_UM, @AA_PZ, @AA_IVA, @AA_VAL, @AA_PRZ, @AA_CODFSS, @AA_GRUPPO, " +
                      "@AA_SCONTO1, @AA_SCONTO2, @AA_SCONTO3, @AA_CFZMIN, @AA_MGZ, @AA_CUB, @AA_PRZ1, @AA_DATA1, @AA_EAN, @WOMAME, @WOFOME, " +
                      "@WOPDME, @WOFMSC, @WOFMST, @RAME)";

                int rows = cn.Execute(sql);

                if (rows > 0)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'inserimento del listino", ex);
            }
            finally
            {
                CloseResouces(cn, null);
            }
        }

        // DELETE
        public static bool EliminaListino()
        {
            SqlConnection cn = GetConnection();
            string sql = "";

            try
            {
                sql = "DELETE FROM MAMG0";

                int rows = cn.Execute(sql);

                if (rows > 0)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'eliminazione del listino", ex);
            }
            finally
            {
                CloseResouces(cn, null);
            }
        }
    }
}