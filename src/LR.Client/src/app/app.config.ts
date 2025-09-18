import { ApplicationConfig, inject, provideAppInitializer } from '@angular/core';
import { provideRouter, UrlSerializer } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { AppInitService } from '../core/services/app/app-init.service';
import { lastValueFrom } from 'rxjs';
import { authInterceptor } from '../core/interceptors/auth.interceptor';
import { ModalOutletUrlSerializer } from '../core/routing/modal-outlet-url.serializer';


export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptors([authInterceptor])),
    provideAppInitializer(async () => {
      const appInitService = inject(AppInitService);

      return lastValueFrom(appInitService.initApp())
    }),
    { provide: UrlSerializer, useClass: ModalOutletUrlSerializer }
  ]
};
