import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BalancesService } from '../common/balances.service';
import { UserViewModel } from '../models/UserViewModel';
import { CommonModule } from '@angular/common';
import { ClarityModule } from '@clr/angular';
import { GroupModel } from '../models/GroupModel';
import { Global } from '../common/Global';

@Component({
  selector: 'app-groups-add',
  standalone: true,
  imports: [CommonModule, ClarityModule, ReactiveFormsModule],
  templateUrl: './groups-add.component.html',
  styleUrls: ['./groups-add.component.scss']
})
export class GroupsAddComponent {
  groupForm: FormGroup;
  users: UserViewModel[] = [];

  constructor(private fb: FormBuilder, private balancesService: BalancesService, private router: Router) {
    this.groupForm = this.fb.group({
      name: ['', Validators.required],
      userEmails: ['', Validators.required]
    });
  }

  loadUsersFromEmail()
  {
    var userEmails = this.groupForm.value.userEmails.split(',');
    // Get the list of user data based on userEmails from balance service
    this.balancesService.getUsersByEmail(userEmails).subscribe(
      (users) => {
        this.users = users;
      },
      (error) => {
        alert('Failed to get users');
        console.error('Failed to get users', error);
      });
  }

  submitGroup(): void {
    if (this.groupForm.valid) {
      var groupModel = new GroupModel();
      groupModel.name = this.groupForm.value.name;
      groupModel.users = this.users;
      groupModel.owner = this.users.find(x => x.id == Global.USER_ID);
      this.balancesService.createGroup(groupModel).subscribe(
        () => {
          alert('Group created successfully');
          this.router.navigate(['/groups']);
        },
        (error) => {
          alert('Failed to create group');
          console.error('Failed to create group', error);
        }
      );
    }
  }
}