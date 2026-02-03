using DisnApp.Data;
using DisnApp.Models;
using DisnApp.Services;
using DisnApp.ViewModel;
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
        private readonly IMensajeService _mensajeService;
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(UserManager<Usuario> userManager, RedDbContext context, IMensajeService mensajeService, IUsuarioService usuarioService)
        {
            _userManager = userManager;
            _context = context;
            _mensajeService = mensajeService;
            _usuarioService = usuarioService;
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
            var usuario = await _usuarioService.GetDetailsAsync(id);

            if (usuario == null)
                return NotFound(); // <- clave

            ViewBag.EsMiPerfil = (usuarioId != null && usuarioId == usuario.Id);

            // si no está logueado => no sigue
            ViewBag.YaLoSigo = usuarioId != null && await _context.Set<SeguidorUsuario>()
                .AnyAsync(s => s.SeguidorId == usuarioId && s.SeguidoId == usuario.Id);

            return View(usuario);
        }

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Seguir(string id) // id = perfil a seguir (SeguidoId)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId)) return Challenge(); // no logueado -> login

            if (userId == id) return RedirectToAction("Details", new { id }); // no te sigas a vos mismo

            var existente = await _usuarioService.PostSeguirAsync(id, userId);

            // Volver al perfil que estabas mirando
            return RedirectToAction("Details", new { id });
        }


        [HttpGet]
        public async Task<IActionResult> Conversacion()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();
            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest"
                         || Request.Query.ContainsKey("partial");

            var items = await _mensajeService.GetBandejaAsync(userId); // ✅ await


            if (isAjax)
                return PartialView("~/Views/Mensaje/_Bandeja.cshtml", items);

            return View(items);
        }


        [HttpGet]
        public async Task<IActionResult> Search(string busqueda)
        {

            busqueda ??= "";

            if (busqueda.Length < 2)
                return PartialView("_UserSearchResults", new List<Usuario>());

            var users = await _userManager.Users
                .Where(u => u.Email.Contains(busqueda))
                .OrderBy(u => u.Email)
                .Take(10)
                .ToListAsync();

            return PartialView("_UserSearchResults", users);

        }

    }
}
