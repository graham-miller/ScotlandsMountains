import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MountainTableComponent } from './mountain-table.component';

describe('MountainTableComponent', () => {
  let component: MountainTableComponent;
  let fixture: ComponentFixture<MountainTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MountainTableComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MountainTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
