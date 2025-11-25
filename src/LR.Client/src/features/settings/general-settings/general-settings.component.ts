import { Component } from '@angular/core';
import { AppLanguageDropdownComponent } from "../../../shared/components/app-language-dropdown/app-language-dropdown.component";
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-general-settings',
  imports: [
    AppLanguageDropdownComponent,
    TranslatePipe
  ],
  templateUrl: './general-settings.component.html'
})
export class GeneralSettingsComponent {

}
