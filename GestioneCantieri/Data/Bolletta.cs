using System;

namespace GestioneCantieri.Data
{
    public class Bolletta
    {
        public long IdBollette { get; set; } = 0;
        public long IdFornitori { get; set; } = 0;
        public string RagSocForni { get; set; } = "";
        public DateTime DataBolletta { get; set; } = DateTime.Now;
        public DateTime DataScadenza { get; set; } = DateTime.Now;
        public DateTime DataPagamento { get; set; } = DateTime.Now;
        public decimal TotaleBolletta { get; set; } = 0;
        public int Progressivo { get; set; } = 0;
    }
}