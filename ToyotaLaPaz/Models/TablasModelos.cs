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
}