import { HttpErrorResponse, HttpEvent, HttpHandlerFn, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthStore } from '../../features/auth/store/auth.store';
import { BehaviorSubject, catchError, filter, finalize, Observable, of, switchMap, take, tap, throwError } from 'rxjs';
import { ROUTES } from '../constants/routes.constants';

let isRefreshing = false;
let refreshTokenSubject = new BehaviorSubject<string | null>(null);

export const authInterceptor: HttpInterceptorFn = (
  req: HttpRequest<any>,
  next: HttpHandlerFn
): Observable<HttpEvent<any>> => {
  const authStore = inject(AuthStore);
  const accessToken = authStore.accessToken();

  let request = req;

  if (accessToken) {
    request = addTokenHeader(request, accessToken);
  }

  return next(request).pipe(
    catchError((err: HttpErrorResponse) => {
      if(err.status === 401 && !request.url.includes(ROUTES.AUTH.LOGIN)) {
        if(!isRefreshing) {
          isRefreshing = true;
          refreshTokenSubject.next(null);

          return authStore.refresh().pipe(
            switchMap(response => {
              refreshTokenSubject.next(response.accessToken.tokenValue);
              return next(addTokenHeader(request, response.accessToken.tokenValue));
            }),
            catchError((refreshErr) => {
              authStore.logout();
              return throwError(() => refreshErr);
            }),
            finalize(() => isRefreshing = false)
          );
        } else {
          return refreshTokenSubject.pipe(
            filter(token => token !== null),
            take(1),
            switchMap(token => next(addTokenHeader(request, token)))
          );
        }
      } else {
        return throwError(() => err);
      }
    })
  );
};

function addTokenHeader(request: HttpRequest<any>, accessToken: string) {
  return request.clone({
    setHeaders: { Authorization: `Bearer ${accessToken}` }
  });
}
