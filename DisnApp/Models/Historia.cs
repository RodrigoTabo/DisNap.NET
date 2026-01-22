using System.ComponentModel.DataAnnotations;

namespace DisnApp.Models
{
    public class Historia
    {
        public int Id { get; set; }

        [Required]
        public string UsuarioId { get; set; } = default!;
        public Usuario Usuario { get; set; } = default!;

        [Required, StringLength(1000)]
        public string UrlImagen { get; set; } = default!;

        public DateTime FechaCreacion { get; set; } 
        public DateTime FechaExpiracion { get; set; }

        public List<HistoriaVista> HistoriaVistas { get; set; } = new();
    }
}
