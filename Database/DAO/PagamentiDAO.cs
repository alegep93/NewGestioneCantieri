using Dapper;
using Database.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Database.DAO
{
    public class PagamentiDAO : BaseDAO
    {
        //SELECT
        public static List<Pagamenti> GetAll(string descrizionePagamento = "")
        {
            List<Pagamenti> ret = new List<Pagamenti>();
            StringBuilder sql = new StringBuilder($"SELECT * FROM TblPagamenti WHERE DescriPagamenti LIKE '%{descrizionePagamento}%'");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Pagamenti>(sql.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetPagamenti per idCantiere in PagamentiDAO", ex);
            }
            return ret;
        }

        public static Pagamenti GetSingle(int idPagamenti)
        {
            Pagamenti ret = new Pagamenti();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT *");
            sql.AppendLine($"FROM TblPagamenti");
            sql.AppendLine($"WHERE IdPagamenti = @idPagamenti");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Pagamenti>(sql.ToString(), new { idPagamenti }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero del singolo pagamento", ex);
            }
            return ret;
        }

        //INSERT
        public static bool InserisciPagamento(Pagamenti pag)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"INSERT INTO TblPagamenti (IdTblCantieri, data, Imporo, DescriPagamenti, Acconto, Saldo)");
            sql.AppendLine($"VALUES (@IdTblCantieri, @data, @Imporo, @DescriPagamenti, @Acconto, @Saldo)");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), pag) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'inserimento di un pagamento", ex);
            }
            return ret;
        }

        //UPDATE
        public static bool UpdatePagamento(Pagamenti pag)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"UPDATE TblPagamenti");
            sql.AppendLine($"SET IdTblCantieri = @IdTblCantieri, data = @data, Imporo = @Imporo,");
            sql.AppendLine($"DescriPagamenti = @DescriPagamenti, Acconto = @Acconto, Saldo = @Saldo");
            sql.AppendLine($"WHERE IdPagamenti = @IdPagamenti");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), pag) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la modifica di un pagamento", ex);
            }
            return ret;
        }

        //DELETE
        public static bool DeletePagamento(int idPagamenti)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder("DELETE FROM TblPagamenti WHERE IdPagamenti = @idPagamenti");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), new { idPagamenti }) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'eliminazione di un pagamento", ex);
            }
            return ret;
        }
    }
}