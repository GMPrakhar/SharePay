import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TransactionAddComponent } from './transaction-add.component';

describe('TransactionAddComponent', () => {
  let component: TransactionAddComponent;
  let fixture: ComponentFixture<TransactionAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TransactionAddComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TransactionAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
