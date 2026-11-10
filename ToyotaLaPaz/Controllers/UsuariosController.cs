using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToyotaLaPaz.Data;
using ToyotaLaPaz.Models;

namespace ToyotaLaPaz.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ToyotaLaPazContext _context;

        public UsuariosController(ToyotaLaPazContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuarios.Include(u => u.Rol).ToListAsync();
            return View(usuarios);
        }
        public IActionResult Register()
        {
            ViewBag.RolID = new SelectList(_context.Roles, "RolID", "NombreRol");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("RolID,NombreUsuario,ContrasenaHash,CorreoElectronico")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
   
                usuario.FechaCreacion = DateTime.Now;
                usuario.Estado = true;

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.RolID = new SelectList(_context.Roles, "RolID", "NombreRol", usuario.RolID);
            return View(usuario);
        }
    }
}