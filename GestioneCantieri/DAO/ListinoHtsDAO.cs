using Dapper;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace GestioneCantieri.DAO
{
    public class ListinoHtsDAO : BaseDAO
    {
        public static List<ListinoHts> GetAllClienti()
        {
            List<ListinoHts> ret = new List<ListinoHts>();
            SqlConnection cn = GetConnection();
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append("SELECT * FROM ListinoHts ORDER BY Codice ");
                ret = cn.Query<ListinoHts>(sql.ToString()).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetAll in ListinoHtsDAO", ex);
            }
            finally { cn.Close(); }

            return ret;
        }

        public static List<ListinoHts> GetAllFiltered(string codice1, string codice2, string codice3, string codProd1, string codProd2, string codProd3, string desc1, string desc2, string desc3)
        {
            string sql = "";
            List<ListinoHts> ret = new List<ListinoHts>();
            SqlConnection cn = GetConnection();

            codice1 = "%" + codice1 + "%";
            codice2 = "%" + codice2 + "%";
            codice3 = "%" + codice3 + "%";
            codProd1 = "%" + codProd1 + "%";
            codProd2 = "%" + codProd2 + "%";
            codProd3 = "%" + codProd3 + "%";
            desc1 = "%" + desc1 + "%";
            desc2 = "%" + desc2 + "%";
            desc3 = "%" + desc3 + "%";

            try
            {
                if (codice1 == "%%" && codice2 == "%%" && codice3 == "%%" && codProd1 == "%%" && codProd2 == "%%" && codProd3 == "%%" && desc1 == "%%" && desc2 == "%%" && desc3 == "%%")
                {
                    sql = "SELECT * FROM ListinoHts ORDER BY Codice ";
                }
                else
                {
                    sql = "SELECT * FROM ListinoHts " +
                          "WHERE Codice LIKE @codice1 AND Codice LIKE @codice2 AND Codice LIKE @codice3 " +
                          "AND codice_prodotto LIKE @codProd1 AND codice_prodotto LIKE @codProd2 AND codice_prodotto LIKE @codProd3 " +
                          "AND Descrizione LIKE @desc1 AND Descrizione LIKE @desc2 AND Descrizione LIKE @desc3 " +
                          "ORDER BY Codice ASC ";
                }

                ret = cn.Query<ListinoHts>(sql, new { codice1, codice2, codice3, codProd1, codProd2, codProd3, desc1, desc2, desc3 }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetAllFiltered in ListinoHtsDAO", ex);
            }
            finally
            {
                CloseResouces(cn, null);
            }

            return ret;
        }

        public static void Delete()
        {
            SqlConnection cn = GetConnection();
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append("DELETE FROM ListinoHts ");
                cn.Execute(sql.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Delete in ListinoHtsDAO", ex);
            }
            finally { cn.Close(); }
        }
    }
}