import { Component, inject } from '@angular/core';
import { ASSETS } from '../../../core/constants/assets.constants';
import { ROUTES } from '../../../core/constants/routes.constants';
import { ProfileStore } from './store/profile.store';
import { BusyComponent } from "../../../shared/components/busy/busy.component";
import { FormBuilder, FormControl } from '@angular/forms';
import { fileSizeValidator, fileTypeValidator } from '../../../shared/validators/file.validator';
import { KeyValuePipe, NgFor } from '@angular/common';

@Component({
  selector: 'app-profile-settings',
  imports: [BusyComponent, NgFor, KeyValuePipe],
  templateUrl: './profile-settings.component.html'
})
export class ProfileSettingsComponent {
  private fb = inject(FormBuilder);
  public profileStore = inject(ProfileStore);

  public routes = ROUTES;
  public icons = ASSETS.IMAGES.ICONS

  fileControl = new FormControl<File | null>(null, [
    fileSizeValidator(4 * 1024 * 1024, 'That photo is a bit too large. Try something smaller.'),
    fileTypeValidator(['image/png', 'image/jpeg'])
  ]);

  public onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (!input.files?.length) return;

    const file: File = input.files[0];
    this.fileControl.setValue(file);

    if(this.fileControl.valid)
      this.profileStore.uploadProfilePhoto(file);
  }

  public deleteProfilePhoto() {
    this.profileStore.deleteProfilePhoto();
  }
}
