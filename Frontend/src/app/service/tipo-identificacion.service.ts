import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TipoIdentificacionService {

  constructor(private http: HttpClient) { }

  public Listar(){
    const url = environment.API_URL + '/api/TipoIdentificacion';
    var respuesta = this.http.get(url)
    return respuesta;
  }
}
