using Microsoft.EntityFrameworkCore;
using ToyotaLaPaz.Models;

namespace ToyotaLaPaz.Data
{
    public class ToyotaLaPazContext : DbContext
    {
        public ToyotaLaPazContext(DbContextOptions<ToyotaLaPazContext> options) : base(options){}

        public DbSet<Vehiculo> Vehiculos {get; set;}
        public DbSet<ModeloVehiculo> ModeloVehiculos {get; set;}
        public DbSet<Cliente> Clientes {get; set;}
        public DbSet<Venta> Ventas {get; set;}
        public DbSet<Empleado> Empleados{get; set;}
        public DbSet<Rol> Roles{get; set;}
        public DbSet<Usuario> Usuarios{get; set;    }
    }
}