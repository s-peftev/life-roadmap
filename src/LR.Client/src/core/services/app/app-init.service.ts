import { inject, Injectable } from '@angular/core';
import { AuthStore } from '../../../features/auth/store/auth.store';
import { catchError, map, Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AppInitService {
  private authStore = inject(AuthStore);

  public initApp(): Observable<null> {
    return this.authStore.refresh().pipe(
      map(() => null),
      catchError(() => of(null))
    );
  }
}
