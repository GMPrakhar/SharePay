import { Component, Input, OnInit } from '@angular/core';
import { BalancedTransactionModel } from '../models/BalancedTransactionModel';
import { BalancesService } from '../common/balances.service';
import { CommonModule } from '@angular/common';
import { ClarityModule } from '@clr/angular';

@Component({
  selector: 'app-balances',
  standalone: true,
  imports: [CommonModule, ClarityModule],
  templateUrl: './balances.component.html',
  styleUrls: ['./balances.component.scss']
})
export class BalancesComponent {
  @Input() groupId: string | null = '';
  balances: BalancedTransactionModel[] = [];

  constructor(private balancesService: BalancesService) {}

  ngOnInit()
  {
    this.loadBalances();
  }

  loadBalances(): void {
    this.balancesService.getBalances(this.groupId).subscribe(
      (balances) => {
        this.balances = balances;
      },
      (error) => {
        console.error('Failed to load balances', error);
      }
    );
  }

  settleUp(balance: BalancedTransactionModel): void {
    if (confirm(`Settle up ${balance.amount} from ${balance.from_name} to ${balance.to_name}?`)) {
      const transaction = {
        from_user: balance.from,
        to_users: [balance.to],
        division_strategy_per_user_unit: { [balance.to]: 1 },
        total_amount: balance.amount,
        description: 'Settle up',
        category: 'Settle'
      };

      this.balancesService.settleUp(this.groupId, transaction).subscribe(
        () => {
          alert('Transaction added successfully');
          this.balances = this.balances.filter(b => b !== balance);
        },
        (error) => {
          alert('Failed to add transaction');
          console.error('Failed to add transaction', error);
        }
      );
    }
  }
}