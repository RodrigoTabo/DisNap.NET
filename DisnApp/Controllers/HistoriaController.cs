using DisnApp.Data;
using DisnApp.Models;
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
        private readonly RedDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public HistoriaController(RedDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: HistoriaController
        public ActionResult Index()
        {
            return View();
        }

        // GET: HistoriaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HistoriaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HistoriaController/Create
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

        // GET: HistoriaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HistoriaController/Edit/5
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

        // GET: HistoriaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HistoriaController/Delete/5
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


        public async Task<ActionResult> _Viewer(int id)
        {

            var ahora = DateTime.UtcNow;

            var historias = await _context.Historias
            .Include(h => h.UsuarioId)
            .Where(h => h.FechaExpiracion > ahora)
            .OrderByDescending(h => h.FechaCreacion)
            .ToListAsync();

            return View(historias);
        }

    }
}
