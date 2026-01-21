namespace DisnApp.Models
{
    public class SeguidorUsuario
    {
        public int Id { get; set; }
        public string SeguidorId { get; set; } = string.Empty;
        public Usuario Seguidor { get; set; }

        public string SeguidoId { get; set; } = string.Empty;
        public Usuario Seguido { get; set; }
        public DateTime FechaInicio { get; set; }
    }
}
