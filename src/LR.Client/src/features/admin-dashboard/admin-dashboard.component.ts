import { DatePipe, NgFor } from '@angular/common';
import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { AdminStore } from './store/admin.store';
import { BusyComponent } from "../../shared/components/busy/busy.component";
import { ASSETS } from '../../core/constants/assets.constants';
import { UserForAdmin } from '../../models/admin/user-for-admin.model';
import { Role } from '../../core/enums/role.enum';
import { TranslatePipe } from '@ngx-translate/core';


@Component({
  selector: 'app-admin-dashboard',
  imports: [
    NgFor,
    BusyComponent,
    DatePipe,
    TranslatePipe
  ],
  templateUrl: './admin-dashboard.component.html',
})
export class AdminDashboardComponent implements OnInit {
  public adminStore = inject(AdminStore);
  public isBusy = computed(() => this.adminStore.isBusy());
  public icons = ASSETS.IMAGES.ICONS

  ngOnInit(): void {
    this.adminStore.loadUserList();
  }

  public getRolesString(user: UserForAdmin): string {
    return user.roles.map(role => Role[role]).join(', ');
  }

  public deleteUserPhoto(userId: string) {
    this.adminStore.deleteUserPhoto(userId);
  }
}
