/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { NoBinaryComponent } from './no-binary.component';

describe('NoBinaryComponent', () => {
  let component: NoBinaryComponent;
  let fixture: ComponentFixture<NoBinaryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NoBinaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NoBinaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
