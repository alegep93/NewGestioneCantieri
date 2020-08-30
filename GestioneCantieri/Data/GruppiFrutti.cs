
namespace GestioneCantieri.Data
{
    public class GruppiFrutti
    {
        public int Id { get; set; } = 0;
        public string NomeGruppo { get; set; } = "";
        public string Descrizione { get; set; } = "";
        public bool Completato { get; set; } = false;
        public bool Controllato { get; set; } = false;
    }
}