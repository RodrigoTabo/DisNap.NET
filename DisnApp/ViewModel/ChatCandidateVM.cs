namespace DisnApp.ViewModel
{
    public class ChatCandidateVM
    {
        public string UsuarioId { get; set; }
        public string Nombre { get; set; }

        public bool TieneConversacion { get; set; }
        public int? ConversacionId { get; set; }   // null si no existe
    }

}
