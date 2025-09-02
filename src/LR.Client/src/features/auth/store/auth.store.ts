import { getState, patchState, signalStore, withComputed, withHooks, withMethods, withProps, withState } from "@ngrx/signals";
import { AuthSlice, initialAuthSlice } from "./auth.slice";
import { computed, effect, inject } from "@angular/core";
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { withDevtools } from "@angular-architects/ngrx-toolkit";
import { AuthService } from "../services/auth-service.service";
import { LoginRequest } from "../../../models/auth/login-request.model";
import { setAuthenticatedUser, setBusy } from "./auth.updaters";
import { exhaustMap, tap } from "rxjs";
import { tapResponse } from "@ngrx/operators";
import { AccessToken } from "../../../models/auth/access-token.model";
import { AuthResponse } from "../../../models/auth/auth-response.model";
import { Router } from "@angular/router";
import { ROUTES } from "../../../core/constants/routes.constants";

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
    withMethods((store) => ({
        login: rxMethod<LoginRequest>(input$ => input$.pipe(
            tap(_ => patchState(store, setBusy(true))),
            exhaustMap(request => store._authService.login(request)
                .pipe(
                    tapResponse({
                        next: response => patchState(store, setAuthenticatedUser(response)),
                        error: err => console.log(err),
                        finalize: () => patchState(store, setBusy(false))
                    })
                )
            )
        ))
    })),
    withHooks(store => ({
        onInit: () => {
            const router = inject(Router);
            const authInfoJson = localStorage.getItem('user');

            if (authInfoJson) {
                const authInfo = JSON.parse(authInfoJson) as AuthSlice;
                patchState(store, {
                    accessToken: authInfo.accessToken,
                    expiresAt: authInfo.expiresAt ? new Date(authInfo.expiresAt) : null,
                    user: authInfo.user
                })
            }

            effect(() => {
                const state = getState(store);
                if (!state.accessToken || !state.expiresAt || !state.user) return;

                const authInfo: AuthSlice = {
                    user: state.user,
                    accessToken: state.accessToken,
                    expiresAt: state.expiresAt,
                    isBusy: false
                };

                const authInfoJson = JSON.stringify(authInfo);

                localStorage.setItem('user', authInfoJson);
            });

            effect(() => {
                if (store.isAuthenticated()) {
                    setTimeout(() => router.navigate([ROUTES.DASHBOARD]));
                }
            });
        }
    })),
    withDevtools('auth-store')
);