import { Component } from '@angular/core';
import { GroupModel } from '../models/GroupModel';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { HttpClient, HttpClientModule, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { Global } from '../common/Global';
import { ClarityModule } from '@clr/angular';

@Component({
  selector: 'app-groups',
  standalone: true,
  imports: [CommonModule, ClarityModule],
  templateUrl: './groups.component.html',
  styleUrl: './groups.component.scss'
})
export class GroupsComponent {
  groups: GroupModel[] = [];

  constructor(private router: Router, private httpClient: HttpClient)
  {
    this.httpClient.get<GroupModel[]>(`${Global.API_URL}/v1/user/${Global.USER_ID}/groups`)
    .subscribe(result => {
      this.groups = result;
    });
    
  }

  navigateToAddGroup()
  {
    this.router.navigate(['/add-group']);
  }

  toTransactions(groupModel: GroupModel)
  {
    console.log("ToTransactions");
    this.router.navigate(['/groups', groupModel.id, groupModel.name]);
  }
}
