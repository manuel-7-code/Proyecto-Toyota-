using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToyotaLaPaz.Data;
using ToyotaLaPaz.Models;

namespace ToyotaLaPaz.Controllers
{
    public class VehiculosController : Controller
    {
        private readonly ToyotaLaPazContext _context;

        public VehiculosController(ToyotaLaPazContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var vehiculos = await _context.Vehiculos
                .Include(v => v.ModeloVehiculo)
                .ToListAsync();
            return View(vehiculos);
        }

        public IActionResult Create()
        {
            ViewBag.ModeloID = new SelectList(_context.ModeloVehiculos, "ModeloID", "NombreModelo");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModeloID,VIN,Motor,Color,PrecioBase,EstadoVehiculo")] Vehiculo vehiculo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehiculo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ModeloID = new SelectList(_context.ModeloVehiculos, "ModeloID", "NombreModelo", vehiculo.ModeloID);
            return View(vehiculo);
        }
    }
}