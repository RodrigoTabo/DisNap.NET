namespace DisnApp.Models
{
    public class ConversacionUsuario
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public int ConversacionId { get; set; }
        public Conversacion Conversacion { get; set; }

        public DateTime? UltimaLectura { get; set; }
        public bool Eliminada { get; set; }
        public DateTime? EliminadaAt { get; set; }

    }
}
