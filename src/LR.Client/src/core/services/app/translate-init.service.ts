import { inject, Injectable } from '@angular/core';
import { GeneralAppStore } from '../../../features/settings/general-settings/store/general-app.store';
import { InterpolatableTranslationObject, TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { STORAGE_KEYS } from '../../constants/storage-keys.constants';
import { AppLanguage } from '../../enums/app-language.enum';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TranslateInitService {
  private generalAppStore = inject(GeneralAppStore);
  private translateService =  inject(TranslateService);

  public initI18N(): Observable<InterpolatableTranslationObject> {
    const savedLanguage = localStorage.getItem(STORAGE_KEYS.APP_LANG) as AppLanguage | null;
    const initLanguage = savedLanguage || environment.defaultAppLanguage;

    this.generalAppStore.selectLanguage(initLanguage);

    return this.translateService.use(initLanguage);
  }
}
