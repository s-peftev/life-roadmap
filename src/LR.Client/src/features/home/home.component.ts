import { Component } from '@angular/core';
import { ROUTES } from '../../core/constants/routes.constants';
import { RouterLink } from '@angular/router';
import { ASSETS } from '../../core/constants/assets.constants';

@Component({
  selector: 'app-home',
  imports: [
    RouterLink
  ],
  templateUrl: './home.component.html'
})
export class HomeComponent {
  public ROUTES = ROUTES;
  public titleImage = ASSETS.IMAGES.ILLUSTRATIONS.TITLE;
}
