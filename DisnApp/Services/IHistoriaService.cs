using DisnApp.Models;

namespace DisnApp.Services
{
    public interface IHistoriaService
    {
        Task<List<Historia>> GetViewerAsync();
    }
}
