﻿using Dapper;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GestioneCantieri.DAO
{
    public class CompGruppoFrutDAO : BaseDAO
    {
        // SELECT
        public static List<CompGruppoFrut> GetCompGruppo(int idGruppo)
        {
            List<CompGruppoFrut> ret = new List<CompGruppoFrut>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT CGF.Id, F.descr001 'NomeFrutto', CGF.IdTblFrutto, Qta");
            sql.AppendLine("FROM TblCompGruppoFrut AS CGF");
            sql.AppendLine("INNER JOIN TblFrutti AS F ON CGF.IdTblFrutto = F.ID1");
            sql.AppendLine("WHERE IdTblGruppo = @idGruppo");
            sql.AppendLine("ORDER BY CGF.Id ASC");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<CompGruppoFrut>(sql.ToString(), new { idGruppo }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetCompGruppo in CompGruppoFrutDAO", ex);
            }
            return ret;
        }

        public static List<StampaFruttiPerGruppi> GetFruttiInGruppi(string idGruppo)
        {
            List<StampaFruttiPerGruppi> ret = new List<StampaFruttiPerGruppi>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT GF.NomeGruppo, F.descr001 'NomeFrutto', CGF.Qta");
            sql.AppendLine("FROM TblCompGruppoFrut AS CGF");
            sql.AppendLine("INNER JOIN TblGruppiFrutti AS GF ON CGF.IdTblGruppo = GF.Id");
            sql.AppendLine("INNER JOIN TblFrutti AS F ON CGF.IdTblFrutto = F.ID1");
            sql.AppendLine(idGruppo != "-1" && idGruppo != "" ? "WHERE Gf.Id = @idGruppo" : "");
            sql.AppendLine("GROUP BY GF.NomeGruppo, F.descr001, CGF.Qta");
            sql.AppendLine("ORDER BY GF.NomeGruppo");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<StampaFruttiPerGruppi>(sql.ToString(), new { idGruppo }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetFruttiInGruppi in CompGruppoFrutDAO", ex);
            }
            return ret;
        }

        // INSERT
        public static bool InserisciCompGruppo(int idGruppo, int idFrutto, string qta)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder("INSERT INTO TblCompGruppoFrut(IdTblGruppo,IdTblFrutto,Qta) VALUES (@idGruppo,@idFrutto,@qta)");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), new { idGruppo, idFrutto, qta }) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la InserisciCompGruppo in CompGruppoFrutDAO", ex);
            }
            return ret;
        }

        // DELETE
        public static bool DeleteGruppo(int idGruppo)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder("DELETE FROM TblCompGruppoFrut WHERE IdTblGruppo = @idGruppo");
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
                throw new Exception("Errore durante la DeleteGruppo in CompGruppoFrutDAO", ex);
            }
            return ret;
        }

        public static bool Delete(int idCompGruppo)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder("DELETE FROM TblCompGruppoFrut WHERE Id = @idCompGruppo");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql.ToString(), new { idCompGruppo });
                }
                ret = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Delete in CompGruppoFrutDAO", ex);
            }
            return ret;
        }
    }
}