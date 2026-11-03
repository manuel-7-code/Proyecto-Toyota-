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

        // Listar usuarios registrados
        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuarios.Include(u => u.Rol).ToListAsync();
            return View(usuarios);
        }

        // RF2: Vista para Crear Cuenta / Registrar Usuario
        public IActionResult Register()
        {
            ViewBag.RolID = new SelectList(_context.Roles, "RolID", "NombreRol");
            return View();
        }

        // RF2: Guardar el nuevo usuario en SQL Server
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("RolID,NombreUsuario,ContrasenaHash,CorreoElectronico")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Nota: En producción se recomienda aplicar hashing seguro (ej. BCrypt o ASP.NET Identity)
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