namespace GestioneCantieri.Data
{
    public class CompGruppoFrut
    {
        public int Id { get; set; } = 0;
        public int IdTblGruppo { get; set; } = 0;
        public int IdTblFrutto { get; set; } = 0;
        public int Qta { get; set; } = 0;
        public string NomeFrutto { get; set; } = "";
    }
}