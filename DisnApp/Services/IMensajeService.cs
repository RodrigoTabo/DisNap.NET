using DisnApp.Models;
using DisnApp.ViewModel;

namespace DisnApp.Services
{
    public interface IMensajeService
    {

        Task<List<Mensaje>> GetChatAsync(int conversacionId, string userId);
        Task<List<Mensaje>> SendMessageAsync(int conversacionId, string userId, string texto);
        Task<List<ConversacionVM>> GetBandejaAsync(string userId);

    }
}
