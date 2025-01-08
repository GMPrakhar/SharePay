import { CommonModule, formatDate } from '@angular/common';
import { Component, Input } from '@angular/core';
import { ClarityModule } from '@clr/angular';
import { TransactionModel } from '../models/TransactionModel';

@Component({
  selector: 'app-transaction-list',
  standalone: true,
  imports: [CommonModule, ClarityModule],
  templateUrl: './transaction-list.component.html',
  styleUrl: './transaction-list.component.scss'
})
export class TransactionListComponent {

  @Input() transactions: TransactionModel[] = [];

  transactionCategoryToIconMapper(category: number): string
  {
    // Map TransactionCategory enum to corresponding clarity icons
    switch (category) {
      case 0:
        return 'wallet';
      case 1:
        return 'flame';
      case 2:
        return 'car';
      case 3:
        return 'shopping-cart';
      case 4:
        return 'scissors';
      case 5:
        return 'rupee';
      default:
        return 'general';
    }
  }

  toDateFormat(date: Date): string
  {
    return formatDate(date, 'MMM dd', 'en-US');
  }
}
