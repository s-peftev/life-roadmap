import { Component } from '@angular/core';
import { RouterLink } from "@angular/router";
import { ROUTES } from '../../../../core/constants/routes.constants';
import { TranslatePipe } from '@ngx-translate/core';
import { AppLanguageDropdownComponent } from "../../../../shared/components/app-language-dropdown/app-language-dropdown.component";

@Component({
  selector: 'app-nav',
  imports: [
    RouterLink,
    TranslatePipe,
    AppLanguageDropdownComponent
],
  templateUrl: './nav.component.html'
})
export class NavComponent {
  public ROUTES = ROUTES;
}
