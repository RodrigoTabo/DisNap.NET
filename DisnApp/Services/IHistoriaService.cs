using DisnApp.Models;
using DisnApp.ViewModel;

namespace DisnApp.Services
{
    public interface IHistoriaService
    {
        Task<Historia> CreateAsync(CrearHistoriaVM vm, string userId);
        Task<Historia> DeleteAsync(int id, string userId);
        Task<List<Historia>> GetViewerAsync();

    }
}
