import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TransactionModel } from '../models/TransactionModel'; // Adjust the import path as necessary
import { CommonModule, formatDate } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Global } from '../common/Global';
import { ClarityModule } from '@clr/angular';
import { TransactionListComponent } from '../transaction-list/transaction-list.component';
import { BalancesComponent } from '../balances/balances.component';


@Component({
  selector: 'app-transactions',
  standalone: true,
  imports: [CommonModule, ClarityModule, TransactionListComponent, BalancesComponent],
  templateUrl: './transactions.component.html',
  styleUrl: './transactions.component.scss'
})
export class TransactionsComponent {
  groupName: string | null = '';
  id: string | null = '';
  transactions: TransactionModel[] = [];

  constructor(private router: ActivatedRoute, private httpClient: HttpClient, private route: Router)
  {
  }

  ngOnInit()
  {
    console.log(this.router.snapshot.paramMap)
    this.id = this.router.snapshot.paramMap.get('id');
    this.groupName = this.router.snapshot.paramMap.get('name');
    this.httpClient.get<TransactionModel[]>(`${Global.API_URL}/v1/groups/${this.id}/transactions?page=1&size=10&userId=${Global.USER_ID}`)
    .subscribe((result : TransactionModel[]) => {
      console.log(result);
      result.forEach(transaction => {
          if(transaction.category == 6)
          {
            transaction.transaction_info = 0;
          }
      });
      this.transactions = result;
    });
  }

  navigateToAddTransaction()
  {
    this.route.navigate(['/add-transaction', this.id, this.groupName]);
  }
}
