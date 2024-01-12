﻿using System;

namespace Database.Models
{
    public class FatturaAcquisto
    {
        public long IdFattureAcquisto { get; set; } = 0;
        public int IdFornitore { get; set; } = 0;
        public int Numero { get; set; } = 0;
        public DateTime Data { get; set; } = DateTime.Now;
        public double Imponibile { get; set; } = 0;
        public int Iva { get; set; } = 0;
        public double RitenutaAcconto { get; set; } = 0;
        public bool ReverseCharge { get; set; } = false;
        public bool IsNotaDiCredito { get; set; } = false;
        public string FilePath { get; set; } = "";
        public string RagioneSocialeFornitore { get; set; } = "";
    }
}