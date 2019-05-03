import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TeamsManagerComponent } from './teams-manager.component';

describe('TeamsManagerComponent', () => {
  let component: TeamsManagerComponent;
  let fixture: ComponentFixture<TeamsManagerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TeamsManagerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TeamsManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
