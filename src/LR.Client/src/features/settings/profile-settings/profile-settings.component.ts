import { Component, inject } from '@angular/core';
import { ASSETS } from '../../../core/constants/assets.constants';
import { ROUTES } from '../../../core/constants/routes.constants';
import { ProfileStore } from './store/profile.store';
import { BusyComponent } from "../../../shared/components/busy/busy.component";

@Component({
  selector: 'app-profile-settings',
  imports: [BusyComponent],
  templateUrl: './profile-settings.component.html'
})
export class ProfileSettingsComponent {
  public profileStore = inject(ProfileStore);

  public routes = ROUTES;
  public icons = ASSETS.IMAGES.ICONS

  public onFileSelected(event: Event) {
  const input = event.target as HTMLInputElement;
  if (!input.files?.length) return;

  const file: File = input.files[0];
  this.profileStore.uploadProfilePhoto(file);
}
}
