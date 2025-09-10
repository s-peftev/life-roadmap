import { Component, inject, output } from '@angular/core';
import { ASSETS } from '../../../core/constants/assets.constants';
import { AuthStore } from '../../../features/auth/store/auth.store';
import { Router, RouterLink, RouterLinkActive } from "@angular/router";
import { ROUTES } from '../../../core/constants/routes.constants';
import { Location } from '@angular/common';

@Component({
  selector: 'app-sidebar',
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
  private router = inject(Router);
  private location = inject(Location);

  public authStore = inject(AuthStore);

  public openSettings = output<void>();

  public routes = ROUTES;
  public icons = ASSETS.IMAGES.ICONS;

  public isSidebarOpen = true;

  public openSettingsPage() {
    this.openSettings.emit();
    this.location.go('/settings/roadmaps');
  }

  public toggleSidebar() {
    this.isSidebarOpen = !this.isSidebarOpen;
  }

  public logout() {
    this.authStore.logout();
  }
}
