using Dapper;
using Database.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Database.DAO
{
    public class MaterialiCantieriDAO : BaseDAO
    {
        //SELECT
        public static List<MaterialiCantieri> GetByIdCantiere(long idCantiere)
        {
            List<MaterialiCantieri> ret = new List<MaterialiCantieri>();
            StringBuilder sql = new StringBuilder($"SELECT * FROM TblMaterialiCantieri WHERE IdTblCantieri = @idCantiere");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<MaterialiCantieri>(sql.ToString(), new { idCantiere }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetAll in MaterialiCantieriDAO", ex);
            }
            return ret;
        }

        public static List<MaterialiCantieri> GetByListOfCantieri(string idCantieri)
        {
            List<MaterialiCantieri> ret = new List<MaterialiCantieri>();
            StringBuilder sql = new StringBuilder($"SELECT *, (Qta * PzzoUniCantiere) AS Valore FROM TblMaterialiCantieri WHERE IdTblCantieri IN ({idCantieri})");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<MaterialiCantieri>(sql.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetByListOfCantieri in MaterialiCantieriDAO", ex);
            }
            return ret;
        }

        public static List<MaterialiCantieri> GetMaterialeCantiere(string idCantiere, string codiceArticolo = "", string descrizioneCodiceArticolo = "")
        {
            List<MaterialiCantieri> ret = new List<MaterialiCantieri>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT *, (Qta * PzzoUniCantiere) AS Valore");
            sql.AppendLine($"FROM TblMaterialiCantieri");
            sql.AppendLine($"WHERE IdTblCantieri = @idCantiere AND CodArt LIKE '%{codiceArticolo}%' AND DescriCodArt LIKE '%{descrizioneCodiceArticolo}%'");

            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<MaterialiCantieri>(sql.ToString(), new { idCantiere }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero dei materiali di cantiere", ex);
            }
            return ret;
        }

        public static List<MaterialiCantieri> GetMaterialeCantiereForGridView(string idCantiere, string codArt, string descriCodArt, string protocollo, string fornitore, string tipologia)
        {
            List<MaterialiCantieri> ret = new List<MaterialiCantieri>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT {(codArt != "%%" || descriCodArt != "%%" ? "" : "TOP 500")} A.*");
            sql.AppendLine($"FROM TblMaterialiCantieri AS A");
            sql.AppendLine($"LEFT JOIN TblCantieri AS B ON(A.IdTblCantieri = b.IdCantieri)");
            sql.AppendLine($"LEFT JOIN TblOperaio AS C ON(A.Acquirente = C.IdOperaio)");
            sql.AppendLine($"LEFT JOIN TblForitori AS D ON(A.Fornitore = D.IdFornitori)");
            sql.AppendLine($"WHERE A.IdTblCantieri = @idCantiere AND ISNULL(A.CodArt,'') LIKE '%{codArt}%'");
            sql.AppendLine($"AND ISNULL(A.DescriCodArt,'') LIKE '%{descriCodArt}%' AND ISNULL(A.ProtocolloInterno,'') LIKE '%{protocollo}%'");
            sql.AppendLine($"AND ISNULL(D.RagSocForni,'') LIKE '%{fornitore}%' AND Tipologia = @tipologia");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<MaterialiCantieri>(sql.ToString(), new { idCantiere, tipologia }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero dei materiali di cantiere", ex);
            }
            return ret;
        }
        public static List<MaterialiCantieri> GetMaterialeCantiere(string dataInizio, string dataFine, string acquirente, string fornitore, string nDdt)
        {
            List<MaterialiCantieri> ret = new List<MaterialiCantieri>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT A.*, B.RagSocForni AS Fornitore, C.NomeOp AS Acquirente, D.CodCant");
            sql.AppendLine($"FROM TblMaterialiCantieri AS A");
            sql.AppendLine($"LEFT JOIN TblForitori AS B ON A.Fornitore = B.IdFornitori");
            sql.AppendLine($"LEFT JOIN TblOperaio AS C ON A.Acquirente = C.IdOperaio");
            sql.AppendLine($"LEFT JOIN TblCantieri AS D ON A.IdTblCantieri = D.IdCantieri");
            sql.AppendLine($"WHERE A.Data BETWEEN Convert(date, @dataInizio) AND Convert(date, @dataFine) AND C.NomeOp LIKE '%{acquirente.Split('-')[0].Trim()}%'");
            sql.AppendLine($"AND B.RagSocForni LIKE '%{fornitore}%' AND NumeroBolla LIKE '%{nDdt}%'");
            sql.AppendLine($"ORDER BY A.Data, A.NumeroBolla");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<MaterialiCantieri>(sql.ToString(), new { dataInizio, dataFine }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero dei materiali di cantiere per singolo cantiere", ex);
            }
            return ret;
        }

        public static List<MaterialiCantieri> GetMatCantPerResocontoOperaio(string dataInizio, string dataFine, string idOperaio, string codCant = "", int viewStatus = 0)
        {
            List<MaterialiCantieri> ret = new List<MaterialiCantieri>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT A.Data, A.Qta, A.PzzoUniCantiere, B.CodCant, B.DescriCodCAnt, C.NomeOp AS Acquirente, (A.Qta * A.PzzoUniCantiere) AS Valore, A.OperaioPagato");
            sql.AppendLine($"FROM TblMaterialiCantieri AS A");
            sql.AppendLine($"LEFT JOIN TblCantieri AS B ON A.IdTblCantieri = B.IdCantieri");
            sql.AppendLine($"LEFT JOIN TblOperaio AS C ON A.IdTblOperaio = C.IdOperaio");
            sql.AppendLine($"WHERE (A.Data BETWEEN Convert(date, @dataInizio) AND Convert(date, @dataFine))");
            sql.AppendLine($"AND B.CodCant LIKE '%{codCant}%' AND A.idTblOperaio IS NOT NULL");
            sql.AppendLine(viewStatus != 2 ? $"AND (A.OperaioPagato = @viewStatus {(viewStatus == 0 ? "OR A.OperaioPagato IS NULL" : "")})" : "");
            sql.AppendLine(idOperaio != "-1" ? $"AND A.IdTblOperaio = @idOperaio" : "");
            sql.AppendLine($"UNION");
            sql.AppendLine($"SELECT A.Data, 0 AS Qta, 0 AS PzzoUniCantiere, 'Acconto' AS CodCant, 'Acconto' AS DescriCodCAnt, B.NomeOp AS Acquirente, (A.Importo * -1) AS Valore, A.Pagato");
            sql.AppendLine($"FROM TblAccontiOperai A");
            sql.AppendLine($"LEFT JOIN TblOperaio AS B ON A.IdOperaio = B.IdOperaio");
            sql.AppendLine($"WHERE(A.Data BETWEEN Convert(date, @dataInizio) AND Convert(date, @dataFine)) AND A.IdOperaio = @idOperaio");
            sql.AppendLine(viewStatus != 2 ? $"AND A.Pagato = @viewStatus" : "");
            sql.AppendLine($"ORDER BY A.Data, B.CodCant");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<MaterialiCantieri>(sql.ToString(), new { dataInizio, dataFine, idOperaio, viewStatus }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero dei dati per il resoconto operaio", ex);
            }
            return ret;
        }

        public static List<MaterialiCantieri> GetMaterialeCantierePerTipologia(int idCant, string dataDa, string dataA, string tipologia)
        {
            List<MaterialiCantieri> ret = new List<MaterialiCantieri>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT A.*, B.DescriCodCAnt, B.CodCant, C.RagSocCli, D.NomeOp Acquirente");
            sql.AppendLine($"FROM TblMaterialiCantieri AS A");
            sql.AppendLine($"LEFT JOIN TblCantieri AS B ON (A.IdTblCantieri = B.IdCantieri)");
            sql.AppendLine($"LEFT JOIN TblClienti AS C ON (B.IdTblClienti = C.IdCliente)");
            sql.AppendLine($"LEFT JOIN TblOperaio AS D ON (A.IdTblOperaio = D.IdOperaio)");
            sql.AppendLine($"WHERE Tipologia = @tipologia AND A.Data BETWEEN CONVERT(date, @dataDa) AND CONVERT(date, @dataA)");
            sql.AppendLine(idCant != -1 ? $"AND IdTblCantieri = @idCant" : "");
            sql.AppendLine($"ORDER BY A.Data, B.CodCant");

            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<MaterialiCantieri>(sql.ToString(), new { tipologia, idCant, dataDa, dataA }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero dei materiali di cantiere filtrati per tipologia", ex);
            }
            return ret;
        }

        public static List<MaterialiCantieri> GetMaterialeCantiereForRicalcoloConti(int idCantiere, decimal percentuale)
        {
            List<MaterialiCantieri> ret = new List<MaterialiCantieri>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT A.IdMaterialiCantiere, A.IdTblCantieri, A.IdTblOperaio, A.DescriMateriali, A.Qta, A.Visibile, A.Ricalcolo, A.ricaricoSiNo, A.Data, ");
            sql.AppendLine($"A.PzzoUniCantiere, A.Rientro, A.CodArt, A.DescriCodArt, A.Tipologia, A.UnitàDiMisura, A.ZOldNumeroBolla, A.Mate, A.Fascia, A.pzzoTemp, A.Acquirente, A.Fornitore, A.NumeroBolla,");
            sql.AppendLine($"A.ProtocolloInterno, A.Note, A.Note2, A.OperaioPagato,");
            sql.AppendLine($"(CASE WHEN ISNULL(A.PzzoFinCli, 0) = 0");
            sql.AppendLine($"      THEN(A.PzzoUniCantiere + (CASE WHEN A.Visibile = 1 AND A.Ricalcolo = 1 THEN ISNULL(B.ValoreRicalcolo, 0) ELSE 0 END) + (CASE WHEN A.Visibile = 1 AND A.ricaricoSiNo = 1 THEN ISNULL(C.ValoreRicarico, 0) ELSE 0 END))");
            sql.AppendLine($"      ELSE ISNULL(A.PzzoFinCli, 0)");
            sql.AppendLine($"END) AS PzzoFinCli,");
            sql.AppendLine($"(A.Qta * (CASE WHEN ISNULL(A.PzzoFinCli, 0) = 0");
            sql.AppendLine($"THEN(A.PzzoUniCantiere + (CASE WHEN A.Visibile = 1 AND A.Ricalcolo = 1 THEN ISNULL(B.ValoreRicalcolo, 0) ELSE 0 END) + (CASE WHEN A.Visibile = 1 AND A.ricaricoSiNo = 1 THEN ISNULL(C.ValoreRicarico, 0) ELSE 0 END))");
            sql.AppendLine($"ELSE ISNULL(A.PzzoFinCli, 0)");
            sql.AppendLine($"END)) AS Valore");
            sql.AppendLine($"FROM (");
            sql.AppendLine($"  SELECT *");
            sql.AppendLine($"  FROM TblMaterialiCantieri");
            sql.AppendLine($"  WHERE Visibile = 1");
            sql.AppendLine($") AS A");
            sql.AppendLine($"LEFT JOIN (");
            sql.AppendLine($"  SELECT IdMaterialiCantiere, IdTblCantieri, ((PzzoUniCantiere * @percentuale) / 100) AS 'ValoreRicalcolo'");
            sql.AppendLine($"  FROM TblMaterialiCantieri");
            sql.AppendLine($"  WHERE Visibile = 1 AND Ricalcolo = 1");
            sql.AppendLine($") AS B ON A.IdMaterialiCantiere = B.IdMaterialiCantiere AND A.IdTblCantieri = B.IdTblCantieri");
            sql.AppendLine($"LEFT JOIN (");
            sql.AppendLine($"  SELECT A.IdMaterialiCantiere, A.IdTblCantieri, (((A.PzzoUniCantiere * B.Ricarico) / 100)) AS 'ValoreRicarico'");
            sql.AppendLine($"  FROM TblMaterialiCantieri AS A");
            sql.AppendLine($"  LEFT JOIN TblCantieri AS B ON (A.IdTblCantieri = B.IdCantieri)");
            sql.AppendLine($"  WHERE Visibile = 1 AND ricaricoSiNo = 1");
            sql.AppendLine($") AS C ON A.IdMaterialiCantiere = C.IdMaterialiCantiere AND A.IdTblCantieri = C.IdTblCantieri");
            sql.AppendLine($"WHERE A.IdTblCantieri = @idCantiere");
            sql.AppendLine($"ORDER BY A.IdMaterialiCantiere");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<MaterialiCantieri>(sql.ToString(), new { idCantiere, percentuale }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetMaterialeCantiereForRicalcoloConti in MaterialiCantieriDAO", ex);
            }
            return ret;
        }

        public static MaterialiCantieri GetSingle(int idMaterialiCantiere)
        {
            MaterialiCantieri ret = new MaterialiCantieri();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT * FROM TblMaterialiCantieri WHERE idMaterialiCantiere = @idMaterialiCantiere");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<MaterialiCantieri>(sql.ToString(), new { idMaterialiCantiere }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero del singolo record dei Materiali Cantieri", ex);
            }
            return ret;
        }

        //Calcolo Totali
        public static decimal TotaleVisibile(string idCantiere)
        {
            decimal ret = 0;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT ISNULL(SUM(PzzoUniCantiere * Qta), 0)");
            sql.AppendLine($"FROM TblMaterialiCantieri");
            sql.AppendLine($"WHERE (Tipologia = 'MATERIALE' OR Tipologia = 'A CHIAMATA') AND Visibile = 1 AND Ricalcolo = 1 AND PzzoFinCli = 0 AND IdTblCantieri = @idCantiere");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<decimal>(sql.ToString(), new { idCantiere }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il calcolo del materiale visibile", ex);
            }
            return ret;
        }

        public static decimal TotaleNascosto(string idCantiere)
        {
            decimal ret = 0;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT ISNULL(SUM(PzzoUniCantiere * Qta), 0)");
            sql.AppendLine($"FROM TblMaterialiCantieri");
            sql.AppendLine($"WHERE Visibile = 0 AND IdTblCantieri = @idCantiere");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<decimal>(sql.ToString(), new { idCantiere }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il calcolo del materiale nascosto", ex);
            }
            return ret;
        }

        //INSERT
        public static bool InserisciMaterialeCantiere(MaterialiCantieri mc)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"INSERT INTO TblMaterialiCantieri (IdTblCantieri{(mc.IdTblOperaio != 0 ? ",IdTblOperaio" : "")},DescriMateriali,Qta,Visibile,Ricalcolo,ricaricoSiNo,Data,");
            sql.AppendLine($"PzzoUniCantiere,CodArt,DescriCodArt,Tipologia,Fascia,Acquirente,Fornitore,NumeroBolla,ProtocolloInterno,Note,Note2,pzzoFinCli)");
            sql.AppendLine($"VALUES (@IdTblCantieri{(mc.IdTblOperaio != 0 ? ",@IdTblOperaio" : "")},@DescriMateriali,@Qta,@Visibile,@Ricalcolo,@RicaricoSiNo,@Data,@PzzoUniCantiere,@CodArt,@DescriCodArt,@Tipologia,@Fascia,");
            sql.AppendLine($"@Acquirente,@Fornitore,@NumeroBolla,@ProtocolloInterno,@Note,@Note2,@pzzoFinCli)");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), mc) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'inserimento di un materiale cantiere", ex);
            }
            return ret;
        }

        //UPDATE
        public static bool UpdateOperaioPagato(string dataInizio, string dataFine, string idOperaio)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder("UPDATE TblMaterialiCantieri SET OperaioPagato = 1 WHERE (Data BETWEEN Convert(date, @dataInizio) AND Convert(date, @dataFine)) AND IdTblOperaio = @idOperaio");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), new { dataInizio, dataFine, idOperaio }) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'update del campo OperaioPagato", ex);
            }
            return ret;
        }

        public static bool UpdateMatCant(MaterialiCantieri mc)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"UPDATE TblMaterialiCantieri");
            sql.AppendLine($"SET IdTblCantieri = @IdTblCantieri");
            sql.AppendLine(mc.IdTblOperaio != 0 ? $",IdTblOperaio = @IdTblOperaio" : "");
            sql.AppendLine($",DescriMateriali = @DescriMateriali");
            sql.AppendLine($",Qta = @Qta");
            sql.AppendLine($",Visibile = @Visibile");
            sql.AppendLine($",Ricalcolo = @Ricalcolo");
            sql.AppendLine($",ricaricoSiNo = @RicaricoSiNo");
            sql.AppendLine($",Data = @Data");
            sql.AppendLine($",PzzoUniCantiere = @PzzoUniCantiere");
            sql.AppendLine($",CodArt = @CodArt");
            sql.AppendLine($",DescriCodArt = @DescriCodArt");
            sql.AppendLine($",Fascia = @Fascia");
            sql.AppendLine($",NumeroBolla = @NumeroBolla");
            sql.AppendLine($",ProtocolloInterno = @ProtocolloInterno");
            sql.AppendLine($",Note = @Note");
            sql.AppendLine($",Tipologia = @Tipologia");
            sql.AppendLine($",PzzoFinCli = @PzzoFinCli");
            sql.AppendLine($",Acquirente = @Acquirente");
            sql.AppendLine($",Fornitore = @Fornitore");
            sql.AppendLine($",Note2 = @Note2");
            sql.AppendLine($"WHERE IdMaterialiCantiere = @IdMaterialiCantiere");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), mc) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'update del record MatCant", ex);
            }
            return ret;
        }

        public static bool UpdateValoreManodopera(string id, string valManodop)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder("UPDATE TblMaterialiCantieri SET PzzoUniCantiere = @valManodop WHERE IdTblCantieri = @id AND Tipologia = 'MANODOPERA'");

            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), new { id, valManodop }) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'update del valore della manodopera", ex);
            }
            return ret;
        }

        public static bool UpdateCostoOperaio(string idCantiere, string costoOperaio, string idOperaio)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder("UPDATE TblMaterialiCantieri SET PzzoUniCantiere = @costoOperaio WHERE IdTblCantieri = @idCantiere AND IdTblOperaio = @idOperaio AND Tipologia = 'OPERAIO'");

            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), new { idCantiere, costoOperaio, idOperaio }) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'update del costo Operaio", ex);
            }
            return ret;
        }

        //DELETE
        public static bool DeleteMatCant(int idMatCant)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder("DELETE FROM TblMaterialiCantieri WHERE IdMaterialiCantiere = @idMatCant");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), new { idMatCant }) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'eliminazione di un record per i materialiCantieri", ex);
            }
            return ret;
        }
    }
}