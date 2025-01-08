import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BalancedTransactionModel } from '../models/BalancedTransactionModel';
import { Global } from './Global';
import { UserViewModel } from '../models/UserViewModel';
import { TransactionModel } from '../models/TransactionModel';
import { GroupModel } from '../models/GroupModel';

@Injectable({
  providedIn: 'root'
})
export class BalancesService {
  private apiEndpoint = `${Global.API_URL}/v1/groups`;

  constructor(private http: HttpClient) {}

  getBalances(groupId: string | null): Observable<BalancedTransactionModel[]> {
    return this.http.get<BalancedTransactionModel[]>(`${this.apiEndpoint}/${groupId}/transactions/consolidated?userId=${Global.USER_ID}`);
  }

  settleUp(groupId: string | null, transaction: any): Observable<any> {
    return this.http.post(`${this.apiEndpoint}/${groupId}/transactions?userId=${Global.USER_ID}`, transaction);
  }
  
  getUsers(groupId : string | null): Observable<UserViewModel[]> {
    return this.http.get<UserViewModel[]>(`${this.apiEndpoint}/${groupId}/users?userId=${Global.USER_ID}`);
  }

  addTransaction(groupId: string | null, transaction: TransactionModel): Observable<any> {
    return this.http.post(`${this.apiEndpoint}/${groupId}/transactions?userId=${Global.USER_ID}`, transaction);
  }

  createGroup(group: GroupModel): Observable<any> {
    return this.http.post(`${this.apiEndpoint}`, group);
  }

  getUsersByEmail(emails: string[]): Observable<UserViewModel[]> {
    return this.http.post<UserViewModel[]>(`${Global.API_URL}/v1/users/list`, emails);
  }
}