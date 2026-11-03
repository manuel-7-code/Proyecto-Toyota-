
CREATE DATABASE ToyotaLaPazDB;
GO

USE ToyotaLaPazDB;
GO

CREATE TABLE Roles (
    RolID INT IDENTITY(1,1) PRIMARY KEY,
    NombreRol VARCHAR(50) NOT NULL UNIQUE, 
    Descripcion VARCHAR(255)
);

CREATE TABLE Usuarios (
    UsuarioID INT IDENTITY(1,1) PRIMARY KEY,
    RolID INT NOT NULL,
    NombreUsuario VARCHAR(50) NOT NULL UNIQUE,
    ContrasenaHash VARCHAR(255) NOT NULL, 
    CorreoElectronico VARCHAR(100) NOT NULL,
    Estado BIT DEFAULT 1, 
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (RolID) REFERENCES Roles(RolID)
);

CREATE TABLE Departamentos (
    DepartamentoID INT IDENTITY(1,1) PRIMARY KEY,
    NombreDepartamento VARCHAR(100) NOT NULL, 
    Ubicacion VARCHAR(100)
);

CREATE TABLE Empleados (
    EmpleadoID INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioID INT NULL,
    DepartamentoID INT NOT NULL,
    Nombres VARCHAR(100) NOT NULL, 
    Apellidos VARCHAR(100) NOT NULL,
    CarnetIdentidad VARCHAR(20) NOT NULL UNIQUE,
    Telefono VARCHAR(20),
    Correo VARCHAR(100),
    Cargo VARCHAR(100) NOT NULL,
    FechaContratacion DATE NOT NULL,
    Salario DECIMAL(10,2) NOT NULL,
    EstadoActivo BIT DEFAULT 1, 
    FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID),
    FOREIGN KEY (DepartamentoID) REFERENCES Departamentos(DepartamentoID)
);

CREATE TABLE Proveedores (
    ProveedorID INT IDENTITY(1,1) PRIMARY KEY,
    NombreProveedor VARCHAR(100) NOT NULL, 
    PaisOrigen VARCHAR(50) NOT NULL,      
    Contacto VARCHAR(100),
    Correo VARCHAR(100),
    Telefono VARCHAR(20)
);

CREATE TABLE ImportacionesVehiculos (
    ImportacionID INT IDENTITY(1,1) PRIMARY KEY,
    ProveedorID INT NOT NULL,            
    FechaEmbarque DATE NOT NULL,
    FechaEstimadaLlegada DATE NOT NULL,
    PuertoDestino VARCHAR(100) DEFAULT 'Arica / Iquique - Aduana La Paz',
    EstadoSeguimiento VARCHAR(50) DEFAULT 'En tránsito', 
    CostoTotalEnvio DECIMAL(12,2) NOT NULL,              
    CostoAranceles DECIMAL(12,2) DEFAULT 0.00,
    FechaRecepcionReal DATETIME NULL,                  
    FOREIGN KEY (ProveedorID) REFERENCES Proveedores(ProveedorID)
);

CREATE TABLE ModelosVehiculo (
    ModeloID INT IDENTITY(1,1) PRIMARY KEY,
    NombreModelo VARCHAR(100) NOT NULL, 
    Fabricacion INT NOT NULL, --Año de fabricacion
    TipoVehiculo VARCHAR(50) NOT NULL, 
    EspecificacionesTecnicas TEXT
);

CREATE TABLE Vehiculos (
    VehiculoID INT IDENTITY(1,1) PRIMARY KEY,
    ModeloID INT NOT NULL,
    ImportacionID INT NULL,               
    VIN VARCHAR(50) NOT NULL UNIQUE,        
    Motor VARCHAR(50) NOT NULL,
    Color VARCHAR(30) NOT NULL,
    PrecioBase DECIMAL(12,2) NOT NULL,
    EstadoVehiculo VARCHAR(50) DEFAULT 'En Stock', 
    FOREIGN KEY (ModeloID) REFERENCES ModelosVehiculo(ModeloID),
    FOREIGN KEY (ImportacionID) REFERENCES ImportacionesVehiculos(ImportacionID)
);

CREATE TABLE Almacenes (
    AlmacenID INT IDENTITY(1,1) PRIMARY KEY,
    NombreAlmacen VARCHAR(100) NOT NULL, 
    Direccion VARCHAR(255) NOT NULL,
    Ciudad VARCHAR(50) DEFAULT 'La Paz'
);

CREATE TABLE InventarioStock (
    InventarioID INT IDENTITY(1,1) PRIMARY KEY,
    VehiculoID INT NOT NULL UNIQUE,
    AlmacenID INT NOT NULL,
    UbicacionPatio VARCHAR(50),
    FechaIngreso DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (VehiculoID) REFERENCES Vehiculos(VehiculoID),
    FOREIGN KEY (AlmacenID) REFERENCES Almacenes(AlmacenID)
);

CREATE TABLE MovimientosInventario (
    MovimientoID INT IDENTITY(1,1) PRIMARY KEY,
    VehiculoID INT NOT NULL,
    AlmacenID INT NOT NULL,
    TipoMovimiento VARCHAR(20) NOT NULL, 
    FechaMovimiento DATETIME DEFAULT GETDATE(),
    Motivo VARCHAR(255),           
    FOREIGN KEY (VehiculoID) REFERENCES Vehiculos(VehiculoID),
    FOREIGN KEY (AlmacenID) REFERENCES Almacenes(AlmacenID)
);

