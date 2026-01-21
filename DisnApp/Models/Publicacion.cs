using System.ComponentModel.DataAnnotations;

namespace DisnApp.Models
{
    public class Publicacion
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        [Required, StringLength(500)]
        public string Descripcion { get; set; }
        [Required, StringLength(500)]
        public string UrlImagen { get; set; }
        public DateTime FechaSubida { get; set; }
        public List<PublicacionLike> Likes { get; set; } = new();
        public List<Comentario> Comentarios { get; set; } = new();
        public bool Eliminada { get; set; } = false;

    }
}
