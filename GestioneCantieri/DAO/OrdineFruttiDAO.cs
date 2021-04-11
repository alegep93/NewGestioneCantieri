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
    public class OrdineFruttiDAO : BaseDAO
    {
        public static bool InserisciGruppo(string idCantiere, string idGruppoFrutto, string idLocale, int? idTblMatOrdFrutGroup, long? idSerie)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO TblMatOrdFrut(IdCantiere,IdGruppiFrutti,IdLocale,IdTblMatOrdFrutGroup,IdSerie)");
            sql.AppendLine("VALUES (@idCantiere,@idGruppoFrutto,@idLocale,@idTblMatOrdFrutGroup,@idSerie)");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), new { idCantiere, idGruppoFrutto, idLocale, idTblMatOrdFrutGroup, idSerie }) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la InserisciGruppo in OrdineFruttiDAO", ex);
            }
            return ret;
        }

        public static bool InserisciFruttoNonInGruppo(string idCantiere, string idLocale, string idFrutto, string qtaFrutti, int? idTblMatOrdFrutGroup, long? idSerie)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO TblMatOrdFrut(IdCantiere,IdLocale,IdFrutto,QtaFrutti,IdTblMatOrdFrutGroup,IdSerie)");
            sql.AppendLine("VALUES (@idCantiere,@idLocale,@idFrutto,@qtaFrutti,@idTblMatOrdFrutGroup,@idSerie)");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), new { idCantiere, idLocale, idFrutto, qtaFrutti, idTblMatOrdFrutGroup, idSerie }) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la InserisciFruttoNonInGruppo in OrdineFruttiDAO", ex);
            }
            return ret;
        }

        public static bool InserisciDaDefault(int idCantiere, int idLocale, int idLocaleDefault)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            try
            {
                sql.AppendLine($"INSERT INTO TblMatOrdFrut (IdCantiere, IdGruppiFrutti, IdLocale, IdFrutto, QtaFrutti, IdSerie)");
                sql.AppendLine($"SELECT {idCantiere}, IdGruppiFrutti, {idLocale}, IdFrutto, QtaFrutti, IdSerie");
                sql.AppendLine($"FROM TblDefaultMatOrdFrut");
                sql.AppendLine($"WHERE IdLocale = @idLocaleDefault");
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), new { idLocaleDefault }) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la InserisciDaDefault in OrdineFruttiDAO", ex);
            }
            return ret;
        }

        public static List<MatOrdFrut> GetByIdCantiere(int idCantiere)
        {
            List<MatOrdFrut> ret = new List<MatOrdFrut>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT DISTINCT A.IdLocale, B.NomeLocale");
            sql.AppendLine($"FROM TblMatOrdFrut AS A");
            sql.AppendLine($"INNER JOIN TblLocali AS B ON A.IdLocale = B.IdLocali");
            sql.AppendLine($"WHERE A.IdCantiere = @idCantiere");
            sql.AppendLine($"ORDER BY NomeLocale");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<MatOrdFrut>(sql.ToString(), new { idCantiere }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetGruppi in OrdineFruttiDAO", ex);
            }
            return ret;
        }

        public static List<MatOrdFrut> GetGruppi(string idCantiere, string idLocale)
        {
            List<MatOrdFrut> ret = new List<MatOrdFrut>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT GF.NomeGruppo, GF.Descrizione");
            sql.AppendLine("FROM TblMatOrdFrut AS MOF");
            sql.AppendLine("INNER JOIN TblGruppiFrutti AS GF ON MOF.IdGruppiFrutti = GF.Id");
            sql.AppendLine("WHERE IdCantiere = @idCantiere AND IdLocale = @idLocale");
            sql.AppendLine("ORDER BY NomeGruppo ASC");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<MatOrdFrut>(sql.ToString(), new { idCantiere, idLocale }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetGruppi in OrdineFruttiDAO", ex);
            }
            return ret;
        }

        public static List<MatOrdFrut> GetFruttiNonInGruppo(string idCant, string idLocale)
        {
            List<MatOrdFrut> ret = new List<MatOrdFrut>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT F.descr001, SUM(MOF.QtaFrutti)");
            sql.AppendLine("FROM TblMatOrdFrut AS MOF");
            sql.AppendLine("LEFT JOIN TblFrutti AS F ON(MOF.IdFrutto = F.ID1)");
            sql.AppendLine("WHERE IdCantiere = @idCant AND IdLocale = @idLocale AND MOF.idFrutto IS NOT NULL AND MOF.QtaFrutti IS NOT NULL");
            sql.AppendLine("GROUP BY F.descr001");
            sql.AppendLine("ORDER BY F.descr001 ASC");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<MatOrdFrut>(sql.ToString(), new { idCant, idLocale }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetFruttiNonInGruppo in OrdineFruttiDAO", ex);
            }
            return ret;
        }

        public static bool DeleteGruppo(int idGruppo)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder("DELETE FROM TblMatOrdFrut WHERE IdGruppiFrutti = @idGruppo");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql.ToString(), new { idGruppo });
                }
                ret = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la DeleteGruppo in OrdineFruttiDAO", ex);
            }
            return ret;
        }

        public static bool Delete(int id)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder("DELETE FROM TblMatOrdFrut WHERE Id = @id");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql.ToString(), new { id });
                }
                ret = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Delete in OrdineFruttiDAO", ex);
            }
            return ret;
        }

        public static bool DeleteOrdine(int idCantiere)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder("DELETE FROM TblMatOrdFrut WHERE IdCantiere = @idCantiere");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql.ToString(), new { idCantiere });
                }
                ret = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la DeleteOrdine in OrdineFruttiDAO", ex);
            }
            return ret;
        }

        public static List<StampaOrdFrutCantLoc> GetAllGruppiInLocale(string idCant)
        {
            List<StampaOrdFrutCantLoc> ret = new List<StampaOrdFrutCantLoc>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT L.NomeLocale, GF.NomeGruppo, COUNT(Gf.NomeGruppo) AS 'Qta', MIN(MOFG.Descrizione) AS DescrizioneGruppoOrdine");
            sql.AppendLine("FROM TblMatOrdFrut AS MOF");
            sql.AppendLine("JOIN TblLocali AS L ON(MOF.IdLocale = L.IdLocali)");
            sql.AppendLine("JOIN TblGruppiFrutti AS GF ON(MOF.IdGruppiFrutti = GF.Id)");
            sql.AppendLine("LEFT JOIN TblMatOrdFrutGroup AS MOFG ON MOF.IdTblMatOrdFrutGroup = MOFG.IdMatOrdFrutGroup");
            sql.AppendLine("WHERE IdCantiere = @idCant");
            sql.AppendLine("GROUP BY L.NomeLocale, GF.NomeGruppo");
            sql.AppendLine("ORDER BY NomeLocale ASC");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<StampaOrdFrutCantLoc>(sql.ToString(), new { idCant }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetAllGruppiInLocale in OrdineFruttiDAO", ex);
            }
            return ret;
        }

        public static List<StampaOrdFrutCantLoc> GetAllFruttiInLocale(string idCant)
        {
            List<StampaOrdFrutCantLoc> ret = new List<StampaOrdFrutCantLoc>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT F.descr001 AS 'Descr001', SUM(CGF.Qta) AS Qta");
            sql.AppendLine("FROM TblMatOrdFrut AS MOF");
            sql.AppendLine("JOIN TblLocali AS L ON(MOF.IdLocale = L.IdLocali)");
            sql.AppendLine("JOIN TblGruppiFrutti AS GF ON(MOF.IdGruppiFrutti = GF.Id)");
            sql.AppendLine("JOIN TblCompGruppoFrut AS CGF ON(CGF.IdTblGruppo = GF.Id)");
            sql.AppendLine("JOIN TblFrutti AS F ON(CGF.IdTblFrutto = F.ID1)");
            sql.AppendLine("WHERE IdCantiere = @idCant");
            sql.AppendLine("GROUP BY F.descr001");
            sql.AppendLine("ORDER BY F.descr001 ASC");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<StampaOrdFrutCantLoc>(sql.ToString(), new { idCant }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetAllFruttiInLocale in OrdineFruttiDAO", ex);
            }
            return ret;
        }

        public static DataTable GetAllFruttiInLocaleDataTable(string idCant)
        {
            DataTable table = new DataTable();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT F.descr001, SUM(CGF.Qta) AS Qta");
            sql.AppendLine("FROM TblMatOrdFrut AS MOF");
            sql.AppendLine("JOIN TblLocali AS L ON(MOF.IdLocale = L.IdLocali)");
            sql.AppendLine("JOIN TblGruppiFrutti AS GF ON(MOF.IdGruppiFrutti = GF.Id)");
            sql.AppendLine("JOIN TblCompGruppoFrut AS CGF ON(CGF.IdTblGruppo = GF.Id)");
            sql.AppendLine("JOIN TblFrutti AS F ON(CGF.IdTblFrutto = F.ID1)");
            sql.AppendLine("WHERE IdCantiere = @idCant");
            sql.AppendLine("GROUP BY F.descr001");
            sql.AppendLine("ORDER BY F.descr001 ASC");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(sql.ToString(), cn))
                    {
                        cmd.Parameters.Add(new SqlParameter("idCant", idCant));
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {

                            table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                            adapter.Fill(table);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetAllFruttiInLocaleDataTable in OrdineFruttiDAO", ex);
            }
            return table;
        }

        public static List<StampaOrdFrutCantLoc> GetAllFruttiNonInGruppo(string idCant)
        {
            List<StampaOrdFrutCantLoc> ret = new List<StampaOrdFrutCantLoc>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT F.descr001 AS 'Descr001', SUM(MOF.QtaFrutti) AS Qta");
            sql.AppendLine("FROM TblMatOrdFrut AS MOF");
            sql.AppendLine("LEFT JOIN TblFrutti AS F ON(MOF.IdFrutto = F.ID1)");
            sql.AppendLine("where IdCantiere = @idCant AND MOF.idFrutto IS NOT NULL AND MOF.QtaFrutti IS NOT NULL");
            sql.AppendLine("GROUP BY F.descr001");
            sql.AppendLine("ORDER BY F.descr001 ASC");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<StampaOrdFrutCantLoc>(sql.ToString(), new { idCant }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetAllFruttiNonInGruppo in OrdineFruttiDAO", ex);
            }
            return ret;
        }

        public static List<StampaOrdFrutCantLoc> GetFruttiPerStampaExcel(string idCant, string idLocaliList)
        {
            List<StampaOrdFrutCantLoc> ret = new List<StampaOrdFrutCantLoc>();
            StringBuilder sql = new StringBuilder();
            try
            {
                sql.AppendLine($"SELECT descr001, SUM(Qta) Qta, NomeSerie, ArticoloSerie, DescrizioneSerie, MIN(PrezzoNetto) PrezzoNetto, SUM(Valore) Valore FROM (");
                sql.AppendLine($"   SELECT F.descr001, SUM(CGF.Qta) As Qta, S.NomeSerie, (M.AA_SIGF + M.AA_CODF) AS ArticoloSerie, AA_DES AS DescrizioneSerie, AA_PRZ1 AS PrezzoNetto, SUM(CGF.Qta * AA_PRZ1) AS Valore");
                sql.AppendLine($"   FROM TblMatOrdFrut AS MOF");
                sql.AppendLine($"   INNER JOIN TblLocali AS L ON MOF.IdLocale = L.IdLocali");
                sql.AppendLine($"   INNER JOIN TblGruppiFrutti AS GF ON MOF.IdGruppiFrutti = GF.Id");
                sql.AppendLine($"   INNER JOIN TblCompGruppoFrut AS CGF ON CGF.IdTblGruppo = GF.Id");
                sql.AppendLine($"   INNER JOIN TblFrutti AS F ON CGF.IdTblFrutto = F.ID1");
                sql.AppendLine($"   LEFT JOIN TblSerie S ON MOF.IdSerie = S.IdSerie");
                sql.AppendLine($"   LEFT JOIN TblFruttiSerie FS ON CGF.IdTblFrutto = FS.IdFrutto AND S.IdSerie = FS.IdSerie");
                sql.AppendLine($"   LEFT JOIN MAMG0 M ON FS.CodiceListinoUnivoco = M.CodiceListinoUnivoco");
                sql.AppendLine($"   WHERE MOF.IdCantiere = @idCant {(idLocaliList != "" ? $"AND MOF.IdLocale IN ({idLocaliList})" : "")}");
                sql.AppendLine($"   GROUP BY F.descr001, S.NomeSerie, (M.AA_SIGF + M.AA_CODF), AA_DES, AA_PRZ1");
                sql.AppendLine($"   UNION");
                sql.AppendLine($"   SELECT F.descr001, SUM(MOF.QtaFrutti) As Qta, S.NomeSerie, (M.AA_SIGF + M.AA_CODF) AS ArticoloSerie, AA_DES AS DescrizioneSerie, AA_PRZ1 AS PrezzoNetto, SUM(MOF.QtaFrutti * AA_PRZ1) AS Valore");
                sql.AppendLine($"   FROM TblMatOrdFrut AS MOF");
                sql.AppendLine($"   LEFT JOIN TblFrutti AS F ON MOF.IdFrutto = F.ID1");
                sql.AppendLine($"   LEFT JOIN TblFruttiSerie FS ON F.ID1 = FS.IdFrutto AND MOF.IdSerie = FS.IdSerie");
                sql.AppendLine($"   LEFT JOIN TblSerie S ON FS.IdSerie = S.IdSerie");
                sql.AppendLine($"   LEFT JOIN MAMG0 M ON FS.CodiceListinoUnivoco = M.CodiceListinoUnivoco");
                sql.AppendLine($"   WHERE IdCantiere = @idCant AND MOF.idFrutto IS NOT NULL AND MOF.QtaFrutti IS NOT NULL {(idLocaliList != "" ? $"AND MOF.IdLocale IN ({idLocaliList})" : "")}");
                sql.AppendLine($"   GROUP BY F.descr001, S.NomeSerie, (M.AA_SIGF + M.AA_CODF), AA_DES, AA_PRZ1");
                sql.AppendLine($") AS A");
                sql.AppendLine($"GROUP BY descr001,NomeSerie,ArticoloSerie,DescrizioneSerie");
                sql.AppendLine($"ORDER BY ArticoloSerie, DescrizioneSerie");
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<StampaOrdFrutCantLoc>(sql.ToString(), new { idCant, idLocaliList }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetFruttiPerStampaExcel in OrdineFruttiDAO", ex);
            }
            return ret;
        }

        public static List<MatOrdFrut> GetInfoForCantiereAndLocale(string idCant, string idLocale)
        {
            List<MatOrdFrut> ret = new List<MatOrdFrut>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT A.id, B.DescriCodCAnt 'DescrCant', C.NomeLocale 'Appartamento', D.NomeGruppo,");
            sql.AppendLine($"       E.descr001 'NomeFrutto', A.QtaFrutti, F.Descrizione AS DescrizioneGruppoOrdine, G.NomeSerie");
            sql.AppendLine($"FROM TblMatOrdFrut AS A");
            sql.AppendLine($"LEFT JOIN TblCantieri AS B ON A.IdCantiere = B.IdCantieri");
            sql.AppendLine($"LEFT JOIN TblLocali AS C ON A.IdLocale = C.IdLocali");
            sql.AppendLine($"LEFT JOIN TblGruppiFrutti AS D ON A.IdGruppiFrutti = D.Id");
            sql.AppendLine($"LEFT JOIN TblFrutti AS E ON A.IdFrutto = E.ID1");
            sql.AppendLine($"LEFT JOIN TblMatOrdFrutGroup AS F ON A.IdTblMatOrdFrutGroup = F.IdMatOrdFrutGroup");
            sql.AppendLine($"LEFT JOIN TblSerie G ON A.IdSerie = G.IdSerie");
            sql.AppendLine($"WHERE B.IdCantieri = @idCant AND C.IdLocali = @idLocale");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<MatOrdFrut>(sql.ToString(), new { idCant, idLocale }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetInfoForCantiereAndLocale in OrdineFruttiDAO", ex);
            }
            return ret;
        }
    }
}