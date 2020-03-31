/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { PendingevaluationTableComponent } from './pendingevaluation-table.component';

describe('Pendingevaluation2TableComponent', () => {
  let component: PendingevaluationTableComponent;
  let fixture: ComponentFixture<PendingevaluationTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PendingevaluationTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PendingevaluationTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
