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

    [Table("Clientes")]
    public class Clientes
    {
        [Key]
        public int ClienteID { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        [StringLength(150)]
        [Column("NombreCompleto")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El NIT/CI es obligatorio")]
        [StringLength(20)]
        [Column("NIT_CI")]
        public string NitCi { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [StringLength(20)]
        public string Telefono { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Direccion { get; set; }

        [StringLength(100)]
        public string? Correo { get; set; }
    }

    [Table("Ventas")]
    public class Ventas
    {

        [Key]
        public int VentaID { get; set; }

        [Required]
        public int ClienteID { get; set; }
        [ForeignKey("ClienteID")]
        public virtual Clientes? Cliente { get; set; }

        [Required]
        public int VehiculoID { get; set; }
        [ForeignKey("VehiculoID")]
        public virtual Vehiculo? Vehiculo { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaVenta { get; set; } = DateTime.Now; // RF12

        [Required]
        [Column("PrecioFinal", TypeName = "decimal(12,2)")]
        public decimal MontoTotal { get; set; }

        [StringLength(50)]
        [NotMapped]
        public string EstadoVenta
        {
            get; set;
        } = "Completada"; // RF15 / RF16

        [StringLength(50)]
        public string MetodoPago
        {
            get; set;
        } = "Transferencia Bancaria"; // RF15
    }

    [Table("Departamentos")]
    public class Departamentos
    {
        [Key]
        public int DepartamentoID { get; set; }

        [Required(ErrorMessage = "El nombre del departamento es obligatorio")]
        [StringLength(100)]
        public string NombreDepartamento { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Ubicacion { get; set; }
    }

    [Table("Empleados")]
    public class Empleados
    {
        [Key]
        public int EmpleadoID { get; set; }

        public int? UsuarioID { get; set; }
        [ForeignKey("UsuarioID")]
        public virtual Usuario? Usuario { get; set; }

        [Required]
        public int DepartamentoID { get; set; }
        [ForeignKey("DepartamentoID")]
        public virtual Departamentos? Departamento { get; set; }

        [Required(ErrorMessage = "Los nombres son obligatorios")]
        [StringLength(100)]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [StringLength(100)]
        public string Apellidos { get; set; } = string.Empty;

        [Required(ErrorMessage = "El Carnet de Identidad es obligatorio")]
        [StringLength(20)]
        [Column("CarnetIdentidad")]
        public string CI { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Telefono { get; set; }

        [StringLength(100)]
        public string? Correo { get; set; }

        [Required(ErrorMessage = "El cargo es obligatorio")]
        [StringLength(100)]
        public string Cargo { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime FechaContratacion { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Salario { get; set; }

        [Column("EstadoActivo")]
        public bool Estado { get; set; } = true;

        // Propiedad auxiliar para mostrar el nombre completo sin buscar esta columna en SQL
        [NotMapped]
        public string NombreCompleto => $"{Nombres} {Apellidos}".Trim();
    }

    [Table("DistribucionLogistica")]
    public class DistribucionLogistica
    {
        [Key]
        public int LogisticaID { get; set; }

        [Required]
        public int VehiculoID { get; set; }
        [ForeignKey("VehiculoID")]
        public virtual Vehiculo? Vehiculo { get; set; }

        [Required(ErrorMessage = "El origen es obligatorio")]
        [StringLength(100)]
        public string Origen { get; set; } = string.Empty;

        [Required(ErrorMessage = "El destino es obligatorio")]
        [StringLength(100)]
        public string Destino { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        public DateTime FechaEnvio { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime FechaEntregaEstimada { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? FechaEntregaReal { get; set; }

        [StringLength(50)]
        public string EstadoEnvio { get; set; } = "En ruta";

        [StringLength(100)]
        public string? ConductorAsignado { get; set; }
    }

}