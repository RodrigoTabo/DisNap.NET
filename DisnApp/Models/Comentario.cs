using System.ComponentModel.DataAnnotations;

namespace DisnApp.Models
{
    public class Comentario
    {

        public int Id { get; set; }
        [Required]
        public string UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        [Required]
        public int PublicacionId { get; set; }
        public Publicacion Publicacion { get; set; }
        [Required]
        public DateTime FechaComentario { get; set; }
        [Required, StringLength(500)]
        public string Contenido { get; set; }

    }
}
