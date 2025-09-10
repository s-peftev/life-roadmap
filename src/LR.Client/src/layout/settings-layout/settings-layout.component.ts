import { Location } from '@angular/common';
import { Component, inject, output } from '@angular/core';
import { RouterLink, RouterOutlet } from "@angular/router";

@Component({
  selector: 'app-settings-layout',
  imports: [RouterOutlet, RouterLink],
  templateUrl: './settings-layout.component.html',
  styleUrl: './settings-layout.component.css'
})
export class SettingsLayoutComponent {
  private location = inject(Location);

  public closeSettings = output<void>();

  close() {
    this.closeSettings.emit();
    this.location.back();
  }
}
