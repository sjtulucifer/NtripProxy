import { TestBed } from '@angular/core/testing';

import { HighLevelService } from './high-level.service';

describe('HighLevelService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: HighLevelService = TestBed.get(HighLevelService);
    expect(service).toBeTruthy();
  });
});
