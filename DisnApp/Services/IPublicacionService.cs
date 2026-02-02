using DisnApp.Models;
using DisnApp.ViewModel;

namespace DisnApp.Services
{
    public interface IPublicacionService
    {

        Task<Publicacion> GetDetailsAsync(int id);
        Task<Publicacion> CreateAsync(CrearPublicacionVM vm, string userId);

        Task<Publicacion> SoftDeleteAsync(int id, string userId);
        Task<(bool liked, int likeCount)> ToggleLikeAsync(int id, string userId);
        Task<Comentario> AddCommentAsync(int id, string contenido, string userId);
        Task<List<Comentario>> GetComentariosAsync(int publicacionId);

    }
}
