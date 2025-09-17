import { NgFor } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { AuthStore } from '../auth/store/auth.store';
import { MyProfileResponse } from '../../models/user-profile/my-profile-response.model';


@Component({
  selector: 'app-admin-dashboard',
  imports: [
    NgFor,
  ],
  templateUrl: './admin-dashboard.component.html',
})
export class AdminDashboardComponent {
  public authStore = inject(AuthStore);
  public users = signal<MyProfileResponse[]>([]);

  public toggleRole(user: MyProfileResponse) {}
  public changePassword(user: MyProfileResponse) {}
}
