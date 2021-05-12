import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FacturaConsultarComponent } from './pages/factura-consultar/factura-consultar.component';
import { FacturaEditarComponent } from './pages/factura-editar/factura-editar.component';

const routes: Routes = [
    { path: '', redirectTo: 'factura-consultar', pathMatch: 'full' },
    { path: 'factura-consultar', component: FacturaConsultarComponent},
    { path: 'factura-editar/:id', component: FacturaEditarComponent},
    { path: '**', component: FacturaConsultarComponent },
  ];

  
@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule {}
