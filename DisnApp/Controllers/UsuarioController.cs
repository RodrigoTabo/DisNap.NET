using DisnApp.Data;
using DisnApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DisnApp.Controllers
{
    public class UsuarioController : Controller
    {

        private readonly UserManager<Usuario> _userManager;
        private readonly RedDbContext _context;

        public UsuarioController(UserManager<Usuario> userManager, RedDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        // GET: UsuarioController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UsuarioController/Details/5
        public async Task<IActionResult> Details(string id)
        {

            var usuarioId = _userManager.GetUserId(User);


            var usuario = await _context.Usuarios
                 .Include(u => u.Publicaciones)
                 .Include(u => u.Seguidores)
                 .Include(u => u.Siguiendo)
                 .FirstOrDefaultAsync(p => p.Id == id);


            ViewBag.EsMiPerfil = (usuarioId != null && usuarioId == usuario.Id);

            // si no está logueado => no sigue
            ViewBag.YaLoSigo = usuarioId != null && await _context.Set<SeguidorUsuario>()
                .AnyAsync(s => s.SeguidorId == usuarioId && s.SeguidoId == usuario.Id);

            return View(usuario);
        }

        // GET: UsuarioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsuarioController/Create
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

        // GET: UsuarioController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsuarioController/Edit/5
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

        // GET: UsuarioController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsuarioController/Delete/5
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

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Seguir(string id) // id = perfil a seguir (SeguidoId)
        {
            var usuarioId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(usuarioId)) return Challenge(); // no logueado -> login

            if (usuarioId == id) return RedirectToAction("Details", new { id }); // no te sigas a vos mismo

            var existente = await _context.SeguidorUsuarios
                .FirstOrDefaultAsync(s => s.SeguidorId == usuarioId && s.SeguidoId == id);

            if (existente != null)
            {
                _context.SeguidorUsuarios.Remove(existente);
            }
            else
            {
                var nuevo = new SeguidorUsuario
                {
                    SeguidorId = usuarioId,
                    SeguidoId = id,
                    FechaInicio = DateTime.UtcNow
                };

                await _context.SeguidorUsuarios.AddAsync(nuevo); 
            }

            await _context.SaveChangesAsync();

            // Volver al perfil que estabas mirando
            return RedirectToAction("Details", new { id });
        }


    }
}
