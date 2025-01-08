import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupsAddComponent } from './groups-add.component';

describe('GroupsAddComponent', () => {
  let component: GroupsAddComponent;
  let fixture: ComponentFixture<GroupsAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GroupsAddComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GroupsAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
