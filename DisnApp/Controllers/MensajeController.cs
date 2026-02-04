using DisnApp.Data;
using DisnApp.Models;
using DisnApp.Services;
using DisnApp.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using SQLitePCL;
using System.Security.Claims;

namespace DisnApp.Controllers
{
    public class MensajeController : Controller
    {

        private readonly IMensajeService _mensajeService;
        private readonly UserManager<Usuario> _userManager;
        private readonly RedDbContext _context;


        public MensajeController(RedDbContext context, IMensajeService mensajeService, UserManager<Usuario> userManager)
        {
            _mensajeService = mensajeService;
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Chat(int id)
        {
            if (id <= 0) return NotFound();

            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

            try
            {
                var mensajes = await _mensajeService.GetChatAsync(id, userId);
                ViewBag.ConversacionId = id;
                ViewBag.CurrentUserId = userId;
                return PartialView("_Chat", mensajes);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Chat(int ConversacionId, string texto)
        {

            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

            try
            {
                var mensajes = await _mensajeService.SendMessageAsync(ConversacionId, userId, texto);
                ViewBag.CurrentUserId = userId;
                ViewBag.ConversacionId = ConversacionId;
                return PartialView("_Chat", mensajes);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

        }


        public async Task<IActionResult> GetChatStartCandidates(string txtBusqueda)
        {

            var miId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var items = await _mensajeService.GetBandejaAsync(miId);

            try
            {

                if (string.IsNullOrWhiteSpace(txtBusqueda)) return PartialView("~/Views/Mensaje/_Bandeja.cshtml", items) ;

                var candidatos = await _mensajeService.GetChatStartCandidatesAsync(miId, txtBusqueda);
                return PartialView("_ChatCandidates", candidatos);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrGet(string receptorId)
        {
            var miId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var conversacionId = await _context.ConversacionUsuarios
                .Where(cu => cu.UsuarioId == miId)
                .Select(cu => cu.ConversacionId)
                .FirstOrDefaultAsync(cid =>
                    _context.ConversacionUsuarios.Any(x => x.ConversacionId == cid && x.UsuarioId == receptorId));

            if (conversacionId != 0)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { conversacionId });

                return RedirectToAction("Chat", new { id = conversacionId });
            }

            var conversacion = new Conversacion { FechaCreacion = DateTime.UtcNow };
            _context.Conversaciones.Add(conversacion);
            await _context.SaveChangesAsync();

            _context.ConversacionUsuarios.AddRange(
                new ConversacionUsuario { ConversacionId = conversacion.Id, UsuarioId = miId },
                new ConversacionUsuario { ConversacionId = conversacion.Id, UsuarioId = receptorId }
            );
            await _context.SaveChangesAsync();

            // ✅ devolvé ID, no objeto
            return Json(new { conversacionId = conversacion.Id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConversacion(int conversacionId)
        {
            var miId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cu = await _context.ConversacionUsuarios
                .FirstOrDefaultAsync(x => x.ConversacionId == conversacionId && x.UsuarioId == miId);

            if (cu == null) return NotFound();

            cu.Eliminada = true;
            cu.EliminadaAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Si viene por AJAX devolvé OK + opcional JSON
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { ok = true });

            return RedirectToAction("Index");
        }



    }
}
