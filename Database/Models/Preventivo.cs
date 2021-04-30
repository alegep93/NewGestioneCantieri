using System;

namespace Database.Models
{
    public class Preventivo
    {
        public int Id { get; set; } = 0;
        public long Numero { get; set; } = 0;
        public int Anno { get; set; } = 0;
        public int IdOperaio { get; set; } = 0;
        public string NomeOp { get; set; } = "";
        public string Descrizione { get; set; } = "";
        public DateTime Data { get; set; } = new DateTime();
    }
}