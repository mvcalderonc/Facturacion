import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FacturaConsultarComponent } from './factura-consultar.component';

describe('FacturaConsultarComponent', () => {
  let component: FacturaConsultarComponent;
  let fixture: ComponentFixture<FacturaConsultarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FacturaConsultarComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FacturaConsultarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
