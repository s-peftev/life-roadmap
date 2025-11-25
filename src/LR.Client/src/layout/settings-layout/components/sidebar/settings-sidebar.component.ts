import { Component, inject } from '@angular/core';
import { ROUTES } from '../../../../core/constants/routes.constants';
import { ASSETS } from '../../../../core/constants/assets.constants';
import { Router } from '@angular/router';
import { openSettingTab } from '../../shared/methods';
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-settings-sidebar',
  imports: [
    TranslatePipe
  ],
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
