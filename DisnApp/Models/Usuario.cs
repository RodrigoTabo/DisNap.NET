using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DisnApp.Models
{
    public class Usuario : IdentityUser
    {
        [StringLength(50)]
        public string? Nombre { get; set; }
        [StringLength(50)]
        public string? Apellido { get; set; }
        [Required]
        public DateTime FechaRegistro { get; set; }

        public List<Publicacion>? Publicaciones { get; set; } = new();
        public List<Comentario> Comentarios { get; set; } = new();
        public List<PublicacionLike> Likes { get; set; } = new();

        public List<HistoriaVista> HistoriaVista { get; set; } = new();

        [InverseProperty(nameof(SeguidorUsuario.Seguido))]
         public List<SeguidorUsuario> Seguidores { get; set; } = new();

        [InverseProperty(nameof(SeguidorUsuario.Seguidor))]
        public List<SeguidorUsuario> Siguiendo { get; set; } = new();
        public List<Mensaje> MensajesEnviados { get; set; } = new();
        public List<ConversacionUsuario> Conversaciones { get; set; } = new();


    }
}
