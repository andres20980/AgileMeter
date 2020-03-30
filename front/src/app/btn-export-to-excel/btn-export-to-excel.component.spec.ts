import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BtnExportToExcelComponent } from './btn-export-to-excel.component';

describe('BtnExportToExcelComponent', () => {
  let component: BtnExportToExcelComponent;
  let fixture: ComponentFixture<BtnExportToExcelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BtnExportToExcelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BtnExportToExcelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
