using System;

namespace GestioneCantieri.Data
{
    public class DDTFornitori
    {
        public int Id { get; set; } = 0;
        public int IdFornitore { get; set; } = 0;
        public DateTime Data { get; set; } = DateTime.Now;
        public long Protocollo { get; set; } = 0;
        public string NumeroDDT { get; set; } = "";
        public string Articolo { get; set; } = "";
        public string DescrizioneFornitore { get; set; } = "";
        public string DescrizioneMau { get; set; } = "";
        public int Qta { get; set; } = 0;
        public decimal Valore { get; set; } = 0;
        public string RagSocFornitore { get; set; } = "";
        public decimal PrezzoUnitario { get; set; } = 0;
    }
}