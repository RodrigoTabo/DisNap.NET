using System.ComponentModel.DataAnnotations;

namespace DisnApp.Models
{
    public class Mensaje
    {
        public int Id { get; set; }

        [Required]
        public int ConversacionId { get; set; }
        public Conversacion Conversacion { get; set; }

        [Required]
        public string EmisorId { get; set; }
        public Usuario Emisor { get; set; }

        [Required, StringLength(2000)]
        public string Texto { get; set; }

        [Required]
        public DateTime FechaEnvio { get; set; }

        public DateTime? ReadAt { get; set; } // ✅ visto / leído por el otro


    }
}
