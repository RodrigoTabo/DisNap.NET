using DisnApp.Data;
using DisnApp.Models;
using DisnApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DisnApp.Controllers
{
    public class HomeController : Controller
    {

        private readonly UserManager<Usuario> _userManager;
        private readonly IHomeService _homeService;

        public HomeController(UserManager<Usuario> userManager, IHomeService homeService)
        {
            _userManager = userManager;
            _homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {

            var publicacion = await _homeService.GetPublicacionAsync();
            var historias = await _homeService.GetHistoriaAsync();

            ViewBag.HistoriasPorUsuario = historias.GroupBy(h => h.UsuarioId).ToList();
            ViewBag.CurrentUserId = _userManager.GetUserId(User);

            return View(publicacion);

        }

    }
}
