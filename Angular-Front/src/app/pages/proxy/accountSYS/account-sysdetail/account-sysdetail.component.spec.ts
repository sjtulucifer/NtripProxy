import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountSYSDetailComponent } from './account-sysdetail.component';

describe('AccountSYSDetailComponent', () => {
  let component: AccountSYSDetailComponent;
  let fixture: ComponentFixture<AccountSYSDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccountSYSDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountSYSDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
