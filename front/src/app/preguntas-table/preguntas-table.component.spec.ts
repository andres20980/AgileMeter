
import { fakeAsync, ComponentFixture, TestBed } from '@angular/core/testing';

import { PreguntasTableComponent } from './preguntas-table.component';

describe('PreguntasTableComponent', () => {
  let component: PreguntasTableComponent;
  let fixture: ComponentFixture<PreguntasTableComponent>;

  beforeEach(fakeAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ PreguntasTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PreguntasTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should compile', () => {
    expect(component).toBeTruthy();
  });
});
