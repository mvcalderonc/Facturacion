import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Factura } from '../models/factura';

@Injectable({
  providedIn: 'root'
})
export class FacturaService {

  constructor(private http: HttpClient) { }

  public Obtener(facId: string){
    const url = environment.API_URL + '/api/Factura/' + facId;
    var respuesta = this.http.get(url)
    return respuesta;
  }

  public Consultar(numero: string){
    const url = environment.API_URL + '/api/Factura/0/' + numero;
    var respuesta = this.http.get(url)
    return respuesta;
  }

  public Guardar(factura: {factura: Factura}){
    const url = environment.API_URL + '/api/Factura/';
    console.log(factura)
    var respuesta = this.http.post(url, factura);
    return respuesta;
  }

  public Actualizar(facId: string, factura: {factura: Factura}){
    const url = environment.API_URL + '/api/Factura/' + facId;
    console.log(factura)
    var respuesta = this.http.put(url, factura);
    return respuesta;
  }

  public Eliminar(facId: string){
    const url = environment.API_URL + '/api/Factura/' + facId;
    var respuesta = this.http.delete(url)
    return respuesta;
  }
}
