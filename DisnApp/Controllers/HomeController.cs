using System.Diagnostics;
using DisnApp.Data;
using DisnApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DisnApp.Controllers
{
    public class HomeController : Controller
    {

        private readonly RedDbContext _context;

        public HomeController(RedDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            var publicacion = await _context.Publicaciones
                .Include(p => p.Usuario)
                .Include(p => p.Likes)
                .Include(p => p.Comentarios)
                .OrderByDescending(p => p.FechaSubida)
                .ToListAsync();

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
