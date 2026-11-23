using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToyotaLaPaz.Data;
using ToyotaLaPaz.Models;

namespace ToyotaLaPaz.Controllers
{
    public class MarketingController : Controller
    {
        private readonly ToyotaLaPazContext _context;

        public MarketingController(ToyotaLaPazContext context) => _context = context;

        // RF49: Consultar resultados, campañas y promociones vigentes
        public async Task<IActionResult> Index(string? canal)
        {
            ViewBag.CanalActual = canal;

            var campanasQuery = _context.CampanasMarketing
                .Include(c => c.Promociones)
                .ThenInclude(p => p.ModeloVehiculo)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(canal))
            {
                campanasQuery = campanasQuery.Where(c => c.CanalDifusion == canal);
            }

            var campanas = await campanasQuery.OrderByDescending(c => c.CampanaID).ToListAsync();

            var promociones = await _context.PromocionesVehiculo
                .Include(p => p.Campana)
                .Include(p => p.ModeloVehiculo)
                .OrderByDescending(p => p.PromocionID)
                .ToListAsync();

            // Indicadores de resultados (RF49): ventas registradas dentro de fechas de campaña
            var ventasTotales = await _context.Ventas.Include(v => v.Vehiculo).ToListAsync();
            ViewBag.TotalInvertido = campanas.Sum(c => c.Presupuesto);
            ViewBag.TotalPromocionesActivas = promociones.Count(p => p.EstaVigente);
            ViewBag.VentasEnCampana = ventasTotales.Count(v =>
                campanas.Any(c => v.FechaVenta.Date >= c.FechaInicio.Date && v.FechaVenta.Date <= c.FechaFin.Date));

            ViewBag.Promociones = promociones;
            return View(campanas);
        }

        // RF44: Registrar Campaña (Vista)
        public IActionResult CreateCampana() => View();

        // RF44: Registrar Campaña (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCampana([Bind("NombreCampana,Presupuesto,FechaInicio,FechaFin,CanalDifusion")] CampanasMarketing campana)
        {
            if (ModelState.IsValid)
            {
                _context.Add(campana);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(campana);
        }

        // RF45: Modificar Campaña (Vista)
        public async Task<IActionResult> EditCampana(int? id)
        {
            if (id == null) return NotFound();
            var campana = await _context.CampanasMarketing.FindAsync(id);
            if (campana == null) return NotFound();
            return View(campana);
        }

        // RF45: Modificar Campaña (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCampana(int id, [Bind("CampanaID,NombreCampana,Presupuesto,FechaInicio,FechaFin,CanalDifusion")] CampanasMarketing campana)
        {
            if (id != campana.CampanaID) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(campana);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(campana);
        }

        // RF46, RF47 & RF48: Registrar Promoción, Asociar Vehículo y Vigencia (Vista)
        public IActionResult CreatePromocion(int? campanaId)
        {
            ViewBag.CampanaID = new SelectList(_context.CampanasMarketing, "CampanaID", "NombreCampana", campanaId);
            ViewBag.ModeloID = new SelectList(_context.ModeloVehiculos, "ModeloID", "NombreModelo");
            return View();
        }

        // RF46, RF47 & RF48: Registrar Promoción (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePromocion([Bind("CampanaID,NombrePromocion,ModeloID,FechaInicioVigencia,FechaFinVigencia")] PromocionesVehiculo promocion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(promocion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CampanaID = new SelectList(_context.CampanasMarketing, "CampanaID", "NombreCampana", promocion.CampanaID);
            ViewBag.ModeloID = new SelectList(_context.ModeloVehiculos, "ModeloID", "NombreModelo", promocion.ModeloID);
            return View(promocion);
        }
    }
}