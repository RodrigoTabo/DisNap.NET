using DisnApp.Data;
using DisnApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DisnApp.Services
{
    public class HomeService : IHomeService
    {

        private readonly RedDbContext _context;

        public HomeService(RedDbContext context)
        {
            _context = context;
        }

        public async Task<List<Publicacion>> GetPublicacionAsync()
        {

            return await _context.Publicaciones
                .AsNoTracking()
                .Include(p => p.Usuario)
                .Include(p => p.Likes)
                .Include(p => p.Comentarios)
                .Where(p => !p.Eliminada)
                .OrderByDescending(p => p.FechaSubida)
                .ToListAsync();
        }

        public async Task<List<Historia>> GetHistoriaAsync()
        {
            return await _context.Historias
                .AsNoTracking()
                .Include(h => h.Usuario)
                .Where(h => h.FechaExpiracion > DateTime.UtcNow)
                .OrderByDescending(h => h.FechaCreacion)
                .ToListAsync();

        }


    }
}
