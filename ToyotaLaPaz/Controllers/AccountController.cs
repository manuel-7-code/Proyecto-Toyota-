using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToyotaLaPaz.Data;
using ToyotaLaPaz.Models;

namespace ToyotaLaPaz.Controllers
{
    public class AccountController : Controller
    {
        private readonly ToyotaLaPazContext _context;

        public AccountController(ToyotaLaPazContext context)
        {
            _context = context;
        }

        // RF1: Vista de Inicio de Sesión
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Vehiculos");
            }
            return View();
        }

        // RF1: Procesar Inicio de Sesión
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string nombreUsuario, string contrasena)
        {
            if (string.IsNullOrEmpty(nombreUsuario) || string.IsNullOrEmpty(contrasena))
            {
                ViewBag.Error = "Por favor ingrese usuario y contraseña.";
                return View();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario && u.ContrasenaHash == contrasena && u.Estado);

            if (usuario == null)
            {
                ViewBag.Error = "Credenciales incorrectas o cuenta inactiva.";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Email, usuario.CorreoElectronico),
                new Claim(ClaimTypes.Role, usuario.Rol?.NombreRol ?? "Usuario")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Vehiculos");
        }

        // RF3: Cerrar Sesión
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        // RF2: Crear Cuenta / Registrar Usuario (Vista)
        public IActionResult Register()
        {
            ViewBag.RolID = new SelectList(_context.Roles, "RolID", "NombreRol");
            return View();
        }

        // RF2: Crear Cuenta / Registrar Usuario (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("RolID,NombreUsuario,ContrasenaHash,CorreoElectronico")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.FechaCreacion = DateTime.Now;
                usuario.Estado = true;

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            ViewBag.RolID = new SelectList(_context.Roles, "RolID", "NombreRol", usuario.RolID);
            return View(usuario);
        }
    }
}