import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { BalancesService } from '../common/balances.service';
import { UserViewModel } from '../models/UserViewModel';
import { TransactionCategory, TransactionModel } from '../models/TransactionModel';
import { ActivatedRoute, Router } from '@angular/router';
import { ClarityModule } from '@clr/angular';
import { CommonModule } from '@angular/common';
import { Global } from '../common/Global';

@Component({
  selector: 'app-transaction-add',
  standalone: true,
  imports: [CommonModule, ClarityModule, FormsModule, ReactiveFormsModule],
  templateUrl: './transaction-add.component.html',
  styleUrls: ['./transaction-add.component.scss']
})
export class TransactionAddComponent implements OnInit {
  groupID: string | null = '';
  groupName: string | null = '';
  transactionForm: FormGroup;
  categories = Object.keys(TransactionCategory);
  users: UserViewModel[] = [];
  selectedUsers: UserViewModel[] = [];

  constructor(private fb: FormBuilder, private balancesService: BalancesService, private router: ActivatedRoute, private route: Router) {
    this.transactionForm = this.fb.group({
      description: ['', Validators.required],
      total_amount: [0, [Validators.required, Validators.min(0.01)]],
      category: ['', Validators.required],
      selected_users: [[], Validators.required],
      from_user: ['', Validators.required] // New field added here
    });
  }

  ngOnInit(): void {
    this.groupID = this.router.snapshot.paramMap.get('id');
    this.groupName = this.router.snapshot.paramMap.get('name');
    this.loadUsers();
  }

  loadUsers(): void {
    this.balancesService.getUsers(this.groupID).subscribe(
      (users) => {
        console.log(users);
        this.users = users;
      },
      (error) => {
        console.error('Failed to load users', error);
      }
    );
  }

  onUserSelectionChange(event: Event, user: UserViewModel): void {
    const target = event.target as HTMLInputElement;
    if (target && target.checked !== undefined) {
      this.selectedUsers = target.checked ? [...this.selectedUsers, user] : this.selectedUsers.filter(u => u !== user);
      this.transactionForm.patchValue({ selected_users: this.selectedUsers });
    }
  }

  submitTransaction(): void {
    if (this.transactionForm.valid) {
      const transaction = {
        ...this.transactionForm.value,
        to_users: this.selectedUsers.map(user => user.id),
        division_strategy_per_user_unit: this.selectedUsers.reduce((acc, user) => {
          acc[user.id] = 1;
          return acc;
        }, {} as any),
      };

      this.balancesService.addTransaction(this.groupID, transaction).subscribe(
        () => {
          alert('Transaction added successfully');
          this.route.navigate(['/groups', this.groupID, this.groupName]);
        },
        (error) => {
          alert('Failed to add transaction');
          console.error('Failed to add transaction', error);
        }
      );
    }
  }
}