
import { fakeAsync, ComponentFixture, TestBed } from '@angular/core/testing';

import { ComentariosTableComponent } from './comentarios-table.component';

describe('ComentariosTableComponent', () => {
  let component: ComentariosTableComponent;
  let fixture: ComponentFixture<ComentariosTableComponent>;

  beforeEach(fakeAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ ComentariosTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ComentariosTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should compile', () => {
    expect(component).toBeTruthy();
  });
});
