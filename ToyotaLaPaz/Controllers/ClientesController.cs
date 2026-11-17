using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToyotaLaPaz.Data;
using ToyotaLaPaz.Models;

namespace ToyotaLaPaz.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ToyotaLaPazContext _context;

        public ClientesController(ToyotaLaPazContext context) => _context = context;

        // RF10: Consultar clientes
        public async Task<IActionResult> Index() => View(await _context.Clientes.ToListAsync());

        // RF9: Registrar cliente (Vista)
        public IActionResult Create() => View();

        // RF9: Registrar cliente (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClienteID,Nombre,NitCi,Correo,Telefono,Direccion")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // RF11: Modificar cliente (Vista)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        // RF11: Modificar cliente (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClienteID,Nombre,NitCi,Correo,Telefono,Direccion")] Cliente cliente)
        {
            if (id != cliente.ClienteID) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }
    }
}