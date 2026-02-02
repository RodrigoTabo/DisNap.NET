using DisnApp.Data;
using DisnApp.Models;
using DisnApp.ViewModel;
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



        public async Task<Historia> CreateAsync (CrearHistoriaVM vm, string userId)
        {

            var ahora = DateTime.UtcNow;

            var nueva = new Historia
            {
                UsuarioId = userId,
                FechaCreacion = ahora,
                FechaExpiracion = ahora.AddHours(24),
                UrlImagen = vm.UrlImagen
            };

            await _context.Historias.AddAsync(nueva);
            await _context.SaveChangesAsync();

            return nueva;
        }

        public async Task<Historia> DeleteAsync(int id, string userId)
        {
            var historia = await _context.Historias
                    .FirstOrDefaultAsync(p => p.Id == id);

            if (historia == null)
                return null;

            if (historia.UsuarioId != userId)
                throw new UnauthorizedAccessException();

            _context.Historias.Remove(historia);
            await _context.SaveChangesAsync();

            return historia;
        }

        public async Task<List<Historia>> GetViewerAsync(string miId)
        {
            var ahora = DateTime.UtcNow;

            return await _context.Historias
                .Include(h => h.Usuario) // OJO: debe ser la navegación, no el Id
                .Where(h => h.FechaExpiracion > ahora)
                .Where(h =>
                    h.UsuarioId == miId ||
                    _context.SeguidorUsuarios.Any(s =>
                        s.SeguidorId == miId &&
                        s.SeguidoId == h.UsuarioId
                    )
                )
                .OrderByDescending(h => h.UsuarioId == miId)
                .ThenByDescending(h => h.FechaCreacion)
                .OrderByDescending(h => h.FechaCreacion)
                .ToListAsync();
        }


    }
}
