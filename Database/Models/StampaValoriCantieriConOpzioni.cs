using System;

namespace Database.Models
{
    public class StampaValoriCantieriConOpzioni
    {
        public int IdCantieri { get; set; } = 0;
        public int IdtblClienti { get; set; } = 0;
        public int Ricarico { get; set; } = 0;
        public int Iva { get; set; } = 0;
        public int Anno { get; set; } = 0;
        public int FasciaTblCantieri { get; set; } = 0;
        public int Numero { get; set; } = 0;
        public string CodCant { get; set; } = "";
        public string DescriCodCAnt { get; set; } = "";
        public string Indirizzo { get; set; } = "";
        public string Città { get; set; } = "";
        public string RagSocCli { get; set; } = "";
        public string CodRiferCant { get; set; } = "";
        public decimal PzzoManodopera { get; set; } = 0;
        public decimal ValorePreventivo { get; set; } = 0;
        public decimal TotaleConto { get; set; } = 0;
        public decimal TotaleAcconti { get; set; } = 0;
        public decimal TotaleFinale { get; set; } = 0;
        public bool Chiuso { get; set; } = false;
        public bool Riscosso { get; set; } = false;
        public bool Preventivo { get; set; } = false;
        public bool DaDividere { get; set; } = false;
        public bool Diviso { get; set; } = false;
        public bool Fatturato { get; set; } = false;
        public DateTime Data { get; set; } = DateTime.Now;
    }
}