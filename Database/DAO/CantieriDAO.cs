using Dapper;
using Database.Models;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Database.DAO
{
    public class CantieriDAO : BaseDAO
    {
        // SELECT
        public static List<Cantieri> GetAll()
        {
            List<Cantieri> ret = new List<Cantieri>();
            StringBuilder sql = new StringBuilder($"SELECT * FROM TblCantieri ORDER BY CodCant");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Cantieri>(sql.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetAll in CantieriDAO", ex);
            }
            return ret;
        }

        public static List<Cantieri> GetCantieri(string anno, string codCant, string descr, string cliente, bool chiuso, bool riscosso, bool fatturato, bool nonRiscuotibile)
        {
            List<Cantieri> ret = new List<Cantieri>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT Cant.*, Cli.RagSocCli");
            sql.AppendLine($"FROM TblCantieri AS Cant");
            sql.AppendLine($"JOIN TblClienti AS Cli ON Cant.IdTblClienti = Cli.IdCliente");
            sql.AppendLine($"WHERE Anno LIKE '%{anno}%' AND CodCant LIKE '%{codCant}%' AND DescriCodCAnt LIKE '%{descr}%' AND Cli.RagSocCli LIKE '%{cliente}%'");
            sql.AppendLine($"AND Chiuso = @chiuso AND Riscosso = @riscosso AND Fatturato = @fatturato AND NonRiscuotibile = @nonRiscuotibile");
            sql.AppendLine($"ORDER BY Cant.CodCant");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Cantieri>(sql.ToString(), new { anno, codCant, descr, cliente, chiuso, riscosso, fatturato, nonRiscuotibile }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'applicazione dei filtri sui cantieri", ex);
            }
            return ret;
        }

        public static List<Cantieri> GetCantieri(string anno, string codiceCantiere, string descrizione, bool chiuso, bool riscosso)
        {
            List<Cantieri> ret = new List<Cantieri>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT Cant.*, Cli.RagSocCli");
            sql.AppendLine($"FROM TblCantieri AS Cant");
            sql.AppendLine($"JOIN TblClienti AS Cli ON Cant.IdTblClienti = Cli.IdCliente");
            sql.AppendLine($"WHERE Anno LIKE '%{anno}%' AND CodCant LIKE '%{codiceCantiere}%' AND DescriCodCAnt LIKE '%{descrizione}%'");
            sql.AppendLine($"AND Chiuso = @chiuso AND Riscosso = @riscosso");
            sql.AppendLine($"ORDER BY Cant.CodCant");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Cantieri>(sql.ToString(), new { anno, codiceCantiere, descrizione, chiuso, riscosso }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetCantieri con codiceCantiere in CantieriDAO", ex);
            }
            return ret;
        }

        public static List<Cantieri> GetCantieri(string anno, string codiceCantiere, string descrizione)
        {
            List<Cantieri> ret = new List<Cantieri>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT Cant.*, Cli.RagSocCli");
            sql.AppendLine($"FROM TblCantieri AS Cant");
            sql.AppendLine($"JOIN TblClienti AS Cli ON Cant.IdTblClienti = Cli.IdCliente");
            sql.AppendLine($"WHERE Anno LIKE '%{anno}%' AND CodCant LIKE '%{codiceCantiere}%' AND DescriCodCAnt LIKE '%{descrizione}%'");
            sql.AppendLine($"ORDER BY Cant.CodCant");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Cantieri>(sql.ToString(), new { anno, codiceCantiere, descrizione }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetCantieri con anno, codiceCantiere, descrizione in CantieriDAO", ex);
            }
            return ret;
        }

        public static List<Cantieri> GetCantieri(string anno, string codiceCantiere, bool fatturato, bool chiuso, bool riscosso)
        {
            List<Cantieri> ret = new List<Cantieri>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT Cant.*, Cli.RagSocCli");
            sql.AppendLine($"FROM TblCantieri AS Cant");
            sql.AppendLine($"JOIN TblClienti AS Cli ON Cant.IdTblClienti = Cli.IdCliente");
            sql.AppendLine($"WHERE Anno LIKE '%{anno}%' AND CodCant LIKE '%{codiceCantiere}%'");
            sql.AppendLine($"AND fatturato = @fatturato AND Chiuso = @chiuso AND Riscosso = @riscosso");
            sql.AppendLine($"ORDER BY Cant.CodCant");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Cantieri>(sql.ToString(), new { anno, codiceCantiere, fatturato, chiuso, riscosso }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetCantieri con codiceCantiere in CantieriDAO", ex);
            }
            return ret;
        }

        public static List<Cantieri> GetCantieri(string anno, int idCliente, bool fatturato, bool chiuso, bool riscosso, bool nonRiscuotibile)
        {
            List<Cantieri> ret = new List<Cantieri>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT Cant.*, Cli.RagSocCli");
            sql.AppendLine($"FROM TblCantieri AS Cant");
            sql.AppendLine($"JOIN TblClienti AS Cli ON Cant.IdTblClienti = Cli.IdCliente");
            sql.AppendLine($"WHERE Anno LIKE '%{anno}%' AND Chiuso = @chiuso AND Riscosso = @riscosso AND Fatturato = @fatturato AND NonRiscuotibile = @nonRiscuotibile");
            sql.AppendLine(idCliente != -1 ? $"AND IdTblClienti = @idCliente" : "");
            sql.AppendLine($"ORDER BY Cant.CodCant");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Cantieri>(sql.ToString(), new { anno, idCliente, chiuso, riscosso, fatturato, nonRiscuotibile }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetCantieri con idCliente in CantieriDAO", ex);
            }
            return ret;
        }

        public static List<Cantieri> GetCantieri(string codiceCantiere, string descrizioneCantiere)
        {
            List<Cantieri> ret = new List<Cantieri>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT * FROM TblCantieri");
            sql.AppendLine($"WHERE CodCant LIKE '%{codiceCantiere}%' AND DescriCodCAnt LIKE '%{descrizioneCantiere}%'");
            sql.AppendLine($"ORDER BY CodCant");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Cantieri>(sql.ToString(), new { codiceCantiere, descrizioneCantiere }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero dei cantieri", ex);
            }
            return ret;
        }

        public static Cantieri GetSingle(int idCantiere)
        {
            Cantieri ret = new Cantieri();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT Cant.*, Cli.RagSocCli");
            sql.AppendLine($"FROM TblCantieri AS Cant");
            sql.AppendLine($"JOIN TblClienti AS Cli ON Cant.IdTblClienti = Cli.IdCliente");
            sql.AppendLine($"WHERE Cant.IdCantieri = @idCantiere");
            sql.AppendLine($"ORDER BY Cant.CodCant");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Cantieri>(sql.ToString(), new { idCantiere }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetSingle in CantieriDAO", ex);
            }
            return ret;
        }

        // INSERT
        public static bool InserisciCantiere(Cantieri c)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"INSERT INTO TblCantieri (IdTblClienti,Data,CodCant,DescriCodCAnt,Indirizzo,Città,Ricarico,");
            sql.AppendLine($"PzzoManodopera,Chiuso,Riscosso,Numero,ValorePreventivo,IVA,Anno,Preventivo,");
            sql.AppendLine($"FasciaTblCantieri,DaDividere,Diviso,Fatturato,NonRiscuotibile,CodRiferCant,id_preventivo)");
            sql.AppendLine($"VALUES (@IdTblClienti,CONVERT(date,@Data),@CodCant,@DescriCodCAnt,@Indirizzo,@Città,@Ricarico,");
            sql.AppendLine($"@PzzoManodopera,@Chiuso,@Riscosso,@Numero,@ValorePreventivo,@IVA,@Anno,@Preventivo,@FasciaTblCantieri,");
            sql.AppendLine($"@DaDividere,@Diviso,@Fatturato,@NonRiscuotibile,@CodRiferCant,@IdPreventivo)");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), c) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'inserimento di un nuovo cantiere", ex);
            }
            return ret;
        }

        // UPDATE
        public static bool UpdateCantiere(Cantieri c)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"UPDATE TblCantieri");
            sql.AppendLine($"SET IdTblClienti = @IdTblClienti, Data = CONVERT(date,@Data),");
            sql.AppendLine($"CodCant = @CodCant, DescriCodCAnt = @DescriCodCAnt,");
            sql.AppendLine($"Indirizzo = @Indirizzo, Città = @Città,");
            sql.AppendLine($"Ricarico = @Ricarico, PzzoManodopera = @PzzoManodopera,");
            sql.AppendLine($"Chiuso = @Chiuso, Riscosso = @Riscosso,");
            sql.AppendLine($"Numero = @Numero, ValorePreventivo = @ValorePreventivo,");
            sql.AppendLine($"IVA = @IVA, Anno = @Anno,");
            sql.AppendLine($"Preventivo = @Preventivo, FasciaTblCantieri = @FasciaTblCantieri,");
            sql.AppendLine($"DaDividere = @DaDividere, Diviso = @Diviso,");
            sql.AppendLine($"Fatturato = @Fatturato, NonRiscuotibile = @NonRiscuotibile,");
            sql.AppendLine($"codRiferCant = @CodRiferCant, id_preventivo = @IdPreventivo");
            sql.AppendLine($"WHERE IdCantieri = @IdCantieri");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), c) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore durante l'aggiornamento del cantiere {c.IdCantieri}", ex);
            }
            return ret;
        }

        public static void SetDiCo(int idCantiere, long numDiCo)
        {
            StringBuilder sql = new StringBuilder($"UPDATE TblCantieri SET NumDiCo = @numDiCo WHERE IdCantieri = @idCantiere");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql.ToString(), new { idCantiere, numDiCo });
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore durante il SetDiCo del cantiere {idCantiere}", ex);
            }
        }

        // DELETE
        public static bool EliminaCantiere(int idCant)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder("DELETE FROM TblCantieri WHERE IdCantieri = @idCant");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), new { idCant }) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'eliminazione del cantiere", ex);
            }
            return ret;
        }

        public static void DeleteDiCo(int idCantiere)
        {
            StringBuilder sql = new StringBuilder("UPDATE TblCantieri SET NumDiCo = NULL WHERE IdCantieri = @idCantiere");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql.ToString(), new { idCantiere });
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore durante il DeleteDiCo del cantiere {idCantiere}", ex);
            }
        }
    }
}