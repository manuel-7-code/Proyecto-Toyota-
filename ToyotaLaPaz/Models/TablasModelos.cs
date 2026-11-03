using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToyotaLaPaz.Models
{
    [Table("Vehiculos")]
    public class Vehiculo
    {
        [Key]
        public int VehiculoID { get; set; }
        public int ModeloID { get; set; }
        [ForeignKey("ModeloID")]
        public virtual ModeloVehiculo? ModeloVehiculo { get; set; }
        public string VIN { get; set; } = string.Empty;
        public string Motor { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public decimal PrecioBase { get; set; }
        public string EstadoVehiculo { get; set; } = "En Stock";
    }

    [Table("ModelosVehiculo")]
    public class ModeloVehiculo
    {
        [Key]
        public int ModeloID { get; set; }
        public string NombreModelo { get; set; } = string.Empty;
        public int Fabricacion { get; set; }
        public string TipoVehiculo { get; set; } = string.Empty;
    }

    [Table("Clientes")]
    public class Cliente
    {
        [Key]
        public int ClienteID { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string NIT_CI { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
    }

    [Table("Ventas")]
    public class Venta
    {
        [Key]
        public int VentaID { get; set; }
        public int ClienteID { get; set; }
        public int VehiculoID { get; set; }
        public decimal PrecioFinal { get; set; }
    }

    [Table("Empleados")]
    public class Empleado
    {
        [Key]
        public int EmpleadoID { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
    }

    [Table("Roles")]
    public class Rol
    {
        [Key]
        public int RolID { get; set; }

        [Required]
        [StringLength(50)]
        public string NombreRol { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Descripcion { get; set; }
    }

    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        public int UsuarioID { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio")]
        public int RolID { get; set; }

        [ForeignKey("RolID")]
        public virtual Rol? Rol { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [StringLength(50)]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(255)]
        public string ContrasenaHash { get; set; } = string.Empty; // RF1 / RF2

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress]
        [StringLength(100)]
        public string CorreoElectronico { get; set; } = string.Empty;

        public bool Estado { get; set; } = true; // RF3: Activo / Inactivo

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}