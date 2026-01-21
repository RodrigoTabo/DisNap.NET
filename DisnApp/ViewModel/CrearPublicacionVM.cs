using System.ComponentModel.DataAnnotations;

namespace DisnApp.ViewModel
{
    public class CrearPublicacionVM
    {
        [Required]
        public string Descripcion { get; set; }
        public string? UrlImagen { get; set; }
    }
}
