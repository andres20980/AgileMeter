/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { NobinaryComponent } from './nobinary.component';

describe('NobinaryComponent', () => {
  let component: NobinaryComponent;
  let fixture: ComponentFixture<NobinaryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NobinaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NobinaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
