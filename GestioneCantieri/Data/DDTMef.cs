using System;

namespace GestioneCantieri.Data
{
    public class DDTMef
    {
        int id = 0, anno = 0, n_ddt = 0, qta = 0, annoN_ddt = 0, idFornitore = 0;
        DateTime data = new DateTime();
        string codArt = "", descriCodArt = "", acquirente = "";
        decimal importo = 0, prezzoUnitario = 0, valore = 0;
        bool daInserire = false;

        public int Id { get => id; set => id = value; }
        public int Anno { get => anno; set => anno = value; }
        public int N_ddt { get => n_ddt; set => n_ddt = value; }
        public int Qta { get => qta; set => qta = value; }
        public int AnnoN_ddt { get => annoN_ddt; set => annoN_ddt = value; }
        public int IdFornitore { get => idFornitore; set => idFornitore = value; }
        public DateTime Data { get => data; set => data = value; }
        public string CodArt { get => codArt; set => codArt = value; }
        public string DescriCodArt { get => descriCodArt; set => descriCodArt = value; }
        public string Acquirente { get => acquirente; set => acquirente = value; }
        public decimal Importo { get => importo; set => importo = value; }
        public decimal PrezzoUnitario { get => prezzoUnitario; set => prezzoUnitario = value; }
        public decimal Valore { get => valore; set => valore = value; }
        public bool DaInserire { get => daInserire; set => daInserire = value; }


        public string FTVRF0 { get; set; } = "";
        public string FTDT30 { get; set; } = "";
        public string FTAIN { get; set; } = "";
        public string DescrizioneArticolo2 { get; set; } = "";
        public int Iva { get; set; } = 0;
        public decimal PrezzoListino { get; set; } = 0;
        public DateTime Data2 { get; set; } = new DateTime();
        public string Valuta { get; set; } = "";
        public string FTFOM { get; set; } = "";
        public string FTCMA { get; set; } = "";
        public string FTCDO { get; set; } = "";
        public string FLFLAG { get; set; } = "";
        public string FLFLQU { get; set; } = "";
        public DateTime Data3 { get; set; } = new DateTime();
        public string FTORAG { get; set; } = "";
        public decimal Importo2 { get; set; } = 0;
        public string FTIMRA { get; set; } = "";
        public string FTMLT0 { get; set; } = "";
    }
}