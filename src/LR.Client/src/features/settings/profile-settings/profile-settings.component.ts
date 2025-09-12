import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { ASSETS } from '../../../core/constants/assets.constants';
import { ROUTES } from '../../../core/constants/routes.constants';
import { AuthStore } from '../../auth/store/auth.store';

@Component({
  selector: 'app-profile-settings',
  imports: [],
  templateUrl: './profile-settings.component.html'
})
export class ProfileSettingsComponent {
  public icons = ASSETS.IMAGES.ICONS
  public authStore = inject(AuthStore);
  public routes = ROUTES;
  private router = inject(Router);

}
