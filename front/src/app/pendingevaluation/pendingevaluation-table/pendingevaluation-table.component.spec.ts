
import { fakeAsync, ComponentFixture, TestBed } from '@angular/core/testing';

import { PendingEvaluationTableComponent } from './pendingevaluation-table.component';

describe('SortedTableComponent', () => {
  let component: PendingEvaluationTableComponent;
  let fixture: ComponentFixture<PendingEvaluationTableComponent>;

  beforeEach(fakeAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ PendingEvaluationTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PendingEvaluationTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should compile', () => {
    expect(component).toBeTruthy();
  });
});
