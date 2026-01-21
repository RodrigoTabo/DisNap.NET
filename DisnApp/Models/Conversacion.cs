using System.ComponentModel.DataAnnotations;

namespace DisnApp.Models
{
    public class Conversacion
    {
        public int Id { get; set; }
        [Required]
        public DateTime FechaCreacion { get; set; }
        public DateTime? UltimaActividad { get; set; }
        public List<ConversacionUsuario> Participantes { get; set; } = new();
        public List<Mensaje> Mensajes { get; set; } = new();
    }
}
