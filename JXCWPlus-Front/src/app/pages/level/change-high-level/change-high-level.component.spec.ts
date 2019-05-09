import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangeHighLevelComponent } from './change-high-level.component';

describe('ChangeHighLevelComponent', () => {
  let component: ChangeHighLevelComponent;
  let fixture: ComponentFixture<ChangeHighLevelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChangeHighLevelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChangeHighLevelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
