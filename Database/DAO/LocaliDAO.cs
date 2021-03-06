﻿using Dapper;
using Database.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Database.DAO
{
    public class LocaliDAO : BaseDAO
    {
        public static List<Locali> GetAll()
        {
            List<Locali> ret = new List<Locali>();
            StringBuilder sql = new StringBuilder("SELECT IdLocali, NomeLocale FROM TblLocali ORDER BY NomeLocale ASC");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret=cn.Query<Locali>(sql.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetAll in LocaliDAO", ex);
            }
            return ret;
        }

        public static List<Locali> GetOnlyFirstTypeOfLocale()
        {
            List<Locali> ret = new List<Locali>();
            StringBuilder sql = new StringBuilder();
            try
            {
                sql.AppendLine($"SELECT MIN(idLocali) AS idLocali, value AS NomeLocale");
                sql.AppendLine($"FROM TblLocali");
                sql.AppendLine($"CROSS APPLY STRING_SPLIT(NomeLocale, '0')");
                sql.AppendLine($"WHERE RTRIM(value) != '' AND RTRIM(value) NOT IN ('1','2','3','4','5','6','7','8','9')");
                sql.AppendLine($"GROUP BY [value]");
                sql.AppendLine($"ORDER BY [value]");
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Locali>(sql.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetAll in LocaliDAO", ex);
            }
            return ret;
        }

        public static Locali GetSingle(int idLocale)
        {
            Locali ret = new Locali();
            StringBuilder sql = new StringBuilder("SELECT * FROM TblLocali WHERE IdLocali = @idLocale");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Locali>(sql.ToString(), new { idLocale }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetAll in LocaliDAO", ex);
            }
            return ret;
        }

        public static bool InserisciLocale(string nomeLocale)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder("INSERT INTO TblLocali(NomeLocale) VALUES (@nomeLocale)");

            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql.ToString(), new { nomeLocale });
                }
                ret = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la InserisciLocale in LocaliDAO", ex);
            }
            return ret;
        }

        public static bool ModificaLocale(int idLocale, string nomeLocale)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder("UPDATE TblLocali SET NomeLocale = @nomeLocale WHERE IdLocali = @idLocale");

            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql.ToString(), new { idLocale, nomeLocale });
                }
                ret = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la ModificaLocale in LocaliDAO", ex);
            }
            return ret;
        }

        public static bool EliminaLocale(int idLocale)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder("DELETE FROM TblLocali WHERE IdLocali = @idLocale");

            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql.ToString(), new { idLocale });
                }
                ret = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la EliminaLocale in LocaliDAO", ex);
            }
            return ret;
        }
    }
}