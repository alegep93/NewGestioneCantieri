using System;

namespace GestioneCantieri.Data
{
    public class Pagamenti
    {
        public int IdPagamenti { get; set; } = 0;
        public int IdTblCantieri { get; set; } = 0;
        public DateTime Data { get; set; } = DateTime.Now;
        public decimal Imporo { get; set; } = 0;
        public string DescriPagamenti { get; set; } = "";
        public string CodCant { get; set; } = "";
        public string DescriCodCant { get; set; } = "";
        public bool Acconto { get; set; } = false;
        public bool Saldo { get; set; } = false;
    }
}