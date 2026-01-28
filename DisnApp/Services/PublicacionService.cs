using DisnApp.Data;
using DisnApp.Models;
using DisnApp.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DisnApp.Services
{
    public class PublicacionService : IPublicacionService
    {

        private readonly RedDbContext _context;
        public PublicacionService(RedDbContext context)
        {
            _context = context;
        }


        public async Task<Publicacion?> GetDetailsAsync(int id)
        {
            return await _context.Publicaciones
                .Include(p => p.Usuario)
                .Include(p => p.Likes)
                .Include(p => p.Comentarios).ThenInclude(c => c.Usuario)
                .FirstOrDefaultAsync(p => p.Id == id && !p.Eliminada);
        }

        public async Task<Publicacion> CreateAsync(CrearPublicacionVM vm, string userId)
        {
            var nueva = new Publicacion
            {
                UsuarioId = userId,
                Descripcion = vm.Descripcion,
                UrlImagen = vm.UrlImagen,
                FechaSubida = DateTime.Now
            };

            await _context.Publicaciones.AddAsync(nueva);
            await _context.SaveChangesAsync();

            return nueva;

        }

        public async Task<Publicacion> SoftDeleteAsync(int id, string userId)
        {
            var publicacion = await _context.Publicaciones
                    .FirstOrDefaultAsync(p => p.Id == id);

            if (publicacion == null)
                return null;

            if (publicacion.UsuarioId != userId)
                throw new UnauthorizedAccessException();

            publicacion.Eliminada = true;
            await _context.SaveChangesAsync();

            return publicacion;
        }


        public async Task<PublicacionLike> ToggleLikeAsync(int id, string userId)
        {
            var usuarioId = userId;
            var likeExistente = await _context.PublicacionLikes
                .FirstOrDefaultAsync(l => l.PublicacionId == id && l.UsuarioId == usuarioId);

            if (likeExistente != null)
            {
                _context.PublicacionLikes.Remove(likeExistente);
                await _context.SaveChangesAsync();
                return likeExistente;
            }
            else
            {
                var nuevoLike = new PublicacionLike
                {
                    PublicacionId = id,
                    UsuarioId = usuarioId,
                    FechaLike = DateTime.Now
                };
                await _context.PublicacionLikes.AddAsync(nuevoLike);
                await _context.SaveChangesAsync();
                return nuevoLike;
            }
        }


        public async Task<Comentario> AddCommentAsync(int id, string contenido, string userId)
        {
            var nuevoComentario = new Comentario
            {
                PublicacionId = id,
                UsuarioId = userId,
                Contenido = contenido,
                FechaComentario = DateTime.Now
            };

            await _context.Comentarios.AddAsync(nuevoComentario);
            await _context.SaveChangesAsync();

            return nuevoComentario;

        }

        public Task<List<Comentario>> GetComentariosAsync(int publicacionId)
        {
            return _context.Comentarios
                .Where(c => c.PublicacionId == publicacionId)
                .Include(c => c.Usuario)
                .OrderByDescending(c => c.FechaComentario)
                .ToListAsync();
        }



    }
}
