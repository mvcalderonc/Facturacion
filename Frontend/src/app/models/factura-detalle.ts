import { Producto } from "./producto";

export class FacturaDetalle {
    id:number;
    producto:Producto;
    cantidad:number;
    precioUnitario:number;
    subTotal:number;
}
