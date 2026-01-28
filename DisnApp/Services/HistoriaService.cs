using DisnApp.Data;
using DisnApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DisnApp.Services
{
    public class HistoriaService : IHistoriaService
    {

        private readonly RedDbContext _context;

        public HistoriaService(RedDbContext context)
        {
            _context = context; 
        }

        public async Task<List<Historia>> GetViewerAsync()
        {
            var ahora = DateTime.UtcNow;

            return await _context.Historias
            .Include(h => h.UsuarioId)
            .Where(h => h.FechaExpiracion > ahora)
            .OrderByDescending(h => h.FechaCreacion)
            .ToListAsync();

        } 


    }
}
