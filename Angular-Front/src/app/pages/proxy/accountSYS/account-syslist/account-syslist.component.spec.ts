import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountSYSListComponent } from './account-syslist.component';

describe('AccountSYSListComponent', () => {
  let component: AccountSYSListComponent;
  let fixture: ComponentFixture<AccountSYSListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccountSYSListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountSYSListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
