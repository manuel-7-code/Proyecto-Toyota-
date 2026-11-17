using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToyotaLaPaz.Data;
using ToyotaLaPaz.Models;

namespace ToyotaLaPaz.Controllers
{
    public class LogisticaController : Controller
    {
        private readonly ToyotaLaPazContext _context;

        public LogisticaController(ToyotaLaPazContext context) => _context = context;

        // RF28: Consultar entregas y distribuciones
        public async Task<IActionResult> Index()
        {
            var lista = await _context.DistribucionLogistica
                .Include(d => d.Vehiculo)
                .ThenInclude(v => v!.ModeloVehiculo)
                .ToListAsync();
            return View(lista);
        }

        // RF23, RF24, RF25: Vista de registro
        public IActionResult Create()
        {
            ViewBag.VehiculoID = new SelectList(_context.Vehiculos.Where(v => v.EstadoVehiculo == "En Stock"), "VehiculoID", "VIN");
            return View();
        }

        // RF23, RF24, RF25: Registrar distribución/envío (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VehiculoID,Origen,Destino,FechaEnvio,FechaEntregaEstimada,ConductorAsignado")] DistribucionLogistica distribucion)
        {
            if (ModelState.IsValid)
            {
                distribucion.EstadoEnvio = "En ruta";
                _context.Add(distribucion);

                // Actualizar estado del vehículo
                var vehiculo = await _context.Vehiculos.FindAsync(distribucion.VehiculoID);
                if (vehiculo != null)
                {
                    vehiculo.EstadoVehiculo = "En Tránsito";
                    _context.Update(vehiculo);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.VehiculoID = new SelectList(_context.Vehiculos, "VehiculoID", "VIN", distribucion.VehiculoID);
            return View(distribucion);
        }

        // RF26: Actualizar estado de envío (Vista)
        public async Task<IActionResult> EditEstado(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.DistribucionLogistica.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        // RF26 & RF27: Actualizar estado y registrar entrega real (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEstado(int id, string estadoEnvio, DateTime? fechaEntregaReal)
        {
            var item = await _context.DistribucionLogistica.FindAsync(id);
            if (item != null)
            {
                item.EstadoEnvio = estadoEnvio;
                if (estadoEnvio == "Entregado")
                {
                    item.FechaEntregaReal = fechaEntregaReal ?? DateTime.Now;

                    // Cambiar vehículo a disponible/en sucursal destino
                    var vehiculo = await _context.Vehiculos.FindAsync(item.VehiculoID);
                    if (vehiculo != null)
                    {
                        vehiculo.EstadoVehiculo = "En Sucursal Destino";
                        _context.Update(vehiculo);
                    }
                }
                _context.Update(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}