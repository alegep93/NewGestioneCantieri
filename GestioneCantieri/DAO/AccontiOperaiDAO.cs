using Dapper;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace GestioneCantieri.DAO
{
    public class AccontiOperaiDAO : BaseDAO
    {
        public static List<AccontoOperaio> GetAcconti(long idOperaio)
        {
            List<AccontoOperaio> ret = new List<AccontoOperaio>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT A.*, B.NomeOp");
            sql.AppendLine($"FROM TblAccontiOperai A");
            sql.AppendLine($"INNER JOIN TblOperaio B ON A.IdOperaio = B.IdOperaio");
            sql.AppendLine(idOperaio != -1 ? $" WHERE A.IdOperaio = @idOperaio" : "");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<AccontoOperaio>(sql.ToString(), new { idOperaio }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetAll in AccontiOperaiDAO", ex);
            }
            return ret;
        }

        public static AccontoOperaio GetSingle(long idAccontoOperaio)
        {
            AccontoOperaio ret = new AccontoOperaio();
            StringBuilder sql = new StringBuilder("SELECT * FROM TblAccontiOperai WHERE IdAccontoOperaio = @idAccontoOperaio");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<AccontoOperaio>(sql.ToString(), new { idAccontoOperaio }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GeSingle in AccontiOperaiDAO", ex);
            }
            return ret;
        }

        public static void Insert(AccontoOperaio item)
        {
            StringBuilder sql = new StringBuilder($"INSERT INTO TblAccontiOperai(IdOperaio,Data,Importo,Descrizione) VALUES (@IdOperaio,@Data,@Importo,@Descrizione)");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql.ToString(), item);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Insert in AccontiOperaiDAO", ex);
            }
        }

        public static void Update(AccontoOperaio item)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"UPDATE TblAccontiOperai");
            sql.AppendLine($"SET IdOperaio = @IdOperaio, Data = @Data, Importo = @Importo, Descrizione = @Descrizione");
            sql.AppendLine($"WHERE IdAccontoOperaio = @IdAccontoOperaio");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql.ToString(), item);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Update in AccontiOperaiDAO", ex);
            }
        }

        public static bool UpdateAccontoPagato(DateTime dataDa, DateTime dataA, int idOperaio)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"UPDATE TblAccontiOperai");
            sql.AppendLine($"SET Pagato = 1");
            sql.AppendLine($"WHERE IdOperaio = @idOperaio AND Data BETWEEN @dataDa AND @dataA");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), new { idOperaio, dataDa, dataA }) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Update in AccontiOperaiDAO", ex);
            }
            return ret;
        }

        public static void Delete(long idAccontoOperaio)
        {
            StringBuilder sql = new StringBuilder($"DELETE FROM TblAccontiOperai WHERE IdAccontoOperaio = @idAccontoOperaio");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql.ToString(), new { idAccontoOperaio });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Delete in AccontiOperaiDAO", ex);
            }
        }
    }
}