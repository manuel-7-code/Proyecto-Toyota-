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

        // GET: Vehiculos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo == null) return NotFound();

            ViewBag.ModeloID = new SelectList(_context.ModeloVehiculos, "ModeloID", "NombreModelo", vehiculo.ModeloID);
            return View(vehiculo);
        }

        // POST: Vehiculos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VehiculoID,ModeloID,ImportacionID,VIN,Motor,Color,PrecioBase,EstadoVehiculo")] Vehiculo vehiculo)
        {
            if (id != vehiculo.VehiculoID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehiculo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Vehiculos.Any(e => e.VehiculoID == vehiculo.VehiculoID))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ModeloID = new SelectList(_context.ModeloVehiculos, "ModeloID", "NombreModelo", vehiculo.ModeloID);
            return View(vehiculo);
        }
    }
}