import { Component } from '@angular/core';
import { RouterLink } from "@angular/router";
import { ROUTES } from '../../core/constants/routes.constants';


@Component({
  selector: 'app-nav',
  imports: [RouterLink],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  public ROUTES = ROUTES;
}
