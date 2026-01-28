using DisnApp.Data;
using DisnApp.Models;
using DisnApp.Services;
using DisnApp.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace DisnApp.Controllers
{
    public class HistoriaController : Controller
    {
        private readonly IHistoriaService _historiaService;
        private readonly UserManager<Usuario> _userManager;

        public HistoriaController(UserManager<Usuario> userManager, IHistoriaService historiaService)
        {
            _userManager = userManager;
            _historiaService = historiaService;
        }

        [Authorize]
        public async Task<ActionResult> Viewer()
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

            var historias = await _historiaService.GetViewerAsync();

            return View(historias);

        }

    }
}
