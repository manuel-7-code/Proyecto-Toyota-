using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToyotaLaPaz.Data;
using ToyotaLaPaz.Models;

namespace ToyotaLaPaz.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly ToyotaLaPazContext _context;

        public EmpleadosController(ToyotaLaPazContext context) => _context = context;

        // RF18 & RF21: Consultar empleado y consultar personal
        public async Task<IActionResult> Index(int? departamentoId, string? buscar)
        {
            ViewBag.Departamentos = new SelectList(_context.Departamentos, "DepartamentoID", "NombreDepartamento", departamentoId);
            ViewBag.BuscarActual = buscar;

            var query = _context.Empleados
                .Include(e => e.Departamento)
                .Where(e => e.Estado)
                .AsQueryable();

            if (departamentoId.HasValue)
            {
                query = query.Where(e => e.DepartamentoID == departamentoId.Value);
            }

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                query = query.Where(e => 
                    e.Nombres.Contains(buscar) || 
                    e.Apellidos.Contains(buscar) || 
                    e.CI.Contains(buscar) || 
                    e.Cargo.Contains(buscar));
            }

            return View(await query.ToListAsync());
        }

        // RF17 & RF20: Registrar empleado (Vista)
        public IActionResult Create()
        {
            ViewBag.DepartamentoID = new SelectList(_context.Departamentos, "DepartamentoID", "NombreDepartamento");
            return View();
        }

        // RF17 & RF20: Registrar empleado (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartamentoID,Nombres,Apellidos,CI,Cargo,Telefono,Correo,Salario,FechaContratacion")] Empleados empleado)
        {
            if (ModelState.IsValid)
            {
                empleado.Estado = true;
                _context.Add(empleado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.DepartamentoID = new SelectList(_context.Departamentos, "DepartamentoID", "NombreDepartamento", empleado.DepartamentoID);
            return View(empleado);
        }

        // RF19 & RF20: Modificar empleado (Vista)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null) return NotFound();

            ViewBag.DepartamentoID = new SelectList(_context.Departamentos, "DepartamentoID", "NombreDepartamento", empleado.DepartamentoID);
            return View(empleado);
        }

        // RF19 & RF20: Modificar empleado (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmpleadoID,UsuarioID,DepartamentoID,Nombres,Apellidos,CI,Cargo,Telefono,Correo,Salario,FechaContratacion,Estado")] Empleados empleado)
        {
            if (id != empleado.EmpleadoID) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(empleado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.DepartamentoID = new SelectList(_context.Departamentos, "DepartamentoID", "NombreDepartamento", empleado.DepartamentoID);
            return View(empleado);
        }

        // RF22: Eliminar empleado (Baja lógica)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado != null)
            {
                empleado.Estado = false;
                _context.Update(empleado);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}