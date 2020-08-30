﻿using System;

namespace GestioneCantieri.Data
{
    public class MatOrdFrut
    {
        public int Id { get; set; } = 0;
        public int IdCantiere { get; set; } = 0;
        public int IdGruppiFrutti { get; set; } = 0;
        public int IdLocali { get; set; } = 0;
        public int IdFrutto { get; set; } = 0;
        public int QtaFrutti { get; set; } = 0;
        public DateTime DataOrdine { get; set; } = DateTime.Now;
        public string Appartamento { get; set; } = "";
        public string NomeGruppo { get; set; } = "";
        public string Descrizione { get; set; } = "";
        public string DescrCant { get; set; } = "";
        public string NomeFrutto { get; set; } = "";
        public int IdTblMatOrdFrutGroup { get; set; } = 0;
        public string DescrizioneGruppoOrdine { get; set; } = "";
    }
}