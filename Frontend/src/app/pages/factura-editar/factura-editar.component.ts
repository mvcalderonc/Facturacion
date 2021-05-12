import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Cliente } from 'src/app/models/cliente';
import { Factura } from 'src/app/models/factura';
import { FacturaDetalle } from 'src/app/models/factura-detalle';
import { TipoIdentificacion } from 'src/app/models/tipo-identificacion';
import { FacturaService } from 'src/app/service/factura.service';
import { TipoIdentificacionService } from '../../service/tipo-identificacion.service'
import { ActivatedRoute } from '@angular/router'
import { Producto } from 'src/app/models/producto';
import { ProductoService } from 'src/app/service/producto.service';
import { FormGroup, FormControl } from '@angular/forms';
import { Variable } from '@angular/compiler/src/render3/r3_ast';

@Component({
  selector: 'app-factura-editar',
  templateUrl: './factura-editar.component.html',
  styleUrls: ['./factura-editar.component.css']
})
export class FacturaEditarComponent implements OnInit {
  facturaDatos = new FormGroup({
    comboTipoIdentificacion: new FormControl(''),
    textIdentificacion: new FormControl(''),
    textNombres: new FormControl(''),
    textApellidos: new FormControl(''),
    textFechaNacimiento: new FormControl(''),
    textCorreoElectronico: new FormControl(''),
  });

  tiposIdentificacion;
  factura: Factura;
  listaProductos: Producto[];
  productoSeleccionado: Producto;

  constructor(private activatedRoute: ActivatedRoute
    , private tipoIdentificacionServicio: TipoIdentificacionService
    , private facturaServicio: FacturaService
    , private productoServicio: ProductoService) { }

  ngOnInit(): void {
    
    this.factura = new Factura();
    this.factura.id = 0;
    this.factura.numero = ''
    this.factura.fechaExpedicion = new Date();
    this.factura.valorTotal= 0
    this.factura.cliente = new Cliente();
    this.factura.cliente.id = 0;
    this.factura.cliente.tidId = 2;
    this.factura.cliente.identificacion = '';
    this.factura.cliente.nombres = '';
    this.factura.cliente.apellidos = '';
    this.factura.cliente.fechaNacimiento = new Date();
    this.factura.cliente.correoElectronico = '';
    this.factura.listaFacturaDetalles = [];
    
    this.CargarTiposIdentificacion();

    var facId;
    const id = this.activatedRoute.params.subscribe(param => {
      facId = param.id;
    });
    

    if(facId != 0)
    {
      this.FacturaObtener(facId);      
    }
  }

  CargarTiposIdentificacion(){
    this.tipoIdentificacionServicio.Listar().subscribe((data)=>{
      console.log(data);
      console.log("estado");
      console.log(data['estado']);
      if (data['estado'] != 1){
        console.log(data["mensaje"])

      }
      else
      {
        
        this.tiposIdentificacion = data['listaTiposIdentificacion'];
        console.log("dasd");
        console.log(this.tiposIdentificacion);
      }
    });
  }

  FacturaObtener(facId)
  {
    console.log("consultar factura");
    this.facturaServicio.Obtener(facId).subscribe((data)=>{
      if (data['estado'] != 1){

      }
      else
      {
        
        this.factura = data['listaFacturas'][0];
        this.factura.listaFacturaDetalles.forEach(function (value) {
          value.subTotal = value.precioUnitario * value.cantidad;
        }); 
        this.FacturaDatosPasar()
      }
    });
  }

  FacturaDatosPasar()
  {
    this.facturaDatos.patchValue({
      comboTipoIdentificacion: this.factura.cliente.tidId,
      textIdentificacion: this.factura.cliente.identificacion,
      textNombres: this.factura.cliente.nombres,
      textApellidos: this.factura.cliente.apellidos,
      textFechaNacimiento: this.factura.cliente.fechaNacimiento,
      textCorreoElectronico: this.factura.cliente.correoElectronico,
    });
  }

  FacturaGuardar()
  {
    this.factura.cliente.tidId = Number(this.facturaDatos.value.comboTipoIdentificacion);
    this.factura.cliente.identificacion = this.facturaDatos.value.textIdentificacion;
    this.factura.cliente.nombres = this.facturaDatos.value.textNombres;
    this.factura.cliente.apellidos = this.facturaDatos.value.textApellidos;
    // this.factura.cliente.fechaNacimiento = this.facturaDatos.value.textFechaNacimiento;
    this.factura.cliente.correoElectronico = this.facturaDatos.value.textCorreoElectronico;

    const contenedor = {
      factura: {...this.factura}
    };

    if(this.factura.id == 0){
      console.log('crear')
      this.facturaServicio.Guardar(contenedor).subscribe((data)=>{
        if (data['estado'] != 1){
          console.log(data["mensaje"])
          alert(data["mensaje"]);
        }
        else
        {
          console.log(data["factura"]);
        }
      });
    }
    else{
      console.log('actualizar')
      this.facturaServicio.Actualizar(this.factura.id.toString(), contenedor).subscribe((data)=>{
        if (data['estado'] != 1){
          console.log(data["mensaje"])
          alert(data["mensaje"]);
        }
        else
        {
          console.log(data["factura"]);
        }
      });
    }
  }

  ProductoAgregar(){
    this.productoSeleccionado = null;
  }

  ProductoConsultar(codigo, nombre)
  {
    this.productoServicio.Consultar(codigo.value, nombre.value).subscribe((data)=>{
      console.log(data);
      console.log("estado");
      console.log(data['estado']);
      if (data['estado'] != 1){
        console.log(data["mensaje"])

      }
      else
      {
        
        this.listaProductos = data['listaProductos'];
        console.log(this.listaProductos);
      }
    });
  }

  ProductoSeleccionar(productoSeleccionado)
  {
    this.productoSeleccionado = productoSeleccionado;
  }

  @ViewChild('closeModal') private closeModal: ElementRef;
  ProductoAceptar(cantidad)
  {
    if(this.productoSeleccionado != null)
    {
      const unidades = cantidad.value
      
      const facturaDetalle = new FacturaDetalle();
      facturaDetalle.producto = this.productoSeleccionado;
      facturaDetalle.cantidad = Number(unidades);
      facturaDetalle.precioUnitario = this.productoSeleccionado.precioUnitario;
      facturaDetalle.subTotal = facturaDetalle.precioUnitario * facturaDetalle.cantidad;

      this.factura.listaFacturaDetalles.push(facturaDetalle);

      var total = 0;
      this.factura.listaFacturaDetalles.forEach(function (value) {
        total = total + value.subTotal;
      }); 
      this.factura.valorTotal = total;
      this.closeModal.nativeElement.click(); 
    }
    else
    {
      alert("Debe seleccionar un producto");
    }
  }
}
