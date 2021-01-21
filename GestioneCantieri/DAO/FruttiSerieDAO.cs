using Dapper;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GestioneCantieri.DAO
{
    public class FruttiSerieDAO : BaseDAO
    {
        public static List<FruttoSerie> GetAll()
        {
            List<FruttoSerie> ret = new List<FruttoSerie>();
            StringBuilder sql = new StringBuilder();

            sql.AppendLine($"SELECT A.IdFruttoSerie, A.IdSerie, A.IdFrutto, A.CodiceListinoUnivoco, C.NomeSerie, B.descr001 AS NomeFrutto, AA_DES AS DescrizioneListino");
            sql.AppendLine($"FROM TblFruttiSerie A");
            sql.AppendLine($"INNER JOIN TblFrutti B ON A.IdFrutto = B.ID1");
            sql.AppendLine($"INNER JOIN TblSerie C ON A.IdSerie = C.IdSerie");
            sql.AppendLine($"INNER JOIN MAMG0 D ON A.CodiceListinoUnivoco = D.CodiceListinoUnivoco");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<FruttoSerie>(sql.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetAll in FruttiSerieDAO", ex);
            }
            return ret;
        }

        public static void Insert(FruttoSerie fruttoSerie)
        {
            StringBuilder sql = new StringBuilder($"INSERT INTO TblFruttiSerie (IdFrutto, IdSerie, CodiceListinoUnivoco) VALUES (@IdFrutto, @IdSerie, @CodiceListinoUnivoco)");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql.ToString(), fruttoSerie);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Insert in FruttiSerieDAO", ex);
            }
        }

        public static void Update(FruttoSerie fruttoSerie)
        {
            StringBuilder sql = new StringBuilder($"UPDATE TblFruttiSerie SET IdFrutto = @IdFrutto, IdSerie = @IdSerie, CodiceListinoUnivoco = @CodiceListinoUnivoco WHERE IdFruttoSerie = @IdFruttoSerie");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql.ToString(), fruttoSerie);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Update in FruttiSerieDAO", ex);
            }
        }

        public static void Delete(long idFruttoSerie)
        {
            StringBuilder sql = new StringBuilder($"DELETE FROM TblFruttiSerie WHERE IdFruttoSerie = @idFruttoSerie");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    cn.Execute(sql.ToString(), new { idFruttoSerie });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la Delete in FruttiSerieDAO", ex);
            }
        }
    }
}