CREATE TABLE DistribucionLogistica (
    LogisticaID INT IDENTITY(1,1) PRIMARY KEY,
    VehiculoID INT NOT NULL,
    Origen VARCHAR(100) NOT NULL,
    Destino VARCHAR(100) NOT NULL,           
    FechaEnvio DATETIME NOT NULL,               
    FechaEntregaEstimada DATETIME NOT NULL,
    FechaEntregaReal DATETIME NULL,             
    EstadoEnvio VARCHAR(50) DEFAULT 'En ruta',  
    ConductorAsignado VARCHAR(100),            
    FOREIGN KEY (VehiculoID) REFERENCES Vehiculos(VehiculoID)
);

CREATE TABLE CampanasMarketing (
    CampanaID INT IDENTITY(1,1) PRIMARY KEY,
    NombreCampana VARCHAR(100) NOT NULL, 
    Presupuesto DECIMAL(10,2) NOT NULL,
    FechaInicio DATE NOT NULL,
    FechaFin DATE NOT NULL,
    CanalDifusion VARCHAR(50) NOT NULL
);

CREATE TABLE PromocionesVehiculo (
    PromocionID INT IDENTITY(1,1) PRIMARY KEY,
    CampanaID INT NOT NULL,
    NombrePromocion VARCHAR(100) NOT NULL, 
    ModeloID INT NOT NULL,                 
    FechaInicioVigencia DATE NOT NULL,     
    FechaFinVigencia DATE NOT NULL,       
    FOREIGN KEY (CampanaID) REFERENCES CampanasMarketing(CampanaID),
    FOREIGN KEY (ModeloID) REFERENCES ModelosVehiculo(ModeloID)
);

CREATE TABLE Clientes (
    ClienteID INT IDENTITY(1,1) PRIMARY KEY,
    NombreCompleto VARCHAR(150) NOT NULL, 
    NIT_CI VARCHAR(20) NOT NULL UNIQUE,
    Telefono VARCHAR(20) NOT NULL,
    Direccion VARCHAR(255),
    Correo VARCHAR(100)
);

CREATE TABLE Ventas (
    VentaID INT IDENTITY(1,1) PRIMARY KEY,
    ClienteID INT NOT NULL,
    VehiculoID INT NOT NULL UNIQUE,       
    EmpleadoID INT NOT NULL,              
    FechaVenta DATETIME DEFAULT GETDATE(), 
    PrecioFinal DECIMAL(12,2) NOT NULL,
    MetodoPago VARCHAR(50) NOT NULL,
    FOREIGN KEY (ClienteID) REFERENCES Clientes(ClienteID),
    FOREIGN KEY (VehiculoID) REFERENCES Vehiculos(VehiculoID),
    FOREIGN KEY (EmpleadoID) REFERENCES Empleados(EmpleadoID)
);

CREATE TABLE PagosVenta (
    PagoID INT IDENTITY(1,1) PRIMARY KEY,
    VentaID INT NOT NULL,               
    MontoPagado DECIMAL(12,2) NOT NULL,
    FechaPago DATETIME DEFAULT GETDATE(),
    MetodoPago VARCHAR(50) NOT NULL,
    ReferenciaBanco VARCHAR(100),
    FOREIGN KEY (VentaID) REFERENCES Ventas(VentaID)
);

CREATE TABLE ComprobantesVenta (
    ComprobanteID INT IDENTITY(1,1) PRIMARY KEY,
    VentaID INT NOT NULL,               
    NumeroFactura VARCHAR(50) NOT NULL UNIQUE,
    FechaEmision DATETIME DEFAULT GETDATE(),
    MontoTotal DECIMAL(12,2) NOT NULL,
    NIT_Cliente VARCHAR(20) NOT NULL,
    RazonSocial VARCHAR(150) NOT NULL,
    FOREIGN KEY (VentaID) REFERENCES Ventas(VentaID)
);

CREATE TABLE Cotizaciones (
    CotizacionID INT IDENTITY(1,1) PRIMARY KEY,
    ClienteID INT NOT NULL,
    EmpleadoID INT NOT NULL,
    ModeloID INT NOT NULL,          
    FechaCotizacion DATETIME DEFAULT GETDATE(),
    ValidezHasta DATE NOT NULL,
    PrecioCotizado DECIMAL(12,2) NOT NULL, 
    Observaciones TEXT,
    EstadoCotizacion VARCHAR(50) DEFAULT 'Pendiente',
    FOREIGN KEY (ClienteID) REFERENCES Clientes(ClienteID),
    FOREIGN KEY (EmpleadoID) REFERENCES Empleados(EmpleadoID),
    FOREIGN KEY (ModeloID) REFERENCES ModelosVehiculo(ModeloID)
);

CREATE TABLE ReservasVehiculo (
    ReservaID INT IDENTITY(1,1) PRIMARY KEY,
    VehiculoID INT NOT NULL,              
    ClienteID INT NOT NULL,              
    FechaReserva DATETIME DEFAULT GETDATE(),
    FechaExpiracion DATETIME NOT NULL,
    MontoGarantia DECIMAL(10,2) NOT NULL,
    EstadoReserva VARCHAR(50) DEFAULT 'Activa',
    FOREIGN KEY (VehiculoID) REFERENCES Vehiculos(VehiculoID),
    FOREIGN KEY (ClienteID) REFERENCES Clientes(ClienteID)
);

