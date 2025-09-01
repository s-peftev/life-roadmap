import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { ROUTES } from '../constants/routes.constants';

export const GuestGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const isAuth = true;

  if(!isAuth) return true;

  router.navigate([ROUTES.DASHBOARD]);
  return false;
};