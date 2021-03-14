namespace GestioneCantieri.Data
{
    public class DefaultMatOrdFrut
    {
        public long IdTblDefaultMatOrdFrut { get; set; } = 0;
        public int? IdGruppiFrutti { get; set; } = null;
        public int IdLocale { get; set; } = 0;
        public int? IdFrutto { get; set; } = null;
        public long? IdSerie { get; set; } = null;
        public int QtaFrutti { get; set; } = 0;
        public string NomeLocale { get; set; } = "";
        public string NomeGruppo { get; set; } = "";
        public string Descrizione { get; set; } = "";
        public string NomeFrutto { get; set; } = "";
        public string NomeSerie { get; set; } = "";
    }
}