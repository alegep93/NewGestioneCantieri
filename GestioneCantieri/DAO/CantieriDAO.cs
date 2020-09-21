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
    public class CantieriDAO : BaseDAO
    {
        // SELECT
        public static Cantieri GetSingle(string id)
        {
            Cantieri ret = new Cantieri();
            StringBuilder sql = new StringBuilder();

            sql.AppendLine($"SELECT Cant.IdCantieri, Cli.RagSocCli, Cant.CodCant, Cant.DescriCodCAnt,");
            sql.AppendLine($"Cant.Data, Cant.Indirizzo, Cant.Città, Cant.Ricarico,");
            sql.AppendLine($"Cant.PzzoManodopera, Cant.Chiuso, Cant.Riscosso, Cant.Numero,");
            sql.AppendLine($"Cant.ValorePreventivo, Cant.IVA, Cant.Anno, Cant.Preventivo,");
            sql.AppendLine($"Cant.FasciaTblCantieri, Cant.DaDividere, Cant.Diviso, Cant.Fatturato");
            sql.AppendLine($"FROM TblCantieri AS Cant");
            sql.AppendLine($"JOIN TblClienti AS Cli ON Cant.IdTblClienti = Cli.IdCliente");
            sql.AppendLine($"WHERE Cant.IdCantieri = @idCant");
            sql.AppendLine($"ORDER BY Cant.CodCant ASC");

            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Cantieri>(sql.ToString(), new { idCant = id }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetSingle in CantieriDAO", ex);
            }
            return ret;
        }
        public static DataTable GetCantieri(string anno, string codCant, string descr, bool chiuso, bool riscosso)
        {
            StringBuilder sql = new StringBuilder();

            anno = "%" + anno + "%";
            codCant = "%" + codCant + "%";
            descr = "%" + descr + "%";

            try
            {
                sql.AppendLine($"SELECT Cant.IdCantieri, Cli.RagSocCli, Cant.CodCant, Cant.DescriCodCAnt,");
                sql.AppendLine($"Cant.Data, Cant.Indirizzo, Cant.Città, Cant.Ricarico,");
                sql.AppendLine($"Cant.PzzoManodopera, Cant.Chiuso, Cant.Riscosso, Cant.Numero,");
                sql.AppendLine($"Cant.ValorePreventivo, Cant.IVA, Cant.Anno, Cant.Preventivo,");
                sql.AppendLine($"Cant.FasciaTblCantieri, Cant.DaDividere, Cant.Diviso, Cant.Fatturato");
                sql.AppendLine($"FROM TblCantieri AS Cant");
                sql.AppendLine($"JOIN TblClienti AS Cli ON(Cant.IdTblClienti = Cli.IdCliente)");
                sql.AppendLine($"WHERE Anno LIKE @pAnno AND CodCant LIKE @pCodCant AND DescriCodCAnt LIKE @pDescr");
                sql.AppendLine($"AND Chiuso LIKE @pChiuso AND Riscosso LIKE @pRiscosso");
                sql.AppendLine($"ORDER BY Cant.CodCant ASC");

                using (SqlConnection cn = GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(sql.ToString(), cn))
                    {
                        cmd.Parameters.Add(new SqlParameter("pAnno", anno));
                        cmd.Parameters.Add(new SqlParameter("pCodCant", codCant));
                        cmd.Parameters.Add(new SqlParameter("pDescr", descr));
                        cmd.Parameters.Add(new SqlParameter("pChiuso", chiuso));
                        cmd.Parameters.Add(new SqlParameter("pRiscosso", riscosso));

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable table = new DataTable();
                            table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                            adapter.Fill(table);
                            return table;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'applicazione dei filtri sui cantieri", ex);
            }
        }

        public static DataTable GetCantieri(string anno, string codCant, bool fatturato, bool chiuso, bool riscosso)
        {
            SqlConnection cn = GetConnection();
            string sql = "";

            anno = "%" + anno + "%";
            codCant = "%" + codCant + "%";

            try
            {
                sql = "SELECT Cant.IdCantieri, Cli.RagSocCli, Cant.CodCant, Cant.DescriCodCAnt, " +
                      "Cant.Data, Cant.Indirizzo, Cant.Città, Cant.Ricarico, " +
                      "Cant.PzzoManodopera, Cant.Chiuso, Cant.Riscosso, Cant.Numero, " +
                      "Cant.ValorePreventivo, Cant.IVA, Cant.Anno, Cant.Preventivo, " +
                      "Cant.FasciaTblCantieri, Cant.DaDividere, Cant.Diviso, Cant.Fatturato " +
                      "FROM TblCantieri AS Cant " +
                      "JOIN TblClienti AS Cli ON(Cant.IdTblClienti = Cli.IdCliente) " +
                      "WHERE Anno LIKE @pAnno AND CodCant LIKE @pCodCant " +
                      "AND Chiuso = @pChiuso AND Riscosso = @pRiscosso AND Fatturato = @pFatturato " +
                      "ORDER BY Cant.CodCant ASC ";

                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add(new SqlParameter("pAnno", anno));
                cmd.Parameters.Add(new SqlParameter("pCodCant", codCant));
                cmd.Parameters.Add(new SqlParameter("pFatturato", fatturato));
                cmd.Parameters.Add(new SqlParameter("pChiuso", chiuso));
                cmd.Parameters.Add(new SqlParameter("pRiscosso", riscosso));

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                adapter.Fill(table);

                return table;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'applicazione dei filtri sui cantieri", ex);
            }
            finally { cn.Close(); }
        }
        public static List<Cantieri> GetCantieri(string anno, int idCliente, bool fatturato, bool chiuso, bool riscosso, bool nonRiscuotibile)
        {
            SqlConnection cn = GetConnection();
            ////SqlDataReader dr = null;
            //List<Cantieri> list = new List<Cantieri>();
            string sql = "";

            anno = "%" + anno + "%";

            try
            {
                sql = "SELECT Cant.IdCantieri, Cli.RagSocCli, Cant.CodCant, Cant.DescriCodCAnt, " +
                      "Cant.Data, Cant.Indirizzo, Cant.Città, Cant.Ricarico, " +
                      "Cant.PzzoManodopera, Cant.Chiuso, Cant.Riscosso, Cant.Numero, " +
                      "Cant.ValorePreventivo, Cant.IVA, Cant.Anno, Cant.Preventivo, " +
                      "Cant.FasciaTblCantieri, Cant.DaDividere, Cant.Diviso, Cant.Fatturato, Cant.NonRiscuotibile, Cant.codRiferCant " +
                      "FROM TblCantieri AS Cant " +
                      "JOIN TblClienti AS Cli ON (Cant.IdTblClienti = Cli.IdCliente) " +
                      "WHERE Anno LIKE @pAnno " +
                      "AND Chiuso = @pChiuso AND Riscosso = @pRiscosso AND Fatturato = @pFatturato AND NonRiscuotibile = @nonRiscuotibile ";

                if (idCliente != -1)
                {
                    sql += "AND IdTblClienti = @idCliente ";
                }

                sql += "ORDER BY Cant.CodCant ASC ";

                return cn.Query<Cantieri>(sql, new { pAnno = anno, pFatturato = fatturato, pChiuso = chiuso, pRiscosso = riscosso, idCliente, nonRiscuotibile }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero della lista dei cantieri", ex);
            }
            finally { CloseResouces(cn, null); }
        }
        public static List<Cantieri> GetCantieriAperti()
        {
            SqlConnection cn = GetConnection();
            string sql = "";

            try
            {
                sql = "SELECT IdCantieri,CodCant,DescriCodCAnt FROM TblCantieri " +
                      "WHERE Chiuso = 0 " +
                      "ORDER BY CodCant ASC ";

                return cn.Query<Cantieri>(sql).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero dei cantieri", ex);
            }
            finally { CloseResouces(cn, null); }
        }
        public static Cantieri GetByIdCantiere(int idCantiere)
        {
            try
            {
                string sql = "SELECT IdCantieri,CodCant,DescriCodCAnt FROM TblCantieri WHERE IdCantieri = @idCantiere";
                using (SqlConnection cn = GetConnection())
                {
                    return cn.Query<Cantieri>(sql, new { idCantiere }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero dei cantieri", ex);
            }
        }
        public static List<Cantieri> GetForFatture(string codiceCantiere, string descrizioneCantiere)
        {
            codiceCantiere = $"'%{codiceCantiere}%'";
            descrizioneCantiere = $"'%{descrizioneCantiere}%'";

            string sql = "SELECT * FROM TblCantieri ";
            sql += "WHERE Fatturato = 0 AND Riscosso = 0 AND NonRiscuotibile = 0 ";
            sql += codiceCantiere != "%%" ? $"AND CodCant LIKE {codiceCantiere} " : "";
            sql += descrizioneCantiere != "%%" ? $"AND DescriCodCAnt LIKE {descrizioneCantiere} " : "";
            sql += "ORDER BY CodCant ASC ";

            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    return cn.Query<Cantieri>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero dei cantieri", ex);
            }
        }
        public static DataTable GetAllCantieri()
        {
            SqlConnection cn = GetConnection();
            string sql = "";

            try
            {
                sql = "SELECT Cant.IdCantieri, Cli.RagSocCli, Cant.CodCant, Cant.DescriCodCAnt, " +
                      "Cant.Data, Cant.Indirizzo, Cant.Città, Cant.Ricarico, " +
                      "Cant.PzzoManodopera, Cant.Chiuso, Cant.Riscosso, Cant.Numero, " +
                      "Cant.ValorePreventivo, Cant.IVA, Cant.Anno, Cant.Preventivo, " +
                      "Cant.FasciaTblCantieri, Cant.DaDividere, Cant.Diviso, Cant.Fatturato, Cant.codRiferCant " +
                      "FROM TblCantieri AS Cant " +
                      "JOIN TblClienti AS Cli ON(Cant.IdTblClienti = Cli.IdCliente) " +
                      "ORDER BY Cant.CodCant ASC ";

                SqlCommand cmd = new SqlCommand(sql, cn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                DataTable table = new DataTable();
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                adapter.Fill(table);

                return table;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero dei cantieri", ex);
            }
            finally { cn.Close(); }
        }
        public static DataTable GetAllCantieri(bool chiuso, bool riscosso)
        {
            SqlConnection cn = GetConnection();
            string sql = "";

            try
            {
                sql = "SELECT Cant.IdCantieri, Cli.RagSocCli, Cant.CodCant, Cant.DescriCodCAnt, " +
                      "Cant.Data, Cant.Indirizzo, Cant.Città, Cant.Ricarico, " +
                      "Cant.PzzoManodopera, Cant.Chiuso, Cant.Riscosso, Cant.Numero, " +
                      "Cant.ValorePreventivo, Cant.IVA, Cant.Anno, Cant.Preventivo, " +
                      "Cant.FasciaTblCantieri, Cant.DaDividere, Cant.Diviso, Cant.Fatturato, Cant.codRiferCant " +
                      "FROM TblCantieri AS Cant " +
                      "JOIN TblClienti AS Cli ON(Cant.IdTblClienti = Cli.IdCliente) " +
                      "WHERE Cant.Chiuso = 0 AND Cant.Riscosso = 0 " +
                      "ORDER BY Cant.CodCant ASC ";

                SqlCommand cmd = new SqlCommand(sql, cn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                DataTable table = new DataTable();
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                adapter.Fill(table);

                return table;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero dei cantieri", ex);
            }
            finally { cn.Close(); }
        }
        public static Cantieri GetSingleCantiere(int idCant)
        {
            SqlConnection cn = GetConnection();
            //SqlDataReader dr = null;
            //Cantieri c = new Cantieri();
            string sql = "";

            try
            {
                sql = "SELECT Cant.IdCantieri, Cli.RagSocCli, Cant.CodCant, Cant.DescriCodCAnt, " +
                      "Cant.Data, Cant.Indirizzo, Cant.Città, Cant.Ricarico, " +
                      "Cant.PzzoManodopera, Cant.Chiuso, Cant.Riscosso, Cant.Numero, " +
                      "Cant.ValorePreventivo, Cant.IVA, Cant.Anno, Cant.Preventivo, " +
                      "Cant.FasciaTblCantieri, Cant.DaDividere, Cant.Diviso, Cant.Fatturato, Cant.NonRiscuotibile, Cant.CodRiferCant " +
                      "FROM TblCantieri AS Cant " +
                      "JOIN TblClienti AS Cli ON(Cant.IdTblClienti = Cli.IdCliente) " +
                      "WHERE Cant.IdCantieri = @pIdCant " +
                      "ORDER BY Cli.RagSocCli ASC, Cant.CodCant ASC ";

                return cn.Query<Cantieri>(sql, new { pIdCant = idCant }).SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero di un singolo cantiere", ex);
            }
            finally { CloseResouces(cn, null); }
        }
        public static string GetLastNumCantForYear(string anno)
        {
            SqlConnection cn = GetConnection();
            string sql = "";

            try
            {
                sql = "SELECT (MAX(Numero)+1) FROM TblCantieri WHERE Anno = @pAnno ";

                return cn.Query<string>(sql, new { pAnno = anno }).SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recuper dell'ultimo numero cantiere", ex);
            }
            finally { CloseResouces(cn, null); }
        }
        public static string GetNumCantPerAnno(string anno)
        {
            string sql = "SELECT COUNT(*) FROM TblCantieri WHERE Anno = @anno ";
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    return cn.Query<string>(sql, new { anno }).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recuper dell'ultimo numero cantiere", ex);
            }
        }
        public static DataTable FiltraCantieri(string anno, string codCant, string descr, string cliente, bool chiuso, bool riscosso, bool fatturato, bool nonRiscuotibile)
        {
            SqlConnection cn = GetConnection();
            string sql = "";

            anno = "%" + anno + "%";
            codCant = "%" + codCant + "%";
            descr = "%" + descr + "%";
            cliente = "%" + cliente + "%";

            try
            {
                sql = "SELECT Cant.IdCantieri, Cli.RagSocCli, Cant.CodCant, Cant.DescriCodCAnt, " +
                      "Cant.Data, Cant.Indirizzo, Cant.Città, Cant.Ricarico, " +
                      "Cant.PzzoManodopera, Cant.Chiuso, Cant.Riscosso, Cant.Numero, " +
                      "Cant.ValorePreventivo, Cant.IVA, Cant.Anno, Cant.Preventivo, " +
                      "Cant.FasciaTblCantieri, Cant.DaDividere, Cant.Diviso, Cant.Fatturato, Cant.NonRiscuotibile, Cant.codRiferCant " +
                      "FROM TblCantieri AS Cant " +
                      "JOIN TblClienti AS Cli ON(Cant.IdTblClienti = Cli.IdCliente) " +
                      "WHERE Anno LIKE @pAnno AND CodCant LIKE @pCodCant AND DescriCodCAnt LIKE @pDescr AND Cli.RagSocCli LIKE @pRagSocCli " +
                      "AND Chiuso LIKE @pChiuso AND Riscosso LIKE @pRiscosso AND Fatturato LIKE @pFatturato AND NonRiscuotibile LIKE @pNonRiscuotibile " +
                      "ORDER BY Cant.CodCant ASC ";

                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.Add(new SqlParameter("pAnno", anno));
                cmd.Parameters.Add(new SqlParameter("pCodCant", codCant));
                cmd.Parameters.Add(new SqlParameter("pDescr", descr));
                cmd.Parameters.Add(new SqlParameter("pRagSocCli", cliente));
                cmd.Parameters.Add(new SqlParameter("pChiuso", chiuso));
                cmd.Parameters.Add(new SqlParameter("pRiscosso", riscosso));
                cmd.Parameters.Add(new SqlParameter("pFatturato", fatturato));
                cmd.Parameters.Add(new SqlParameter("pNonRiscuotibile", nonRiscuotibile));

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                adapter.Fill(table);

                return table;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'applicazione dei filtri sui cantieri", ex);
            }
            finally { cn.Close(); }
        }

        //Estrazione dati per creazione intestazione Conto Finale Cliente e Verifica Cantieri
        public static MaterialiCantieri GetDataPerIntestazione(string idCant)
        {
            SqlConnection cn = GetConnection();
            string sql = "";

            try
            {
                sql = "SELECT C.RagSocCli,B.CodCant,B.DescriCodCAnt " +
                      "FROM TblCantieri AS B " +
                      "LEFT JOIN TblClienti AS C ON (B.IdTblClienti = C.IdCliente) " +
                      "WHERE B.IdCantieri = @pIdCant ";

                return cn.Query<MaterialiCantieri>(sql, new { pIdCant = idCant }).SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero dei dati per l'intestazione del conto fin. cli.", ex);
            }
            finally { CloseResouces(cn, null); }
        }

        // INSERT
        public static bool InserisciCantiere(Cantieri c)
        {
            SqlConnection cn = GetConnection();
            string sql = "";

            try
            {
                sql = "INSERT INTO TblCantieri " +
                      "(IdTblClienti,Data,CodCant,DescriCodCAnt,Indirizzo,Città,Ricarico, " +
                      "PzzoManodopera,Chiuso,Riscosso,Numero,ValorePreventivo,IVA,Anno,Preventivo, " +
                      "FasciaTblCantieri,DaDividere,Diviso,Fatturato,NonRiscuotibile,CodRiferCant) " +
                      "VALUES (@IdTblClienti,CONVERT(date,@Data),@CodCant,@DescriCodCAnt,@Indirizzo,@Città,@Ricarico, " +
                      "@PzzoManodopera,@Chiuso,@Riscosso,@Numero,@ValorePreventivo,@IVA,@Anno,@Preventivo,@FasciaTblCantieri, " +
                      "@DaDividere,@Diviso,@Fatturato,@NonRiscuotibile,@CodRiferCant) ";

                int ret = cn.Execute(sql, c);

                if (ret > 0)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'inserimento di un nuovo cantiere", ex);
            }
            finally { CloseResouces(cn, null); }
        }

        // UPDATE
        public static bool UpdateCantiere(Cantieri c)
        {
            SqlConnection cn = GetConnection();
            string sql = "";

            try
            {
                sql = "UPDATE TblCantieri " +
                      "SET IdTblClienti = @IdTblClienti, Data = CONVERT(date,@Data), " +
                      "CodCant = @CodCant, DescriCodCAnt = @DescriCodCAnt, " +
                      "Indirizzo = @Indirizzo, Città = @Città, " +
                      "Ricarico = @Ricarico, PzzoManodopera = @PzzoManodopera, " +
                      "Chiuso = @Chiuso, Riscosso = @Riscosso, " +
                      "Numero = @Numero, ValorePreventivo = @ValorePreventivo, " +
                      "IVA = @IVA, Anno = @Anno, " +
                      "Preventivo = @Preventivo, FasciaTblCantieri = @FasciaTblCantieri, " +
                      "DaDividere = @DaDividere, Diviso = @Diviso, " +
                      "Fatturato = @Fatturato, NonRiscuotibile = @NonRiscuotibile " +
                      "WHERE IdCantieri = @IdCantieri ";

                int row = cn.Execute(sql, c);

                if (row > 0)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'aggiornamento del cantiere " + c.IdCantieri, ex);
            }
            finally { CloseResouces(cn, null); }
        }

        // DELETE
        public static bool EliminaCantiere(int idCant)
        {
            SqlConnection cn = GetConnection();
            string sql = "";

            try
            {
                sql = "DELETE FROM TblCantieri " +
                      "WHERE IdCantieri = @idCant ";

                int row = cn.Execute(sql, new { idCant });

                if (row > 0)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'eliminazione del cantiere", ex);
            }
            finally { CloseResouces(cn, null); }
        }
    }
}