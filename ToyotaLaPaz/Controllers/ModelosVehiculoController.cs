using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToyotaLaPaz.Data;
using ToyotaLaPaz.Models;

namespace ToyotaLaPaz.Controllers
{
    public class ModelosVehiculoController : Controller
    {
        private readonly ToyotaLaPazContext _context;

        public ModelosVehiculoController(ToyotaLaPazContext context)
        {
            _context = context;
        }

        // Consultar Modelos
        public async Task<IActionResult> Index()
        {
            return View(await _context.ModeloVehiculos.ToListAsync());
        }

        // Registrar Modelo (Vista)
        public IActionResult Create()
        {
            return View();
        }

        // Registrar Modelo (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModeloID,NombreModelo,Fabricacion,TipoVehiculo")] ModeloVehiculo modeloVehiculo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(modeloVehiculo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(modeloVehiculo);
        }

        // Modificar Modelo (Vista)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var modelo = await _context.ModeloVehiculos.FindAsync(id);
            if (modelo == null) return NotFound();

            return View(modelo);
        }

        // Modificar Modelo (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ModeloID,NombreModelo,Fabricacion,TipoVehiculo")] ModeloVehiculo modeloVehiculo)
        {
            if (id != modeloVehiculo.ModeloID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modeloVehiculo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ModeloVehiculos.Any(e => e.ModeloID == modeloVehiculo.ModeloID))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(modeloVehiculo);
        }
    }
}