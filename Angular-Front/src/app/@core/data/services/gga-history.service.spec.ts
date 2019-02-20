import { TestBed, inject } from '@angular/core/testing';

import { GGAHistoryService } from './gga-history.service';

describe('GgaHistoryService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [GGAHistoryService]
    });
  });

  it('should be created', inject([GGAHistoryService], (service: GGAHistoryService) => {
    expect(service).toBeTruthy();
  }));
});
