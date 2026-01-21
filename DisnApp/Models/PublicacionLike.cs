namespace DisnApp.Models
{
    public class PublicacionLike
    {
        public int Id { get; set; }  
        public int PublicacionId { get; set; }
        public Publicacion Publicacion { get; set; }
        public string UsuarioId { get; set; }
        public Usuario Usuario { get; set; }   
        public DateTime FechaLike { get; set; }
    }
}
