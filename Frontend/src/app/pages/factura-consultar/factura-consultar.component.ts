import { Component, OnInit } from '@angular/core';
import { Factura } from 'src/app/models/factura';
import { FacturaService } from '../../service/factura.service'

@Component({
  selector: 'app-factura-consultar',
  templateUrl: './factura-consultar.component.html',
  styleUrls: ['./factura-consultar.component.css']
})
export class FacturaConsultarComponent implements OnInit {
  facturaNumero: string;
  facturas: Factura[];
  
  constructor(private facturaServicio: FacturaService) { }

  ngOnInit(): void {
    
  }
  
  FacturaConsultar(facturaNumero){
    this.facturaNumero = facturaNumero.value;
    this.FacturaConsultaEjecutar(this.facturaNumero)
  }

  FacturaConsultaEjecutar(facturaNumero){
    this.facturaServicio.Consultar(facturaNumero).subscribe((data)=>{
      if (data['estado'] != 1){
        alert(data["mensaje"]);
      }
      else
      {
        
        this.facturas = data['listaFacturas'];
      }
    });
  }

  FacturaEliminar(facId)
  {
    this.facturaServicio.Eliminar(facId).subscribe((data)=>{
      if (data['estado'] != 1){
        alert(data["mensaje"]);
      }
      else
      {
        this.FacturaConsultaEjecutar(this.facturaNumero)
      }
    });
  }
}
