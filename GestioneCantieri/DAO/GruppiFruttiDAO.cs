using Dapper;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GestioneCantieri.DAO
{
    public class GruppiFruttiDAO : BaseDAO
    {
        public static bool CreaGruppo(string nomeGruppo, string descr)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("IF NOT EXISTS(SELECT NomeGruppo FROM TblGruppiFrutti WHERE NomeGruppo = @nomeGruppo)");
            sql.AppendLine("INSERT INTO TblGruppiFrutti(NomeGruppo,Descrizione,Completato) VALUES (@nomeGruppo,@descr,0)");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), new { nomeGruppo, descr }) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la creazione di un nuovo gruppo", ex);
            }
            return ret;
        }

        public static int InserisciGruppo(string nomeGruppo, string descr)
        {
            int ret = 0;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("IF NOT EXISTS(SELECT NomeGruppo FROM TblGruppiFrutti WHERE NomeGruppo = @nomeGruppo)");
            sql.AppendLine("INSERT INTO TblGruppiFrutti(NomeGruppo,Descrizione,Completato) VALUES (@nomeGruppo,@descr,0)");
            sql.AppendLine("SELECT CAST(scope_identity() AS int)");

            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<int>(sql.ToString(), new { nomeGruppo, descr }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la creazione di un nuovo gruppo", ex);
            }
            return ret;
        }

        public static List<GruppiFrutti> GetGruppi(string filtroNome1, string filtroNome2, string filtroNome3, bool gruppiNonCompletati = false)
        {
            List<GruppiFrutti> ret = new List<GruppiFrutti>();
            StringBuilder sql = new StringBuilder();

            filtroNome1 = "%" + filtroNome1 + "%";
            filtroNome2 = "%" + filtroNome2 + "%";
            filtroNome3 = "%" + filtroNome3 + "%";

            sql.AppendLine($"SELECT Id,NomeGruppo,Descrizione");
            sql.AppendLine($"FROM TblGruppiFrutti");
            sql.AppendLine($"WHERE NomeGruppo LIKE @filtroNome1 AND NomeGruppo LIKE @filtroNome2 AND NomeGruppo LIKE @filtroNome3");
            sql.AppendLine(gruppiNonCompletati ? "AND Completato = 0" : "");
            sql.AppendLine($"ORDER BY NomeGruppo ASC");

            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<GruppiFrutti>(sql.ToString(), new { filtroNome1, filtroNome2, filtroNome3 }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero dei gruppi", ex);
            }
            return ret;
        }

        public static List<GruppiFrutti> GetAll()
        {
            List<GruppiFrutti> ret = new List<GruppiFrutti>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT Id,NomeGruppo,Descrizione,Completato,Controllato");
            sql.AppendLine("FROM TblGruppiFrutti");
            sql.AppendLine("ORDER BY NomeGruppo ASC");

            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<GruppiFrutti>(sql.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero dei gruppi non controllati", ex);
            }
            return ret;
        }

        public static GruppiFrutti GetSingle(int idGruppo)
        {
            GruppiFrutti ret = new GruppiFrutti();
            StringBuilder sql = new StringBuilder("SELECT * FROM TblGruppiFrutti WHERE Id = @idGruppo");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<GruppiFrutti>(sql.ToString(), new { idGruppo }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetSingle in GruppiFruttiDAO", ex);
            }
            return ret;
        }

        public static GruppiFrutti GetByNome(string nomeGruppo)
        {
            GruppiFrutti ret = new GruppiFrutti();
            StringBuilder sql = new StringBuilder("SELECT * FROM TblGruppiFrutti WHERE NomeGruppo = @nomeGruppo");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<GruppiFrutti>(sql.ToString(), new { nomeGruppo }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetByNome in GruppiFruttiDAO", ex);
            }
            return ret;
        }

        public static bool UpdateGruppo(GruppiFrutti item)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE TblGruppiFrutti");
            sql.AppendLine("SET NomeGruppo = @NomeGruppo, Descrizione = @Descrizione");
            sql.AppendLine("WHERE Id = @Id");

            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), item) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'aggiornamento di un gruppo", ex);
            }
            return ret;
        }


        public static bool DeleteGruppo(int idGruppo)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("IF NOT EXISTS(SELECT Id FROM TblCompGruppoFrut WHERE IdTblGruppo = @idGruppo)");
            sql.AppendLine("DELETE FROM TblGruppiFrutti WHERE Id = @idGruppo ");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), new { idGruppo }) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la DeleteGruppo in GruppoFruttiDAO", ex);
            }
            return ret;
        }

        public static bool CompletaRiapriGruppo(string idGruppo, bool completa)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder("UPDATE TblGruppiFrutti SET Completato = @completa WHERE Id = @idGruppo");

            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), new { idGruppo, completa }) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la CompletaRiapriGruppo in GruppiFruttiDAO", ex);
            }
            return ret;
        }

        public static bool UpdateFlagControllato(int idGruppo)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder("UPDATE TblGruppiFrutti SET Controllato = 1 WHERE Id = @idGruppo");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), new { idGruppo }) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la UpdateFlagControllato in GruppiFruttiDAO per il gruppo = " + idGruppo, ex);
            }
            return ret;
        }
    }
}