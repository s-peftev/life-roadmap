import { Component, inject } from '@angular/core';
import { ROUTES } from '../../../../core/constants/routes.constants';
import { ASSETS } from '../../../../core/constants/assets.constants';
import { Router } from '@angular/router';
import { openSettingTab } from '../../shared/methods';

@Component({
  selector: 'app-settings-sidebar',
  imports: [],
  templateUrl: './settings-sidebar.component.html',
})
export class SettingsSidebarComponent {
  private router = inject(Router);

  public routes = ROUTES;
  public icons = ASSETS.IMAGES.ICONS

  openTab(settingTabName: string): void {
    openSettingTab(settingTabName, this.router);
  }
}
