using DisnApp.Data;
using DisnApp.Models;
using DisnApp.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DisnApp.Controllers
{
    public class PublicacionController : Controller
    {

        private readonly UserManager<Usuario> _userManager;
        private readonly RedDbContext _context;

        public PublicacionController(UserManager<Usuario> userManager, RedDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: PublicacionController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PublicacionController/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var publicacion = await _context.Publicaciones
                .Include(p => p.Usuario)
                .Include(p => p.Likes)
                .Include(p => p.Comentarios).ThenInclude(c => c.Usuario)
                .FirstOrDefaultAsync(p => p.Id == id && !p.Eliminada);

            if (publicacion == null) return NotFound();

            return PartialView("_Details", publicacion);
        }

        // GET: PublicacionController/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: PublicacionController/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CrearPublicacionVM vm)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var nueva = new Publicacion
                    {
                        UsuarioId = _userManager.GetUserId(User),
                        Descripcion = vm.Descripcion,
                        UrlImagen = vm.UrlImagen,
                        FechaSubida = DateTime.Now
                    };

                    await _context.Publicaciones.AddAsync(nueva);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }

        // GET: PublicacionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PublicacionController/Edit/5
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

        // GET: PublicacionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PublicacionController/Delete/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                var publicacion = await _context.Publicaciones.FirstOrDefaultAsync(p => p.Id == id);

                if (publicacion == null)
                    return NotFound();

                var usuarioId = _userManager.GetUserId(User);

                if (publicacion.UsuarioId != usuarioId)
                    return Forbid();

                publicacion.Eliminada = true;
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View("Index", "Home");
            }
        }

        [HttpPost]
        [Authorize]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Like(int id)
        {
            var usuarioId = _userManager.GetUserId(User);
            var likeExistente = await _context.PublicacionLikes
                .FirstOrDefaultAsync(l => l.PublicacionId == id && l.UsuarioId == usuarioId);

            if (likeExistente != null)
            {
                _context.PublicacionLikes.Remove(likeExistente);
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
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Comentar(int id, string contenido)
        {
            var usuarioId = _userManager.GetUserId(User);

            try
            {
                var nuevoComentario = new Comentario
                {
                    PublicacionId = id,
                    UsuarioId = usuarioId,
                    Contenido = contenido,
                    FechaComentario = DateTime.Now
                };

                await _context.Comentarios.AddAsync(nuevoComentario);
                await _context.SaveChangesAsync();

                var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
                if (isAjax)
                {
                    var comentarios = await _context.Comentarios
                        .Where(c => c.PublicacionId == id)
                        .Include(c => c.Usuario)
                        .OrderByDescending(c => c.FechaComentario)
                        .ToListAsync();

                    return PartialView("_CommentsList", comentarios);
                }

                return RedirectToAction("Details", new { id });

            }
            catch (Exception ex)
            {
                return View("Index", "Home");
            }


        }

    }
}
