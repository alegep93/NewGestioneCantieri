
namespace GestioneCantieri.Data
{
    public class Fornitori
    {
        public int IdFornitori { get; set; } = 0;
        public string RagSocForni { get; set; } = "";
        public string Indirizzo { get; set; } = "";
        public string Cap { get; set; } = "";
        public string Città { get; set; } = "";
        public int Tel1 { get; set; } = 0;
        public int Cell1 { get; set; } = 0;
        public double PartitaIva { get; set; } = 0;
        public string CodFiscale { get; set; } = "";
        public string Abbreviato { get; set; } = "";
    }
}