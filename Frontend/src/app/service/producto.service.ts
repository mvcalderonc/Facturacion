import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProductoService {

  constructor(private http: HttpClient) { }

  public Consultar(codigo: string, nombre: string){
    const url = environment.API_URL + '/api/Producto/'+ codigo +'/' + nombre;
    var respuesta = this.http.get(url)
    return respuesta;
  }
}
