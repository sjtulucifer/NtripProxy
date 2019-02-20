import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountSYSAddComponent } from './account-sysadd.component';

describe('AccountSYSAddComponent', () => {
  let component: AccountSYSAddComponent;
  let fixture: ComponentFixture<AccountSYSAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccountSYSAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountSYSAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
