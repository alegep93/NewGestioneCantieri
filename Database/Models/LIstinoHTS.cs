using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Database.Models
{
    public class ListinoHts
    {
        public long Id { get; set; } = 0;
        public string Codice { get; set; } = "";
        public string CodiceProdotto { get; set; } = "";
        public string Descrizione { get; set; } = "";
        public decimal Prezzo { get; set; } = 0;
        public string Cr { get; set; } = "";
        public string G { get; set; } = "";
        public string NoteDisponibilita { get; set; } = "";

    }
}