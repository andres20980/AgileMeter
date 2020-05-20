/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { ChangeLangService } from './ChangeLang.service';

describe('Service: ChangeLang', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ChangeLangService]
    });
  });

  it('should ...', inject([ChangeLangService], (service: ChangeLangService) => {
    expect(service).toBeTruthy();
  }));
});
