using Dapper;
using Database.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Database.DAO
{
    public class DefaultMatOrdFrutDAO : BaseDAO
    {
        public static List<DefaultMatOrdFrut> GetDefaultLocale(int idLocale)
        {
            List<DefaultMatOrdFrut> ret = new List<DefaultMatOrdFrut>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT C.NomeGruppo, C.Descrizione, B.NomeLocale, D.Nome AS NomeFrutto, A.QtaFrutti, E.NomeSerie");
            sql.AppendLine($"FROM TblDefaultMatOrdFrut A");
            sql.AppendLine($"LEFT JOIN TblLocali B ON A.IdLocale = B.IdLocali");
            sql.AppendLine($"LEFT JOIN TblGruppiFrutti C ON A.IdGruppiFrutti = C.Id");
            sql.AppendLine($"LEFT JOIN TblFrutti D ON A.IdFrutto = D.IdFrutti");
            sql.AppendLine($"LEFT JOIN TblSerie E ON A.IdSerie = E.IdSerie");
            sql.AppendLine($"WHERE IdLocale = @idLocale");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<DefaultMatOrdFrut>(sql.ToString(), new { idLocale }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la GetDefaultLocale in DefaultMatOrdFrutDAO", ex);
            }
            return ret;
        }

        public static bool InserisciGruppo(DefaultMatOrdFrut item)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO TblDefaultMatOrdFrut(IdGruppiFrutti, IdLocale, IdFrutto, QtaFrutti, IdSerie)");
            sql.AppendLine("VALUES (@IdGruppiFrutti, @IdLocale, @IdFrutto, @QtaFrutti, @IdSerie)");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), item) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la InserisciGruppo in OrdineFruttiDAO", ex);
            }
            return ret;
        }
    }
}