using DisnApp.Data;
using DisnApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DisnApp.Services
{
    public class UsuarioService : IUsuarioService
    {

        private readonly RedDbContext _context;

        public UsuarioService(RedDbContext context)
        {
            _context = context;
        }


        public async Task<Usuario> GetDetailsAsync(string id)
        {
            return await _context.Usuarios
                  .Include(u => u.Publicaciones.OrderByDescending(u => u.FechaSubida).Where(p => !p.Eliminada))
                  .Include(u => u.Seguidores)
                  .Include(u => u.Siguiendo)
                  .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<SeguidorUsuario> PostSeguirAsync(string id, string userId)
        {

            var existente = await _context.SeguidorUsuarios
                .FirstOrDefaultAsync(s => s.SeguidorId == userId && s.SeguidoId == id);

            if (existente != null)
            {
                _context.SeguidorUsuarios.Remove(existente);
            }
            else
            {
                var nuevo = new SeguidorUsuario
                {
                    SeguidorId = userId,
                    SeguidoId = id,
                    FechaInicio = DateTime.UtcNow
                };

                await _context.SeguidorUsuarios.AddAsync(nuevo);
            }

            await _context.SaveChangesAsync();

            return existente;

        }


    }
}
