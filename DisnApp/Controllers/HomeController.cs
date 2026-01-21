using DisnApp.Data;
using DisnApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DisnApp.Controllers
{
    public class HomeController : Controller
    {

        private readonly RedDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public HomeController(RedDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        public async Task<IActionResult> Index()
        {

            var publicacion = await _context.Publicaciones
                .Include(p => p.Usuario)
                .Include(p => p.Likes)
                .Include(p => p.Comentarios)
                .Where(p => !p.Eliminada)
                .OrderByDescending(p => p.FechaSubida)
                .ToListAsync();

            ViewBag.CurrentUserId = _userManager.GetUserId(User);

            return View(publicacion);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
