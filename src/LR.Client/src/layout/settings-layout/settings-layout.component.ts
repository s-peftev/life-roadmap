import { Component, inject } from '@angular/core';
import { Router, RouterOutlet } from "@angular/router";
import { SettingsSidebarComponent } from './components/sidebar/settings-sidebar.component';

@Component({
  selector: 'app-settings-layout',
  imports: [RouterOutlet, SettingsSidebarComponent],
  templateUrl: './settings-layout.component.html',
})
export class SettingsLayoutComponent {
  private router = inject(Router);

  close() {
    this.router.navigate([{ outlets: { modal: null } }]);
  }
}
