using DisnApp.Models;
using DisnApp.Services;
using DisnApp.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace DisnApp.Controllers
{
    public class PublicacionController : Controller
    {

        private readonly UserManager<Usuario> _userManager;
        private readonly IPublicacionService _publicacionService;



        public PublicacionController(UserManager<Usuario> userManager, IPublicacionService publicacionService)
        {
            _userManager = userManager;
            _publicacionService = publicacionService;
        }

        // GET: PublicacionController/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {

            var publicacion = await _publicacionService.GetDetailsAsync(id);
            if (publicacion == null) return NotFound();

            return PartialView("_Details", publicacion);
        }

        // POST: PublicacionController/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CrearPublicacionVM vm)
        {

            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var nueva = await _publicacionService.CreateAsync(vm, userId);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(); 
            }

        }


        // POST: PublicacionController/Delete/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            var userId = _userManager.GetUserId(User);

            try
            {
                var publicacion = await _publicacionService.SoftDeleteAsync(id, userId);

                if (publicacion == null)
                    return NotFound();

                return RedirectToAction("Index", "Home");
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpPost]
        [Authorize]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Like(int id)
        {

            var userId = _userManager.GetUserId(User);
            var nuevoLike = await _publicacionService.ToggleLikeAsync(id, userId);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Comentar(int id, string contenido)
        {

            if (id <= 0) return BadRequest();
            if (string.IsNullOrWhiteSpace(contenido)) return BadRequest("Contenido requerido.");

            var userId = _userManager.GetUserId(User);
            await _publicacionService.AddCommentAsync(id, contenido, userId);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var comentarios = await _publicacionService.GetComentariosAsync(id);
                return PartialView("_CommentsList", comentarios);
            }

            return RedirectToAction("Details", new { id });

        }

    }
}
