import { Cliente } from "./cliente";
import { FacturaDetalle } from "./factura-detalle";

export class Factura {
    id: number;
    cliente: Cliente;
    fechaExpedicion: Date;
    numero: string;
    listaFacturaDetalles: FacturaDetalle[];
    valorTotal:number;
}
