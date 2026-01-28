using DisnApp.Models;

namespace DisnApp.Services
{
    public interface IUsuarioService
    {
        Task<Usuario> GetDetailsAsync(string id);

        Task<SeguidorUsuario> PostSeguirAsync(string id, string userId);

    }
}
