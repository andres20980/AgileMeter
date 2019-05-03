
import { fakeAsync, ComponentFixture, TestBed } from '@angular/core/testing';

import { SortedTableComponent } from './sorted-table.component';

describe('SortedTableComponent', () => {
  let component: SortedTableComponent;
  let fixture: ComponentFixture<SortedTableComponent>;

  beforeEach(fakeAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ SortedTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SortedTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should compile', () => {
    expect(component).toBeTruthy();
  });
});
