namespace DisnApp.ViewModel
{
    public class ConversacionVM
    {
        public int Id { get; set; }

        public int ConversacionId { get; set; }
        public string OtroUsuarioId { get; set; }
        public string OtroUsuarioNombre { get; set; }
        public string? UltimoTexto { get; set; }
        public DateTime? UltimaActividad { get; set; }
        public int NoLeidos { get; set; } 

    }

}
