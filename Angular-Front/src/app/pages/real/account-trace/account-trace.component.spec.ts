import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountTraceComponent } from './account-trace.component';

describe('AccountTraceComponent', () => {
  let component: AccountTraceComponent;
  let fixture: ComponentFixture<AccountTraceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccountTraceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountTraceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
