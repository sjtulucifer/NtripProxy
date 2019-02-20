import { TestBed, inject } from '@angular/core/testing';

import { AccountSYSService } from './account-sys.service';

describe('AccountSYSService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AccountSYSService]
    });
  });

  it('should be created', inject([AccountSYSService], (service: AccountSYSService) => {
    expect(service).toBeTruthy();
  }));
});
