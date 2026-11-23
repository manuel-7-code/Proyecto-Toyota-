using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToyotaLaPaz.Data;
using ToyotaLaPaz.Models;

namespace ToyotaLaPaz.Controllers
{
    public class VentasController : Controller
    {
        private readonly ToyotaLaPazContext _context;

        public VentasController(ToyotaLaPazContext context) => _context = context;

        // RF14: Consultar historial de ventas
        public async Task<IActionResult> Index()
        {
            var ventas = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Vehiculo)
                .ThenInclude(veh => veh!.ModeloVehiculo)
                .ToListAsync();
            return View(ventas);
        }

        // RF12 & RF13: Vista para registrar venta y verificar stock disponible
        public IActionResult Create()
        {
            ViewBag.ClienteID = new SelectList(_context.Clientes, "ClienteID", "Nombre");
            ViewBag.VehiculoID = new SelectList(_context.Vehiculos.Where(v => v.EstadoVehiculo == "En Stock"), "VehiculoID", "VIN");
            ViewBag.EmpleadoID = new SelectList(_context.Empleados.Where(e => e.Estado), "EmpleadoID", "NombreCompleto"); // <--- Añadir esto
            return View();
        }

        // RF12, RF15 & RF16: Registrar venta, pago y generar comprobante
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClienteID,VehiculoID,MontoTotal,MetodoPago")] Ventas venta)
        {
            if (ModelState.IsValid)
            {
                // Verificar nuevamente disponibilidad del vehículo
                var vehiculo = await _context.Vehiculos.FindAsync(venta.VehiculoID);
                if (vehiculo == null || vehiculo.EstadoVehiculo != "En Stock")
                {
                    ModelState.AddModelError("", "El vehículo seleccionado no se encuentra disponible.");
                }
                else
                {
                    // Cambiar estado del vehículo a "Vendido"
                    vehiculo.EstadoVehiculo = "Vendido";
                    _context.Update(vehiculo);

                    venta.FechaVenta = DateTime.Now;
                    venta.EstadoVenta = "Completada"; // RF15: Pago registrado

                    _context.Add(venta);
                    await _context.SaveChangesAsync();

                    // Redirigir a comprobante (RF16)
                    return RedirectToAction(nameof(Comprobante), new { id = venta.VentaID });
                }
            }
            ViewBag.ClienteID = new SelectList(_context.Clientes, "ClienteID", "Nombre", venta.ClienteID);
            ViewBag.VehiculoID = new SelectList(_context.Vehiculos.Where(v => v.EstadoVehiculo == "En Stock"), "VehiculoID", "VIN", venta.VehiculoID);
            return View(venta);
        }

        // RF16: Generar Comprobante de Venta
        public async Task<IActionResult> Comprobante(int? id)
        {
            if (id == null) return NotFound();

            var venta = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Vehiculo)
                .ThenInclude(veh => veh!.ModeloVehiculo)
                .FirstOrDefaultAsync(v => v.VentaID == id);

            if (venta == null) return NotFound();

            return View(venta);
        }
    }
}