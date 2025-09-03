import { getState, patchState, signalStore, withComputed, withHooks, withMethods, withProps, withState } from "@ngrx/signals";
import { AuthSlice, initialAuthSlice } from "./auth.slice";
import { computed, effect, inject } from "@angular/core";
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { withDevtools } from "@angular-architects/ngrx-toolkit";
import { AuthService } from "../services/auth-service.service";
import { LoginRequest } from "../../../models/auth/login-request.model";
import { setAuthenticatedUser, setBusy, setError } from "./auth.updaters";
import { exhaustMap, finalize, map, tap } from "rxjs";
import { tapResponse } from "@ngrx/operators";
import { Router } from "@angular/router";
import { ROUTES } from "../../../core/constants/routes.constants";
import { ApiError, DefaultErrors, isApiError } from "../../../models/api/api-error.model";

export const AuthStore = signalStore(
    { providedIn: 'root' },
    withState(initialAuthSlice),
    withProps(() => {
        const _authService = inject(AuthService);

        return {
            _authService
        }
    }),
    withComputed((store) => {
        const isAuthenticated = computed(() =>
            !!store.accessToken() && !!store.user());
        const hasValidAccessToken = computed(() =>
            !!store.accessToken() && !!store.user() && !!store.expiresAt() && store.expiresAt()! > new Date());

        return {
            isAuthenticated,
            hasValidAccessToken
        };
    }),
    withMethods((store) => {
        const router = inject(Router);

        return {
            login: rxMethod<LoginRequest>(input$ => input$.pipe(
                tap(_ => patchState(store, setBusy(true))),
                exhaustMap(request =>
                    store._authService.login(request).pipe(
                        tapResponse({
                            next: response => patchState(store, setAuthenticatedUser(response)),
                            error: err => {
                                if (isApiError(err)) {
                                    const apiErr = err as ApiError;
                                    patchState(store, setError(apiErr));
                                } else {
                                    patchState(store, setError(DefaultErrors.UnexpectedError))
                                }
                            },
                            finalize: () => {
                                patchState(store, setBusy(false));
                                router.navigate([ROUTES.DASHBOARD]);
                            }
                        })
                    )
                )
            )),

            logout: rxMethod<void>(trigger$ => trigger$.pipe(
                tap(_ => patchState(store, setBusy(true))),
                exhaustMap(_ =>
                    store._authService.logout().pipe(
                        finalize(() => {
                            patchState(store, initialAuthSlice);
                            router.navigate([ROUTES.AUTH.LOGIN]);
                        })
                    )
                )
            )),

            refresh: rxMethod<void>(trigger$ => trigger$.pipe(
                exhaustMap(() =>
                    store._authService.refresh().pipe(
                        tapResponse({
                            next: response => patchState(store, setAuthenticatedUser(response)),
                            error: () => {
                                patchState(store, initialAuthSlice);
                                router.navigate([ROUTES.AUTH.LOGIN]);
                            }
                        })
                    )
                )
            ))
        };
    }),
    withHooks(store => ({
        onInit: () => {
            store.refresh();
        }
    })),
    withDevtools('auth-store')
);