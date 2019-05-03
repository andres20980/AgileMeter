import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PendingEvaluationComponent } from './pendingevaluation.component';

describe('PreviousevaluationComponent', () => {
  let component: PendingEvaluationComponent;
  let fixture: ComponentFixture<PendingEvaluationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PendingEvaluationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PendingEvaluationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
