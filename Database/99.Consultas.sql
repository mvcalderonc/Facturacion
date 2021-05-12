/********************************************************************************************
Obtener la lista de precios de todos los productos
********************************************************************************************/
SELECT
	pro.pro_Codigo
	, pro.pro_Nombre
	, vpf.vpf_PrecioUnitario
FROM pro_Producto pro
INNER JOIN vpf_PreciosFechas vpf
	ON pro.pro_Id = vpf.pro_Id
WHERE pro.pro_Activo = 1--Se filtran únicamente los productos válidos
AND vpf.vpf_fechaInicio <= GETDATE()
AND GETDATE() <= ISNULL(vpf.vpf_fechaFinal, '2999-1-1')
ORDER BY pro.pro_Nombre

/********************************************************************************************
Obtener la lista de productos cuya existencia en el inventario haya llegado al mínimo
permitido (5 unidades)
********************************************************************************************/
SELECT
	pro.pro_Codigo
	, pro.pro_Nombre
FROM pro_Producto pro
WHERE pro.pro_Activo = 1--Se filtran únicamente los productos válidos
AND pro.pro_CantidadInventario = pro.pro_CantidadReposicion
ORDER BY pro.pro_Nombre

/********************************************************************************************
Obtener una lista de clientes no mayores de 35 años que hayan realizado compras entre el
1 de febrero de 2000 y el 25 de mayo de 2000
********************************************************************************************/
;WITH clientesMenores35 AS (
	SELECT 
		cli.cli_Id
		, cli.cli_Identificacion
		, cli.cli_Nombres
		, cli.cli_Apellidos
	FROM cli_Cliente cli
	WHERE cli_Activo = 1
	AND DATEADD(YEAR, 35, cli.cli_FechaNacimiento) > GETDATE()
)
SELECT DISTINCT
	cli.cli_Id
	, cli.cli_Identificacion
	, cli.cli_Nombres
	, cli.cli_Apellidos
FROM clientesMenores35 cli
INNER JOIN fac_Factura fac
	ON cli.cli_Id = fac.cli_Id
WHERE CONVERT(DATE, '01/02/2000', 103) <= fac.fac_FechaExpedicion 
AND fac.fac_FechaExpedicion < CONVERT(DATE, '25/05/2000', 103)

/********************************************************************************************
Obtener el valor total vendido por cada producto en el año 2000
********************************************************************************************/
;WITH ventas2000 AS (
	SELECT 
		fac.fac_Id
		, fac.cli_Id
	FROM fac_Factura fac
	WHERE fac.fac_Activo = 1
	AND DATEPART(YEAR, fac.fac_FechaExpedicion) = 2000
)
SELECT
	pro.pro_Id
	, pro.pro_Codigo
	, pro.pro_Nombre
	, SUM(fde.fde_Cantidad * fde.fde_PrecioUnitario) TotalVentas2000
FROM pro_Producto pro
LEFT JOIN fde_FacturaDetalle fde
	ON pro.pro_Id = fde.pro_Id
LEFT JOIN ventas2000 fac
	ON fde.fde_Id = fac.fac_Id
WHERE pro.pro_Activo = 1
AND fde.fde_Activo = 1
GROUP BY pro.pro_Id
	, pro.pro_Codigo
	, pro.pro_Nombre
ORDER BY pro.pro_Nombre

/********************************************************************************************
Obtener la última fecha de compra de un cliente y según su frecuencia de compra estimar
en qué fecha podría volver a comprar.
********************************************************************************************/
;WITH datosCompras AS(
	SELECT
		cli.cli_Id
		, cli.cli_Identificacion
		, cli.cli_Nombres
		, cli.cli_Apellidos
		, COUNT(DISTINCT fac.fac_Id) NumeroCompras
		, MIN(fac.fac_FechaExpedicion) FechaPrimeraCompra
		, MAX(fac.fac_FechaExpedicion) FechaUltimaCompra
	FROM cli_Cliente cli
	INNER JOIN fac_Factura fac
		ON cli.cli_Id = fac.cli_Id
	WHERE cli.cli_Activo = 1
	AND fac.fac_Activo = 1
	GROUP BY cli.cli_Id
		, cli.cli_Identificacion
		, cli.cli_Nombres
		, cli.cli_Apellidos
)
SELECT 
	cli_Id
	, cli_Identificacion
	, cli_Nombres
	, cli_Apellidos
	, FechaUltimaCompra
	, DATEADD(D, DATEDIFF(DAY, FechaPrimeraCompra, FechaUltimaCompra)/NumeroCompras, FechaUltimaCompra) SiguienteCompra
FROM datosCompras