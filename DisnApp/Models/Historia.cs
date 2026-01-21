using System.ComponentModel.DataAnnotations;

namespace DisnApp.Models
{
    public class Historia
    {
        public int Id { get; set; }
        [Required]
        public string UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        [Required, StringLength(1000)]
        public string UrlImagen { get; set; }   
        public DateTime FechaExpiracion { get; set; }
        public List<HistoriaVista> HistoriaVista { get; set; } = new();
    }
}
