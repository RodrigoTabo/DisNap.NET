using DisnApp.Data;
using DisnApp.Models;
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

            var mensajes = await _context.Mensajes
                .Where(m => m.ConversacionId == id)
                .OrderBy(m => m.FechaEnvio) // o el campo que uses
                .ToListAsync();

            // devolver vacío en vez de reventar
            return PartialView("_Chat", mensajes);
        }

    }
}
