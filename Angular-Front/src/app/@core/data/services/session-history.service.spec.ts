import { TestBed, inject } from '@angular/core/testing';

import { SessionHistoryService } from './session-history.service';

describe('SessionHistoryService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SessionHistoryService]
    });
  });

  it('should be created', inject([SessionHistoryService], (service: SessionHistoryService) => {
    expect(service).toBeTruthy();
  }));
});
