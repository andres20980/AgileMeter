import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NombreProyectoComponent } from './nombre-proyecto.component';

describe('NombreProyectoComponent', () => {
  let component: NombreProyectoComponent;
  let fixture: ComponentFixture<NombreProyectoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NombreProyectoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NombreProyectoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
