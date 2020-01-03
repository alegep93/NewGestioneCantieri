using System;

namespace GestioneCantieri.Data
{
    public class Clienti
    {
        public int IdCliente { get; set; } = 0;
        public long IdAmministratore { get; set; } = 0;
        public string RagSocCli { get; set; } = "";
        public string Indirizzo { get; set; } = "";
        public string Cap { get; set; } = "";
        public string Città { get; set; } = "";
        public string Tel1 { get; set; } = "";
        public string Cell1 { get; set; } = "";
        public string PartitaIva { get; set; } = "";
        public string CodFiscale { get; set; } = "";
        public string Provincia { get; set; } = "";
        public string Note { get; set; } = "";
        public DateTime Data { get; set; } = DateTime.Now;
    }
}