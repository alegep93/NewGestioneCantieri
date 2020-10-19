using Dapper;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GestioneCantieri.DAO
{
    public class DDTFornitoriDAO : BaseDAO
    {
        // SELECT
        public static List<DDTFornitori> GetAllDDT()
        {
            List<DDTFornitori> ret = new List<DDTFornitori>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT A.Id, B.RagSocForni 'ragSocFornitore', A.Data, A.Protocollo, A.NumeroDDT, A.Articolo, A.DescrizioneFornitore, A.DescrizioneMau, A.Qta, A.Valore");
            sql.AppendLine($"FROM TblDDTFornitori AS A");
            sql.AppendLine($"INNER JOIN TblForitori AS B ON A.IdFornitore = B.IdFornitori");
            sql.AppendLine($"ORDER BY B.RagSocForni, A.Data, A.Protocollo");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<DDTFornitori>(sql.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero dell'elenco dei DDT dei Fornitori", ex);
            }
            return ret;
        }

        public static List<DDTFornitori> GetAllDDT(DDTFornitori filters)
        {
            List<DDTFornitori> ret = new List<DDTFornitori>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT A.Id, B.RagSocForni 'ragSocFornitore', A.Data, A.Protocollo, A.NumeroDDT, A.Articolo, A.DescrizioneFornitore, A.DescrizioneMau, A.Qta, A.Valore");
            sql.AppendLine($"FROM TblDDTFornitori AS A");
            sql.AppendLine($"INNER JOIN TblForitori AS B ON A.IdFornitore = B.IdFornitori");
            sql.AppendLine($"WHERE A.NumeroDDT LIKE '%{filters.NumeroDDT}%' AND A.Articolo LIKE '%{filters.Articolo}%'");
            sql.AppendLine($"AND A.DescrizioneFornitore LIKE '%{filters.DescrizioneFornitore}%' AND A.DescrizioneMau LIKE '%{filters.DescrizioneMau}%'");
            sql.AppendLine(filters.IdFornitore != -1 ? $"AND A.IdFornitore = @IdFornitore" : "");
            sql.AppendLine(filters.Protocollo != -1 ? $"AND A.Protocollo = @Protocollo" : "");
            sql.AppendLine(filters.Qta != -1 ? $"AND A.Qta = @Qta" : "");
            sql.AppendLine($"ORDER BY B.RagSocForni, A.Data, A.Protocollo");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<DDTFornitori>(sql.ToString(), filters).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero dell'elenco filtrato dei DDT dei Fornitori", ex);
            }
            return ret;
        }

        public static DDTFornitori GetDDT(int id)
        {
            DDTFornitori ret = new DDTFornitori();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT Id, IdFornitore, Data, Protocollo, NumeroDDT, Articolo, DescrizioneFornitore, DescrizioneMau, Qta, Valore");
            sql.AppendLine($"FROM TblDDTFornitori");
            sql.AppendLine($"WHERE Id = @id");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Query<DDTFornitori>(sql.ToString(), new { id }).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore durante il recupero del DDT Fornitori con id = {id}", ex);
            }
            return ret;
        }

        // INSERT
        public static bool InsertNewFornitore(DDTFornitori ddt)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"INSERT INTO TblDDTFornitori (IdFornitore, Data, Protocollo, NumeroDDT, Articolo, DescrizioneFornitore, DescrizioneMau, Qta, Valore)");
            sql.AppendLine($"VALUES (@IdFornitore, @Data, @Protocollo, @NumeroDDT, @Articolo, @DescrizioneFornitore, @DescrizioneMau, @Qta, @Valore)");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), ddt) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'inserimento di un nuovo DDT Fornitore ", ex);
            }
            return ret;
        }

        // UPDATE
        public static bool UpdateDDTFornitore(DDTFornitori ddt)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"UPDATE TblDDTFornitori SET IdFornitore = @IdFornitore, Data = @Data, Protocollo = @Protocollo, NumeroDDT = @NumeroDDT, Articolo = @Articolo,");
            sql.AppendLine($"DescrizioneFornitore = @DescrizioneFornitore, DescrizioneMau = @DescrizioneMau, Qta = @Qta, Valore = @Valore");
            sql.AppendLine($"WHERE Id = @Id");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), ddt) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore durante l'aggiornamento del DDT Fornitore {ddt.Id}", ex);
            }
            return ret;
        }

        // DELETE
        public static bool DeleteDDTFornitore(int id)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder("DELETE FROM TblDDTFornitori WHERE Id = @id");
            try
            {
                using (SqlConnection cn = GetConnection())
                {
                    ret = cn.Execute(sql.ToString(), new { id }) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore durante l'eliminazione del DDT Fornitore con id = {id}", ex);
            }
            return ret;
        }
    }
}