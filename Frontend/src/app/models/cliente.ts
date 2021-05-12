import { TipoIdentificacion } from "./tipo-identificacion";

export class Cliente {
    id: number;
    tidId: number;
    identificacion: string;
    nombres: string;
    apellidos: string;
    correoElectronico: string;
    fechaNacimiento: Date;
}
