import { ApplicationConfig, inject, provideAppInitializer } from '@angular/core';
import { provideRouter, UrlSerializer } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { AppInitService } from '../core/services/app/app-init.service';
import { lastValueFrom } from 'rxjs';
import { authInterceptor } from '../core/interceptors/auth.interceptor';
import { ModalOutletUrlSerializer } from '../core/routing/modal-outlet-url.serializer';
import { provideTranslateService } from '@ngx-translate/core';
import { provideTranslateHttpLoader } from '@ngx-translate/http-loader';
import { AppLanguage } from '../core/enums/app-language.enum';
import { TranslateInitService } from '../core/services/app/translate-init.service';
import { environment } from '../environments/environment';


export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(
      withInterceptors([authInterceptor])
    ),
    provideTranslateService({
      loader: provideTranslateHttpLoader({
        prefix: '/assets/i18n/',
        suffix: '.json'
      }),
      fallbackLang: environment.defaultAppLanguage
    }),
    provideAppInitializer(async () => {
      const appInitService = inject(AppInitService);

      return lastValueFrom(appInitService.initApp())
    }),
    provideAppInitializer(async () => {
      const translateInitService = inject(TranslateInitService);

      return lastValueFrom(translateInitService.initI18N());
    }),
    { provide: UrlSerializer, useClass: ModalOutletUrlSerializer }
  ]
};
