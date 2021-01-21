namespace GestioneCantieri.Data
{
    public class FruttoSerie
    {
        public long IdFruttoSerie { get; set; } = 0;
        public int IdFrutto { get; set; } = 0;
        public long IdSerie { get; set; } = 0;
        public string CodiceListinoUnivoco { get; set; } = "";

        public string NomeSerie { get; set; } = "";
        public string NomeFrutto { get; set; } = "";
        public string CodiceListino { get; set; } = "";
        public string DescrizioneListino { get; set; } = "";
    }
}