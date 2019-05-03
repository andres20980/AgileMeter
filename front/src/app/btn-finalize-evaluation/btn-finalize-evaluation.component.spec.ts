import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BtnFinalizeEvaluationComponent } from './btn-finalize-evaluation.component';

describe('BtnFinalizeEvaluationComponent', () => {
  let component: BtnFinalizeEvaluationComponent;
  let fixture: ComponentFixture<BtnFinalizeEvaluationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BtnFinalizeEvaluationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BtnFinalizeEvaluationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
