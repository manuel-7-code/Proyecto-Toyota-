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

        [Required]
        public int EmpleadoID { get; set; } // Obligatorio según tu script SQL
        [ForeignKey("EmpleadoID")]
        public virtual Empleados? Empleado { get; set; }

        [DataType(DataType.Date)]
        [Column("FechaVenta")]
        public DateTime FechaVenta { get; set; } = DateTime.Now;

        [Required]
        [Column("PrecioFinal", TypeName = "decimal(12,2)")]
        public decimal MontoTotal { get; set; }

        [Required]
        [StringLength(50)]
        [Column("MetodoPago")]
        public string MetodoPago { get; set; } = "Transferencia Bancaria";

        // Propiedad calculada o auxiliar solo para vista si se requiere
        [NotMapped]
        public string EstadoVenta { get; set; } = "Completada";
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
    [Table("Proveedores")]
    public class Proveedores
    {
        [Key]
        public int ProveedorID { get; set; }

        [Required(ErrorMessage = "El nombre del proveedor es obligatorio")]
        [StringLength(100)]
        public string NombreProveedor { get; set; } = string.Empty; // RF35

        [Required(ErrorMessage = "El país de origen es obligatorio")]
        [StringLength(50)]
        public string PaisOrigen { get; set; } = string.Empty; // RF39

        [StringLength(100)]
        public string? Contacto { get; set; }

        [StringLength(20)]
        public string? Telefono { get; set; }

        [StringLength(100)]
        public string? Correo { get; set; }
    }

    [Table("ImportacionesVehiculos")]
    public class ImportacionesVehiculos
    {
        [Key]
        public int ImportacionID { get; set; }

        [Required]
        public int ProveedorID { get; set; }
        [ForeignKey("ProveedorID")]
        public virtual Proveedores? Proveedor { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaEmbarque { get; set; } = DateTime.Now;

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaEstimadaLlegada { get; set; } = DateTime.Now.AddDays(30);

        [StringLength(100)]
        public string PuertoDestino { get; set; } = "Arica / Iquique - Aduana La Paz";

        [StringLength(50)]
        public string EstadoSeguimiento { get; set; } = "En tránsito"; // RF40

        [Required]
        [Column(TypeName = "decimal(12,2)")]
        public decimal CostoTotalEnvio { get; set; } // RF41

        [Column(TypeName = "decimal(12,2)")]
        public decimal CostoAranceles { get; set; } = 0.00m; // RF41

        [DataType(DataType.DateTime)]
        public DateTime? FechaRecepcionReal { get; set; } // RF42

        // Propiedad calculada para mostrar el costo total puesto en Aduana La Paz
        [NotMapped]
        public decimal CostoTotalImportacion => CostoTotalEnvio + CostoAranceles;
    }
    [Table("CampanasMarketing")]
    public class CampanasMarketing
    {
        [Key]
        public int CampanaID { get; set; }

        [Required(ErrorMessage = "El nombre de la campaña es obligatorio")]
        [StringLength(100)]
        public string NombreCampana { get; set; } = string.Empty; // RF44

        [Required(ErrorMessage = "El presupuesto es obligatorio")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Presupuesto { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; } = DateTime.Now;

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; } = DateTime.Now.AddMonths(1);

        [Required(ErrorMessage = "El canal de difusión es obligatorio")]
        [StringLength(50)]
        public string CanalDifusion { get; set; } = string.Empty;

        // Relación inversa para consultar promociones asociadas (RF49)
        public virtual ICollection<PromocionesVehiculo> Promociones { get; set; } = new List<PromocionesVehiculo>();

        [NotMapped]
        public string EstadoCampana => DateTime.Today >= FechaInicio.Date && DateTime.Today <= FechaFin.Date
            ? "Activa"
            : (DateTime.Today < FechaInicio.Date ? "Programada" : "Finalizada");
    }

    [Table("PromocionesVehiculo")]
    public class PromocionesVehiculo
    {
        [Key]
        public int PromocionID { get; set; }

        [Required]
        public int CampanaID { get; set; }
        [ForeignKey("CampanaID")]
        public virtual CampanasMarketing? Campana { get; set; }

        [Required(ErrorMessage = "El nombre de la promoción es obligatorio")]
        [StringLength(100)]
        public string NombrePromocion { get; set; } = string.Empty; // RF46

        [Required]
        public int ModeloID { get; set; } // RF47: Asociar promoción con vehículo (modelo)
        [ForeignKey("ModeloID")]
        public virtual ModeloVehiculo? ModeloVehiculo { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaInicioVigencia { get; set; } = DateTime.Now; // RF48

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaFinVigencia { get; set; } = DateTime.Now.AddDays(15); // RF48

        [NotMapped]
        public bool EstaVigente => DateTime.Today >= FechaInicioVigencia.Date && DateTime.Today <= FechaFinVigencia.Date;
    }

}