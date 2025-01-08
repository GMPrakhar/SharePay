import { Routes } from '@angular/router';
import { GroupsComponent } from './groups/groups.component';
import { TransactionsComponent } from './transactions/transactions.component';
import { TransactionAddComponent } from './transaction-add/transaction-add.component';
import { GroupsAddComponent } from './groups-add/groups-add.component';

export const routes: Routes = [
    {
        path: '',
        loadComponent: () => GroupsComponent
    },
    {
        path: 'groups/:id/:name',
        loadComponent: () => TransactionsComponent
    },
    {
        path: 'add-transaction/:id/:name',
        loadComponent: () => TransactionAddComponent
    },
    {
        path: 'add-group',
        loadComponent: () => GroupsAddComponent
    }
];
