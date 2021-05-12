USE master
ALTER DATABASE Facturacion SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE IF EXISTS Facturacion

USE master
CREATE DATABASE Facturacion
GO

USE Facturacion
GO

CREATE TABLE pro_Producto
(
	pro_Id INT CONSTRAINT PK_pro PRIMARY KEY IDENTITY(1, 1)
	, pro_FechaCreacion DATETIME NOT NULL
	, pro_FechaActualizacion DATETIME NOT NULL
	, pro_Activo BIT CONSTRAINT DF_pro_activo DEFAULT 1
	, pro_Codigo VARCHAR(20) NOT NULL
	, pro_Nombre VARCHAR(200) NOT NULL
	, pro_CantidadInventario INT NOT NULL
	, pro_CantidadReposicion INT NOT NULL CONSTRAINT DF_pro_cantidad DEFAULT 5
)

CREATE TABLE tid_TipoIdentificacion
(
	tid_Id INT CONSTRAINT PK_tid PRIMARY KEY
	, tid_FechaCreacion DATETIME NOT NULL
	, tid_FechaActualizacion DATETIME NOT NULL
	, tid_Activo BIT CONSTRAINT DF_tid_activo DEFAULT 1
	, tid_Codigo VARCHAR(20) NOT NULL
	, tid_Nombre VARCHAR(200) NOT NULL
)

CREATE TABLE cli_Cliente
(
	cli_Id INT CONSTRAINT PK_cli PRIMARY KEY IDENTITY(1, 1)
	, cli_FechaCreacion DATETIME NOT NULL
	, cli_FechaActualizacion DATETIME NOT NULL
	, cli_Activo BIT CONSTRAINT DF_cli_activo DEFAULT 1
	, tid_Id INT NOT NULL CONSTRAINT FK_cli_tid REFERENCES tid_TipoIdentificacion (tid_Id)
	, cli_Identificacion VARCHAR(20) NOT NULL
	, cli_Nombres VARCHAR(200) NOT NULL
	, cli_Apellidos VARCHAR(200) NOT NULL
	, cli_FechaNacimiento DATETIME NULL
	, cli_CorreoElectronico VARCHAR(200) NULL
)

CREATE TABLE fac_Factura
(
	fac_Id INT CONSTRAINT PK_fac PRIMARY KEY IDENTITY(1, 1)
	, fac_FechaCreacion DATETIME NOT NULL
	, fac_FechaActualizacion DATETIME NOT NULL
	, fac_Activo BIT CONSTRAINT DF_fac_activo DEFAULT 1
	, cli_Id INT NOT NULL CONSTRAINT FK_fac_cli REFERENCES cli_Cliente (cli_Id)
	, fac_Numero VARCHAR(20) NOT NULL
	, fac_FechaExpedicion DATETIME NOT NULL
)

CREATE TABLE fde_FacturaDetalle
(
	fde_Id INT CONSTRAINT PK_fde PRIMARY KEY IDENTITY(1, 1)
	, fde_FechaCreacion DATETIME NOT NULL
	, fde_FechaActualizacion DATETIME NOT NULL
	, fde_Activo BIT CONSTRAINT DF_fde_activo DEFAULT 1
	, fac_Id INT NOT NULL CONSTRAINT FK_fde_fac REFERENCES fac_Factura (fac_Id)
	, pro_Id INT NOT NULL CONSTRAINT FK_fde_pro REFERENCES pro_Producto (pro_Id)
	, fde_Cantidad INT NOT NULL
	, fde_PrecioUnitario MONEY NOT NULL
)

CREATE TABLE ppr_PrecioProducto
(
	ppr_Id INT CONSTRAINT PK_ppr PRIMARY KEY IDENTITY(1, 1)
	, ppr_FechaCreacion DATETIME NOT NULL
	, ppr_FechaActualizacion DATETIME NOT NULL
	, ppr_Activo BIT CONSTRAINT DF_ppr_activo DEFAULT 1
	, pro_Id INT NOT NULL CONSTRAINT FK_ppr_pro REFERENCES pro_Producto (pro_Id)
	, ppr_FechaInicioVigencia DATETIME NOT NULL INDEX IX_ppr_fechaVigencia NONCLUSTERED (ppr_FechaInicioVigencia)
	, ppr_PrecioUnitario MONEY NOT NULL
)

GO
CREATE VIEW dbo.vpf_PreciosFechas
AS
	WITH preciosFechas AS (
		SELECT 
			pro_Id
			, ppr.ppr_FechaInicioVigencia
			, ppr.ppr_PrecioUnitario
			, ROW_NUMBER() OVER (PARTITION BY pro_Id ORDER BY ppr.ppr_FechaInicioVigencia) RN
		FROM ppr_PrecioProducto ppr
		WHERE ppr.ppr_Activo = 1
	), preciosTiempo AS (
		SELECT 
			pfeI.pro_Id
			, pfeI.ppr_FechaInicioVigencia fechaInicio
			, pfeF.ppr_FechaInicioVigencia fechaFinal
			, pfeI.ppr_PrecioUnitario 
		FROM preciosFechas pfeI
		LEFT JOIN preciosFechas pfeF
			ON pfeI.pro_Id = pfeF.pro_Id
			AND pfeI.RN + 1 = pfeF.RN
	)
	SELECT
		pro_Id
		, fechaInicio vpf_fechaInicio
		, fechaFinal vpf_fechaFinal
		, ppr_PrecioUnitario vpf_PrecioUnitario
	FROM preciosTiempo

GO

GO
CREATE SEQUENCE dbo.SecuenciaNumeroFactura  
    AS INT
	START WITH 1  
    INCREMENT BY 1 ;  

GO
CREATE FUNCTION fnPrecioProductoObtener(@proId INT, @fecha DATE)
RETURNS MONEY
AS
BEGIN
	RETURN 0
END

GO
CREATE TRIGGER dbo.FacturaCrear ON fac_Factura
AFTER INSERT
AS
BEGIN
	UPDATE fac_Factura
	SET fac_Numero = 'F' + RIGHT('00000000000000000000' + CONVERT(VARCHAR(20), NEXT VALUE FOR dbo.SecuenciaNumeroFactura), 19)
	WHERE fac_Id IN (SELECT DISTINCT fac_Id FROM inserted)
END