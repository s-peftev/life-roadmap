import { Component, inject } from '@angular/core';
import { ASSETS } from '../../../core/constants/assets.constants';
import { AuthStore } from '../../../features/auth/store/auth.store';

@Component({
  selector: 'app-sidebar',
  imports: [],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
  public authStore = inject(AuthStore);
  public icons = ASSETS.IMAGES.ICONS.PROFILE;
  public isSidebarOpen = true;

  public toggleSidebar() {
    this.isSidebarOpen = !this.isSidebarOpen;
  }

  public logout() {
    this.authStore.logout();
  }
}
