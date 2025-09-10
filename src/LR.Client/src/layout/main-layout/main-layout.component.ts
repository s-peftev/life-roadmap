import { Component } from '@angular/core';
import { SidebarComponent } from "../../shared/components/sidebar/sidebar.component";
import { RouterOutlet } from '@angular/router';
import { SettingsLayoutComponent } from "../settings-layout/settings-layout.component";

@Component({
  selector: 'app-main-layout',
  imports: [SidebarComponent, RouterOutlet, SettingsLayoutComponent],
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.css'
})
export class MainLayoutComponent {
  public isSettingsOpen = false;

  closeSettings() { this.isSettingsOpen = false; }
  openSettings() { this.isSettingsOpen = true; }
}
