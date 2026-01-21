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
        public ActionResult Details(int id)
        {
            return View();
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
    }
}
