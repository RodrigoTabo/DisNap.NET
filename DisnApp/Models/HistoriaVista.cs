namespace DisnApp.Models
{
    // Registro de "vistas": un usuario vio una historia en un momento dado.
    // Sirve para contar vistas y para saber quién ya la vio.
    public class HistoriaVista
    {
        public int Id { get; set; }

        // Usuario que vio la historia (FK a AspNetUsers)
        public string UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        // Historia vista (FK a Historias)
        public int HistoriaId { get; set; }
        public Historia Historia { get; set; }

        public DateTime FechaVista { get; set; }

    }
}
