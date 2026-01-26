using DisnApp.Data;
using DisnApp.Models;
using DisnApp.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DisnApp.Controllers
{
    public class MensajeController : Controller
    {

        private readonly RedDbContext _context;
        private readonly UserManager<Usuario> _userManager;


        public MensajeController(RedDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: MensajeController
        public async Task<IActionResult> Index()
        {

            var usuarioActual = await _userManager.GetUserAsync(User);

            var listaMensaje = await _context.Mensajes
                .ToListAsync();

            return View(listaMensaje);
        }

        // GET: MensajeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MensajeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MensajeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MensajeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MensajeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MensajeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MensajeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Chat(int id)
        {
            if (id <= 0) return NotFound();

            var userId = _userManager.GetUserId(User);

            // Marcar como leídos los mensajes del otro usuario en esta conversación
            await _context.Mensajes
                .Where(m => m.ConversacionId == id &&
                            m.EmisorId != userId &&
                            m.ReadAt == null)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.ReadAt, DateTime.UtcNow));


            var mensajes = await _context.Mensajes
                .Where(m => m.ConversacionId == id)
                .OrderBy(m => m.FechaEnvio) // o el campo que uses
                .ToListAsync();



            ViewBag.ConversacionId = id;
            ViewBag.CurrentUserId = userId;
            // devolver vacío en vez de reventar
            return PartialView("_Chat", mensajes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Chat(int ConversacionId, string texto)
        {

            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

            if (ConversacionId <= 0) return BadRequest();

            var esParticipante = await _context.ConversacionUsuarios
                .AnyAsync(cu => cu.ConversacionId == ConversacionId && cu.UsuarioId == userId);

            if (!esParticipante) return Forbid();


            var msg = new Mensaje
            {
                ConversacionId = ConversacionId,
                EmisorId = userId,
                Texto = texto,
                FechaEnvio = DateTime.UtcNow
            };


            var conv = await _context.Conversaciones.FirstOrDefaultAsync(c => c.Id == ConversacionId);
            if (conv != null) conv.UltimaActividad = DateTime.UtcNow;

            _context.Mensajes.Add(msg);

            await _context.SaveChangesAsync();

            var mensajes = await _context.Mensajes
                .Where(m => m.ConversacionId == ConversacionId)
                .OrderBy(m => m.FechaEnvio) // o el campo que uses
                .ToListAsync();



            ViewBag.CurrentUserId = userId;
            ViewBag.ConversacionId = ConversacionId;


            return PartialView("_Chat", mensajes);

        }

    }
}
