import { Component, inject } from '@angular/core';
import { ASSETS } from '../../../../core/constants/assets.constants';
import { AuthStore } from '../../../auth/store/auth.store';
import { Router } from '@angular/router';
import { ROUTES } from '../../../../core/constants/routes.constants';

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
