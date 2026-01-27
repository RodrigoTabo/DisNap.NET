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
    public class MensajeController : Controller
    {

        private readonly IMensajeService _mensajeService;
        private readonly UserManager<Usuario> _userManager;


        public MensajeController(IMensajeService mensajeService, UserManager<Usuario> userManager)
        {
            _mensajeService= mensajeService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Chat(int id)
        {
            if (id <= 0) return NotFound();

            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();


            try
            {
                var mensajes = await _mensajeService.GetChatAsync(id, userId);
                ViewBag.ConversacionId = id;
                ViewBag.CurrentUserId = userId;
                return PartialView("_Chat", mensajes);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }




            // devolver vacío en vez de reventar

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Chat(int ConversacionId, string texto)
        {

            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

            try
            {
                var mensajes = await _mensajeService.SendMessageAsync(ConversacionId, userId, texto);
                ViewBag.CurrentUserId = userId;
                ViewBag.ConversacionId = ConversacionId;
                return PartialView("_Chat", mensajes);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

        }

    }
}
