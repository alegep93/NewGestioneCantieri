using Dapper;
using Database.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Database.DAO
{
    public class OperaiDAO : BaseDAO
    {
        public static List<Operai> GetAll(string nome = "", string descrizione = "")
        {
            List<Operai> ret = new List<Operai>();
            StringBuilder sql = new StringBuilder();
            try
            {
                sql.AppendLine($"SELECT *");
                sql.AppendLine($"FROM TblOperaio");
                sql.AppendLine($"WHERE NomeOp LIKE '%{nome}%' AND DescrOP LIKE '%{descrizione}%'");
                sql.AppendLine($"ORDER BY NomeOp");
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Operai>(sql.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetAll in OperaiDAO", ex);
            }
            return ret;
        }

        public static Operai GetSingle(int idOperaio)
        {
            Operai ret = new Operai();
            StringBuilder sql = new StringBuilder();
            try
            {
                sql.AppendLine($"SELECT *");
                sql.AppendLine($"FROM TblOperaio");
                sql.AppendLine($"WHERE IdOperaio = @idOperaio");
                sql.AppendLine($"ORDER BY NomeOp");
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<Operai>(sql.ToString(), new { idOperaio }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetSingle in OperaiDAO", ex);
            }
            return ret;
        }

        public static bool InserisciOperaio(Operai operaio)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"INSERT INTO TblOperaio (NomeOp,DescrOP,Suffisso,Operaio,CostoOperaio)");
            sql.AppendLine($"VALUES (@NomeOp,@DescrOp,@Suffisso,@Operaio,@CostoOperaio)");
            try
            {

                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), operaio) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'inserimento di un nuovo operaio", ex);
            }
            return ret;
        }

        public static bool UpdateOperaio(Operai operaio)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE TblOperaio");
            sql.AppendLine("SET NomeOp = @NomeOp, DescrOp = @DescrOp, Suffisso = @Suffisso, Operaio = @Operaio, CostoOperaio = @CostoOperaio");
            sql.AppendLine("WHERE IdOperaio = @IdOperaio");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), operaio) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'update di un operaio", ex);
            }
            return ret;
        }

        public static bool EliminaOperaio(int idOperaio)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"IF NOT EXISTS (SELECT * FROM TblMaterialiCantieri WHERE IdTblOperaio = @idOperaio)");
            sql.AppendLine($"DELETE FROM TblOperaio WHERE IdOperaio = @idOperaio");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    int row = cn.Execute(sql.ToString(), new { idOperaio });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'eliminazione dell'operaio", ex);
            }
            return ret;
        }
    }
}