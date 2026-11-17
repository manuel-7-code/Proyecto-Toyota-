using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToyotaLaPaz.Data;
using ToyotaLaPaz.Models;

namespace ToyotaLaPaz.Controllers
{
    public class RolesController : Controller
    {
        private readonly ToyotaLaPazContext _context;

        public RolesController(ToyotaLaPazContext context)
        {
            _context = context;
        }

        // Listar Roles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Roles.ToListAsync());
        }

        // Crear Rol (Vista)
        public IActionResult Create()
        {
            return View();
        }

        // Crear Rol (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RolID,NombreRol,Descripcion")] Rol rol)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rol);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rol);
        }

        // Editar Rol (Vista)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null) return NotFound();
            return View(rol);
        }

        // Editar Rol (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RolID,NombreRol,Descripcion")] Rol rol)
        {
            if (id != rol.RolID) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(rol);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rol);
        }
    }
}