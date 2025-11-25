import { Component, inject } from '@angular/core';
import { AppLanguage } from '../../../core/enums/app-language.enum';
import { ASSETS } from '../../../core/constants/assets.constants';
import { GeneralAppStore } from '../../../features/settings/general-settings/store/general-app.store';

@Component({
  selector: 'app-language-dropdown',
  imports: [],
  templateUrl: './app-language-dropdown.component.html'
})
export class AppLanguageDropdownComponent {
  public generalAppStore = inject(GeneralAppStore);
  public langIcons = ASSETS.IMAGES.ICONS.I18N;

  public changeLanguage(lang: AppLanguage): void {
      this.generalAppStore.selectLanguage(lang);
    }
}
