import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { AuthStore } from "../../features/auth/store/auth.store";
import { ROUTES } from "../constants/routes.constants";

export const AdminGuard: CanActivateFn = (route, state) => {
    const authStore = inject(AuthStore);
    const router = inject(Router);

    if(authStore.isAdmin()) return true;

    router.navigate([authStore.hasValidAccessToken() ? ROUTES.DASHBOARD : ROUTES.HOME]);
    return false;
}