using DisnApp.Models;

namespace DisnApp.Services
{
    public interface IHomeService
    {
        Task<List<Publicacion>> GetPublicacionAsync();
        Task<List<Historia>> GetHistoriaAsync();

    }
}
