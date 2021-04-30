using System;

namespace Database.Models
{
    public class Fattura
    {
        public long IdFatture { get; set; } = 0;
        public int IdClienti { get; set; } = 0;
        public long? IdAmministratori { get; set; } = 0;
        public int Numero { get; set; } = 0;
        public DateTime Data { get; set; } = DateTime.Now;
        public double Imponibile { get; set; } = 0;
        public int Iva { get; set; } = 0;
        public double RitenutaAcconto { get; set; } = 0;
        public bool ReverseCharge { get; set; } = false;
        public bool Riscosso { get; set; } = false;
        public bool IsNotaDiCredito { get; set; } = false;
        public string Cantieri { get; set; } = "";
        public string Acconti { get; set; } = "";
        public string RagioneSocialeCliente { get; set; } = "";
        public string NomeAmministratore { get; set; }
    }
}