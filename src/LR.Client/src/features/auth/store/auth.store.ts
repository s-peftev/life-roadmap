import { patchState, signalStore, withComputed, withMethods, withProps, withState } from "@ngrx/signals";
import { initialAuthSlice } from "./auth.slice";
import { computed, inject } from "@angular/core";
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { withDevtools } from "@angular-architects/ngrx-toolkit";
import { AuthService } from "../services/auth-service.service";
import { LoginRequest } from "../../../models/auth/login-request.model";
import { setAuthenticatedUser, setBusy } from "./auth.updaters";
import { exhaustMap, tap } from "rxjs";

export const AuthStore = signalStore(
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
        login: rxMethod<LoginRequest>(input$ => {
            return input$.pipe(
                tap(_ => patchState(store, setBusy(true))),
                exhaustMap(request => store._authService.login(request)),
                tap(response => patchState(store, setAuthenticatedUser(response), setBusy(false)))
            );
        })
    })),
    withDevtools('auth-store')
);