using DisnApp.Models;
using System.ComponentModel.DataAnnotations;

namespace DisnApp.ViewModel
{
    public class ChatVM
    {
        [Required]

        public string EmisorId { get; set; }
        public Usuario Emisor { get; set; }

        [Required, StringLength(2000)]
        public string Texto { get; set; }

        [Required]
        public DateTime FechaEnvio { get; set; }


    }
}
