using System;

namespace GestioneCantieri.Data
{
    public class AccontoOperaio
    {
        public long IdAccontoOperaio { get; set; } = 0;
        public int IdOperaio { get; set; } = 0;
        public string NomeOp { get; set; } = "";
        public DateTime Data { get; set; } = DateTime.Now;
        public decimal Importo { get; set; } = 0;
        public bool Pagato { get; set; } = false;
        public string Descrizione { get; set; } = "";
    }
}