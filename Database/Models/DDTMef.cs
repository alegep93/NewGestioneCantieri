using System;

namespace Database.Models
{
    public class DDTMef
    {
        public int IdDDTMef { get; set; } = 0;
        public int Anno { get; set; } = 0;
        public DateTime Data { get; set; } = DateTime.Now;
        public long N_DDT { get; set; } = 0;
        public string CodArt { get; set; } = "";
        public string DescriCodArt { get; set; } = "";
        public int Qta { get; set; } = 0;
        public decimal Importo { get; set; } = 0;
        public string Acquirente { get; set; } = "";
        public decimal PrezzoUnitario { get; set; } = 0;
        public int AnnoN_ddt { get; set; } = 0;
        public int IdFornitore { get; set; } = 0;
        public string FTVRF0 { get; set; } = "";
        public string FTDT30 { get; set; } = "";
        public string FTAIN { get; set; } = "";
        public string DescrizioneArticolo2 { get; set; } = "";
        public int Iva { get; set; } = 0;
        public decimal PrezzoListino { get; set; } = 0;
        public DateTime Data2 { get; set; } = DateTime.Now;
        public string Valuta { get; set; } = "";
        public string FTFOM { get; set; } = "";
        public string FTCMA { get; set; } = "";
        public string FTCDO { get; set; } = "";
        public string FLFLAG { get; set; } = "";
        public string FLFLQU { get; set; } = "";
        public DateTime Data3 { get; set; } = DateTime.Now;
        public string FTORAG { get; set; } = "";
        public decimal Importo2 { get; set; } = 0;
        public string FTIMRA { get; set; } = "";
        public string FTMLT0 { get; set; } = "";
        public decimal Valore { get; set; } = 0;
        public bool DaInserire { get; set; } = false;
        public decimal MediaPrezzoUnitario { get; set; } = 0;
    }
}