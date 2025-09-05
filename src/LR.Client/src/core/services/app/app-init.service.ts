import { inject, Injectable } from '@angular/core';
import { AuthStore } from '../../../features/auth/store/auth.store';
import { catchError, map, Observable, of, switchMap, timer } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AppInitService {
  private authStore = inject(AuthStore);

  public initApp(): Observable<null> {
    const started = Date.now();

    return this.authStore.refresh().pipe(
      catchError(() => of(null)),
      switchMap(() => {
        const elapsed = Date.now() - started;
        const minDelay = 500;
        return elapsed < minDelay
          ? timer(minDelay - elapsed).pipe(map(() => null))
          : of(null);
      })
    );
  }
}