using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToyotaLaPaz.Data;
using ToyotaLaPaz.Models;

namespace ToyotaLaPaz.Controllers
{
    public class ImportacionesController : Controller
    {
        private readonly ToyotaLaPazContext _context;

        public ImportacionesController(ToyotaLaPazContext context) => _context = context;

        // RF36 & RF43: Consultar historial de importaciones y proveedores
        public async Task<IActionResult> Index(string? estado)
        {
            ViewBag.EstadoActual = estado;
            ViewBag.Proveedores = await _context.Proveedores.ToListAsync();

            var query = _context.ImportacionesVehiculos
                .Include(i => i.Proveedor)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(estado))
            {
                query = query.Where(i => i.EstadoSeguimiento == estado);
            }

            return View(await query.OrderByDescending(i => i.ImportacionID).ToListAsync());
        }

        // RF35 & RF39: Registrar Proveedor y País de Origen (Vista)
        public IActionResult CreateProveedor() => View();

        // RF35 & RF39: Registrar Proveedor y País de Origen (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProveedor([Bind("NombreProveedor,PaisOrigen,Contacto,Telefono,Correo")] Proveedores proveedor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proveedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proveedor);
        }

        // RF37 & RF41: Registrar Importación y Costos (Vista)
        public IActionResult Create()
        {
            ViewBag.ProveedorID = new SelectList(_context.Proveedores, "ProveedorID", "NombreProveedor");
            return View();
        }

        // RF37 & RF41: Registrar Importación y Costos (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProveedorID,FechaEmbarque,FechaEstimadaLlegada,PuertoDestino,CostoTotalEnvio,CostoAranceles")] ImportacionesVehiculos importacion)
        {
            if (ModelState.IsValid)
            {
                importacion.EstadoSeguimiento = "En tránsito";
                _context.Add(importacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ProveedorID = new SelectList(_context.Proveedores, "ProveedorID", "NombreProveedor", importacion.ProveedorID);
            return View(importacion);
        }

        // RF40 & RF42: Seguimiento y Recepción de Vehículos (Vista)
        public async Task<IActionResult> Seguimiento(int? id)
        {
            if (id == null) return NotFound();
            var importacion = await _context.ImportacionesVehiculos
                .Include(i => i.Proveedor)
                .FirstOrDefaultAsync(i => i.ImportacionID == id);

            if (importacion == null) return NotFound();
            return View(importacion);
        }

        // RF40, RF41 & RF42: Actualizar Estado, Aranceles y Registrar Recepción Real (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Seguimiento(int id, string estadoSeguimiento, decimal costoAranceles, DateTime? fechaRecepcionReal)
        {
            var importacion = await _context.ImportacionesVehiculos.FindAsync(id);
            if (importacion != null)
            {
                importacion.EstadoSeguimiento = estadoSeguimiento;
                importacion.CostoAranceles = costoAranceles;

                if (estadoSeguimiento == "Recibido en Aduana / Almacén")
                {
                    importacion.FechaRecepcionReal = fechaRecepcionReal ?? DateTime.Now;
                }

                _context.Update(importacion);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}