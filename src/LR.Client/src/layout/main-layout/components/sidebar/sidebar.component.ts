import { Component, inject, output } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from "@angular/router";
import { ROUTES } from '../../../../core/constants/routes.constants';
import { AuthStore } from '../../../../features/auth/store/auth.store';
import { ASSETS } from '../../../../core/constants/assets.constants';
import { ProfileStore } from '../../../../features/settings/profile-settings/store/profile.store';
import { openSettingTab } from '../../../settings-layout/shared/methods';


@Component({
  selector: 'app-sidebar',
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './sidebar.component.html',
})
export class SidebarComponent {
  private router = inject(Router);

  public authStore = inject(AuthStore);
  public profileStore = inject(ProfileStore);
  
  public openSettings = output<void>();
  public routes = ROUTES;
  public icons = ASSETS.IMAGES.ICONS;
  public isSidebarOpen = true;

  public openSettingsPage() {
    openSettingTab(ROUTES.SETTINGS.PROFILE, this.router);
  }

  public toggleSidebar() {
    this.isSidebarOpen = !this.isSidebarOpen;
  }

  public logout() {
    this.authStore.logout();
  }
}
