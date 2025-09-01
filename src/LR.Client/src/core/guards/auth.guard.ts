import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { ROUTES } from '../constants/routes.constants';

export const AuthGuard: CanActivateFn = (route, state) => {
  const isAuth = true;
  const router = inject(Router);

  if(isAuth) return true;
  
  router.navigate([ROUTES.AUTH.LOGIN]);
  return false;
};
