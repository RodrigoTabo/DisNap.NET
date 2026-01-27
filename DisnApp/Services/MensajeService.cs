using DisnApp.Data;
using DisnApp.Models;
using DisnApp.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace DisnApp.Services
{
    public class MensajeService : IMensajeService
    {


        private readonly RedDbContext _context;

        public MensajeService(RedDbContext context)
        {
            _context = context;
        }



        public async Task<List<Mensaje>> GetChatAsync(int conversacionId, string userId)
        {

            if (conversacionId <= 0) throw new ArgumentException("Conversación inválida.");

            //Seguridad: El usuario debe ser el participante.
            var esParticipante = await _context.ConversacionUsuarios
                .AnyAsync(cu => cu.ConversacionId == conversacionId && cu.UsuarioId == userId);


            if (!esParticipante) throw new UnauthorizedAccessException("No sos participante");


            await _context.Mensajes
                .Where(m => m.ConversacionId == conversacionId &&
                m.EmisorId != userId &&
                m.ReadAt == null)
                .ExecuteUpdateAsync(setters => setters
                .SetProperty(m => m.ReadAt, DateTime.UtcNow));


            return await _context.Mensajes
                .Where(m => m.ConversacionId == conversacionId)
                .OrderBy(m => m.FechaEnvio) // o el campo que uses
                .ToListAsync();


        }

        public async Task<List<Mensaje>> SendMessageAsync(int conversacionId, string userId, string texto)
        {

            if (conversacionId <= 0) throw new ArgumentException("Conversación inválida.");

            //Seguridad: El usuario debe ser el participante.
            var esParticipante = await _context.ConversacionUsuarios
                .AnyAsync(cu => cu.ConversacionId == conversacionId && cu.UsuarioId == userId);


            if (!esParticipante) throw new UnauthorizedAccessException("No sos participante");

            var msg = new Mensaje
            {
                ConversacionId = conversacionId,
                EmisorId = userId,
                Texto = texto,
                FechaEnvio = DateTime.UtcNow
            };

            _context.Mensajes.Add(msg);

            var conv = await _context.Conversaciones.FirstOrDefaultAsync(c => c.Id == conversacionId);
            if (conv != null)
            {
                conv.UltimaActividad = DateTime.UtcNow;
            }


            await _context.SaveChangesAsync();

            return await _context.Mensajes
                .Where(m => m.ConversacionId == conversacionId)
                .OrderBy(m => m.FechaEnvio)
                .AsNoTracking()
                .ToListAsync();

        }

        public async Task<List<ConversacionVM>> GetBandejaAsync(string userId)
        {
            return await _context.Conversaciones
                .Where(c => c.Participantes.Any(p => p.UsuarioId == userId))
                .Select(c => new ConversacionVM
                {
                    ConversacionId = c.Id,
                    OtroUsuarioId = c.Participantes
                        .Where(p => p.UsuarioId != userId)
                        .Select(p => p.UsuarioId)
                        .FirstOrDefault(),
                    OtroUsuarioNombre = c.Participantes
                        .Where(p => p.UsuarioId != userId)
                        .Select(p => p.Usuario.UserName)
                        .FirstOrDefault(),
                    NoLeidos = c.Mensajes.Count(m => m.EmisorId != userId && m.ReadAt == null),
                    UltimoTexto = c.Mensajes
                        .OrderByDescending(m => m.FechaEnvio)
                        .Select(m => m.Texto)
                        .FirstOrDefault(),
                    UltimaActividad = c.UltimaActividad ?? c.Mensajes
                        .OrderByDescending(m => m.FechaEnvio)
                        .Select(m => (DateTime?)m.FechaEnvio)
                        .FirstOrDefault()
                })
                .OrderByDescending(x => x.UltimaActividad)
                .AsNoTracking()
                .ToListAsync();
        }

    }
}
