using Dapper;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GestioneCantieri.DAO
{
    public class DDTMefDAO : BaseDAO
    {
        public static List<DDTMef> GetAll()
        {
            List<DDTMef> ret = new List<DDTMef>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT TOP 500 IdDDTMef, Anno, Data, N_DDT, CodArt,");
            sql.AppendLine($"DescriCodArt, Qta, Importo, Acquirente, PrezzoUnitario, AnnoN_DDT");
            sql.AppendLine($"FROM TblDDTMef");
            sql.AppendLine($"ORDER BY Anno, Data, N_DDT, CodArt");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<DDTMef>(sql.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero dell'elenco dei DDT", ex);
            }
            return ret;
        }

        public static List<DDTMef> GetDdt(DDTMefObject ddt)
        {
            List<DDTMef> ret = new List<DDTMef>();
            StringBuilder sql = new StringBuilder();
            StringBuilder queryFilters = new StringBuilder();

            // Filtri Generici
            queryFilters.AppendLine($"Qta LIKE '%{ddt.Qta}%' AND N_DDT LIKE '%{ddt.NDdt}%'");
            queryFilters.AppendLine($"AND CodArt LIKE '%{ddt.CodArt1}%' AND CodArt LIKE '%{ddt.CodArt2}%' AND CodArt LIKE '%{ddt.CodArt3}%'");
            queryFilters.AppendLine($"AND DescriCodArt LIKE '%{ddt.DescriCodArt1}%' AND DescriCodArt LIKE '%{ddt.DescriCodArt2}%' AND DescriCodArt LIKE '%{ddt.DescriCodArt3}%'");

            sql.AppendLine($"SELECT IdDDTMef, Anno, Data, N_DDT, CodArt,");
            sql.AppendLine($"DescriCodArt, Qta, Importo, Acquirente, PrezzoUnitario, AnnoN_DDT");
            sql.AppendLine($"FROM TblDDTMef");

            // Controllo i casi in cui entrambi gli anni o le date siano state valorizzate
            // Oppure quanto tutti quanti sono vuoti
            // Altrimenti faccio una where generica per tutti gli altri casi
            if (ddt.AnnoInizio != "" && ddt.AnnoFine != "")
            {
                sql.AppendLine($"WHERE (ANNO BETWEEN @AnnoInizio AND @AnnoFine) AND {queryFilters}");
            }
            else if (ddt.DataInizio != "" && ddt.DataFine != "")
            {
                sql.AppendLine($"WHERE (Data BETWEEN CONVERT(Date,@DataInizio) AND CONVERT(Date,@DataFine)) AND {queryFilters}");
            }
            else if (ddt.AnnoInizio == "" && ddt.AnnoFine == "" && ddt.DataInizio == "" && ddt.DataFine == "")
            {
                sql.AppendLine($"WHERE {queryFilters}");
            }
            else
            {
                sql.AppendLine($"WHERE ((ANNO = @AnnoInizio OR Anno = @AnnoFine) OR (Data = CONVERT(Date, @DataInizio) OR Data = CONVERT(Date, @DataFine))) AND {queryFilters}");
            }
            sql.AppendLine($"ORDER BY Anno, Data, N_DDT, CodArt");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<DDTMef>(sql.ToString(), new { ddt.AnnoInizio, ddt.AnnoFine, ddt.DataInizio, ddt.DataFine }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la ricerca dei DDT Mef", ex);
            }
            return ret;
        }

        public static List<DDTMef> GetNewDDT()
        {
            List<DDTMef> ret = new List<DDTMef>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT A.*");
            sql.AppendLine($"FROM TblDDTMefTemp AS A");
            sql.AppendLine($"LEFT JOIN TblDDTMef AS B ON A.AnnoN_DDT = B.AnnoN_DDT");
            sql.AppendLine($"WHERE B.IdDDTMef IS NULL");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<DDTMef>(sql.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero dei nuovi DDT da aggiungere all'anagrafica", ex);
            }
            return ret;
        }

        public static List<DDTMef> GetByAnnoNumeroDdt(string anno, string nDdt)
        {
            List<DDTMef> ret = new List<DDTMef>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT Data, N_DDT");
            sql.AppendLine($"FROM TblDDTMef");
            sql.AppendLine($"WHERE Anno LIKE '%{anno}%' AND N_DDT LIKE '%{nDdt}%'");
            sql.AppendLine($"GROUP BY N_DDT, Data");
            sql.AppendLine($"ORDER BY Data, N_DDT");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<DDTMef>(sql.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero dei DDT Mef", ex);
            }
            return ret;
        }

        public static List<DDTMef> GetDDTForPDF(string dataInizio, string dataFine, string acquirente, string nDdt)
        {
            List<DDTMef> ret = new List<DDTMef>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT IdDDTMef, Anno, Data, N_DDT, CodArt,");
            sql.AppendLine($"DescriCodArt, Qta, Importo, Acquirente, PrezzoUnitario, AnnoN_DDT");
            sql.AppendLine($"FROM TblDDTMef");
            sql.AppendLine($"WHERE (Data BETWEEN Convert(date,@pDataInizio) AND Convert(date,@pDataFine)) AND Acquirente LIKE '%{acquirente}%' AND N_DDT LIKE '%{nDdt}%'");
            sql.AppendLine($"ORDER BY Data, N_DDT, CodArt");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<DDTMef>(sql.ToString(), new { dataInizio, dataFine }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero dei DDT Mef per la stampa in PDF", ex);
            }
            return ret;
        }

        public static List<DDTMef> GetDdtFromDBF(string pathFile, string acquirente, int idFornitore)
        {
            List<DDTMef> ret = new List<DDTMef>();
            string excelConnectionString = $"Provider = vfpoledb; Data Source = {pathFile}; Collating Sequence = machine";
            string commandText = $"SELECT FTANNO, FTDT, FTNR, FTVRF0, FTDT30, FTAFO, FTAIN, FTDEX1, FTDEX2, FTAIV, FTPUN, " +
                                 $"FTQTA, FTPU, FTDTC, FTCVA, FTFOM, FTCMA, FTCDO, FLFLAG, FLFLQU, FTDTAG, FTORAG, FTTSCA, FTIMRA, FTMLT0 " +
                                 $"FROM {pathFile}";
            try
            {
                OleDbConnectionStringBuilder OleStringBuilder = new OleDbConnectionStringBuilder(excelConnectionString)
                {
                    DataSource = pathFile
                };
                OleDbConnection ExcelConection = new OleDbConnection
                {
                    ConnectionString = OleStringBuilder.ConnectionString
                };

                using (OleDbDataAdapter adaptor = new OleDbDataAdapter(commandText, ExcelConection))
                {
                    DataSet ds = new DataSet();
                    adaptor.Fill(ds);
                    ExcelConection.Open();

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (Convert.ToInt32(row.ItemArray[11]) != 0)
                        {
                            DateTime date = Convert.ToDateTime(row.ItemArray[1].ToString().Substring(0, 4) + "-" + row.ItemArray[1].ToString().Substring(4, 2) + "-" + row.ItemArray[1].ToString().Substring(6, 2));
                            DateTime date2 = (row.ItemArray[13].ToString() == "0,000" || row.ItemArray[13].ToString() == "99999999,000") ? DateTime.Now : Convert.ToDateTime(row.ItemArray[13].ToString().Substring(0, 4) + " -" + row.ItemArray[13].ToString().Substring(4, 2) + "-" + row.ItemArray[13].ToString().Substring(6, 2));
                            DateTime date3 = Convert.ToDateTime(row.ItemArray[20].ToString().Substring(0, 4) + "-" + row.ItemArray[20].ToString().Substring(4, 2) + "-" + row.ItemArray[20].ToString().Substring(6, 2));

                            decimal prezzoUnitario = Convert.ToDecimal(row.ItemArray[12]) / Convert.ToInt32(row.ItemArray[11].ToString() == "0" ? 1 : row.ItemArray[11]);
                            int annoN_ddt = Convert.ToInt32(row.ItemArray[0].ToString() + row.ItemArray[2].ToString());

                            ret.Add(new DDTMef
                            {
                                Anno = Convert.ToInt32(row.ItemArray[0]), // FTANNO
                                Data = date, // FTDT
                                N_DDT = Convert.ToInt32(row.ItemArray[2]), // FTNR
                                FTVRF0 = row.ItemArray[3].ToString().Trim(),
                                FTDT30 = row.ItemArray[4].ToString().Trim(),
                                CodArt = row.ItemArray[5].ToString().Trim(), // FTAFO
                                FTAIN = row.ItemArray[6].ToString().Trim(),
                                DescriCodArt = row.ItemArray[7].ToString().Trim(),  // FTDEX1
                                DescrizioneArticolo2 = row.ItemArray[8].ToString().Trim(), // FTDEX2
                                Iva = (row.ItemArray[9].ToString() == "" || row.ItemArray[9].ToString().Trim() == "D") ? 0 : Convert.ToInt32(row.ItemArray[9]), // FTAIV
                                PrezzoListino = Convert.ToDecimal(row.ItemArray[10]), // FTPUN
                                Qta = Convert.ToInt32(row.ItemArray[11]), // FTQTA
                                Importo = Convert.ToDecimal(row.ItemArray[12]), // FTPU
                                Data2 = date2, // FTDTC
                                Valuta = row.ItemArray[14].ToString().Trim(), // FTCVA
                                FTFOM = row.ItemArray[15].ToString().Trim(),
                                FTCMA = row.ItemArray[16].ToString().Trim(),
                                FTCDO = row.ItemArray[17].ToString().Trim(),
                                FLFLAG = row.ItemArray[18].ToString().Trim(),
                                FLFLQU = row.ItemArray[19].ToString().Trim(),
                                Data3 = date3, // FTDTAG
                                FTORAG = row.ItemArray[21].ToString().Trim(),
                                Importo2 = Convert.ToDecimal(row.ItemArray[22]), // FTTSCA
                                FTIMRA = row.ItemArray[23].ToString().Trim(),
                                FTMLT0 = row.ItemArray[24].ToString().Trim(),
                                Acquirente = acquirente,
                                PrezzoUnitario = prezzoUnitario,
                                AnnoN_ddt = annoN_ddt,
                                IdFornitore = idFornitore
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'importazione del DBF per il DDT MEF", ex);
            }
            return ret;
        }

        public static bool CheckIfDdtExistBetweenData(string nDdt, string dataInizio, string dataFine)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder("SELECT N_DDT FROM TblDDTMef WHERE N_DDT = @nDdt AND Data BETWEEN CONVERT(date, @dataInizio) AND CONVERT(date, @dataFine)");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<bool>(sql.ToString(), new { nDdt, dataInizio, dataFine }).ToList().Count > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il controllo della presenza di un N_DDT del DDTMef", ex);
            }
            return ret;
        }

        public static bool InsertNewDdt(DDTMef ddt)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"INSERT INTO TblDDTMef (Anno,Data,N_DDT,CodArt,DescriCodArt,Qta,Importo,Acquirente,PrezzoUnitario,AnnoN_DDT,");
            sql.AppendLine($"FTVRF0,FTDT30,FTAIN,descrizione_articolo_2,Iva,prezzo_listino,Data2,Valuta,FTFOM,FTCMA,FTCDO,FLFLAG,FLFLQU,Data3,FTORAG,Importo2,FTIMRA,FTMLT0)");
            sql.AppendLine($"VALUES (@Anno,@Data,@N_DDT,@CodArt,@DescriCodArt,@Qta,@Importo,@Acquirente,@PrezzoUnitario,@AnnoN_DDT,");
            sql.AppendLine($"@FTVRF0,@FTDT30,@FTAIN,@DescrizioneArticolo2,@Iva,@PrezzoListino,@Data2,@Valuta,@FTFOM,@FTCMA,@FTCDO,");
            sql.AppendLine($"@FLFLAG,@FLFLQU,@Data3,@FTORAG,@Importo2,@FTIMRA,@FTMLT0)");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), ddt) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'inserimento di un nuovo record per il DDTMef ", ex);
            }
            return ret;
        }

        public static bool UpdateDdt()
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"UPDATE A");
            sql.AppendLine($"SET A.Importo = B.Importo, A.PrezzoUnitario = B.PrezzoUnitario, A.prezzo_listino = B.prezzo_listino");
            sql.AppendLine($"FROM TblDDTMef AS A");
            sql.AppendLine($"INNER JOIN TblDDTMefTemp AS B ON A.Anno = B.Anno AND A.N_DDT = B.N_DDT AND A.CodArt = B.CodArt");
            sql.AppendLine($"WHERE A.Qta = B.Qta AND A.Importo != B.Importo");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString()) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'aggiornamento di un record del DDTMef", ex);
            }
            return ret;
        }

        public static bool InsertIntoDdtTemp(DDTMef ddt)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"INSERT INTO TblDDTMefTemp (Anno,Data,N_DDT,CodArt,DescriCodArt,Qta,Importo,Acquirente,PrezzoUnitario,AnnoN_DDT,");
            sql.AppendLine($"FTVRF0,FTDT30,FTAIN,descrizione_articolo_2,Iva,prezzo_listino,Data2,Valuta,FTFOM,FTCMA,FTCDO,FLFLAG,FLFLQU,Data3,FTORAG,Importo2,FTIMRA,FTMLT0)");
            sql.AppendLine($"VALUES (@Anno,@Data,@N_DDT,@CodArt,@DescriCodArt,@Qta,@Importo,@Acquirente,@PrezzoUnitario,@AnnoN_DDT,");
            sql.AppendLine($"@FTVRF0,@FTDT30,@FTAIN,@DescrizioneArticolo2,@Iva,@PrezzoListino,@Data2,@Valuta,@FTFOM,@FTCMA,@FTCDO,");
            sql.AppendLine($"@FLFLAG,@FLFLQU,@Data3,@FTORAG,@Importo2,@FTIMRA,@FTMLT0)");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), ddt) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'inserimento di un nuovo record nella tabella TblDDTMefTemp ", ex);
            }
            return ret;
        }

        public static bool DeleteFromDdtTemp()
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder("DELETE FROM TblDDTMefTemp");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString()) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'eliminazione della tabella TblDDTMefTemp ", ex);
            }
            return ret;
        }
    }
}