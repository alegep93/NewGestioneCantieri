namespace GestioneCantieri.Utils
{
    public class Enumeratori
    {
        public enum tipologie : int
        {
            A_CHIAMATA = 1,
            ACCREDITI = 2,
            ARROTONDAMENTO = 3,
            MANODOPERA = 4,
            MATERIALE = 5,
            OPERAIO = 6,
            RIENTRO = 7,
            SPESE = 8
        }

        public enum TipoUtente : long
        {
            Admin = 1
        }
    }
}