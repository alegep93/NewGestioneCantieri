using System;

namespace Database.Models
{
    public class MaterialiCantieri
    {
        public int IdMaterialiCantiere { get; set; } = 0;
        public int IdTblCantieri { get; set; } = 0;
        public int IdTblOperaio { get; set; } = 0;
        public string DescriMateriali { get; set; } = "";
        public double Qta { get; set; } = 0;
        public bool Visibile { get; set; } = false;
        public bool Ricalcolo { get; set; } = false;
        public bool RicaricoSiNo { get; set; } = false;
        public DateTime Data { get; set; } = DateTime.Now;
        public decimal PzzoUniCantiere { get; set; } = 0;
        public string CodArt { get; set; } = "";
        public string DescriCodArt { get; set; } = "";
        public string Tipologia { get; set; } = ""; 
        public int Fascia { get; set; } = 0;
        public string Acquirente { get; set; } = "";
        public string Fornitore { get; set; } = "";
        public string NumeroBolla { get; set; } = "";
        public int ProtocolloInterno { get; set; } = 0;
        public string Note { get; set; } = "";
        public string Note2 { get; set; } = "";
        public decimal PzzoFinCli { get; set; } = 0;
        public bool OperaioPagato { get; set; } = false;
        public string CodCant { get; set; } = "";
        public string DescriCodCant { get; set; } = "";
        public string RagSocCli { get; set; } = "";
        public decimal Valore { get; set; } = 0;
        public decimal CostoOperaio { get; set; } = 0;
        public decimal ValoreRicarico { get; set; } = 0;
        public decimal ValoreRicalcolo { get; set; } = 0;
        public bool Rientro { get; set; } = false;
    }
}