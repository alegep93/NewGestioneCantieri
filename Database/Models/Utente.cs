namespace Database.Models
{
    public class Utente
    {
        public long IdUtente { get; set; } = 0;
        public long IdTipoUtente { get; set; } = 0;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string Nome { get; set; } = "";
        public string Email { get; set; } = "";
    }
}
