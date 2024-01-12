
namespace Database.Models
{
    public class StampaOrdFrutCantLoc
    {
        public int Qta { get; set; } = 0;
        public string Nome { get; set; } = "";
        public string NomeLocale { get; set; } = "";
        public string NomeGruppo { get; set; } = "";
        public string DescrizioneGruppoOrdine { get; set; } = "";
        public string ArticoloSerie { get; set; } = "";
        public string DescrizioneSerie { get; set; } = "";
        public float PrezzoNetto { get; set; } = 0;
        public float Valore { get; set; } = 0;
    }
}