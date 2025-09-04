import { getState, patchState, signalStore, withComputed, withHooks, withMethods, withProps, withState } from "@ngrx/signals";
import { AuthSlice, initialAuthSlice } from "./auth.slice";
import { computed, effect, inject } from "@angular/core";
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { withDevtools } from "@angular-architects/ngrx-toolkit";
import { AuthService } from "../services/auth-service.service";
import { LoginRequest } from "../../../models/auth/login-request.model";
import { setAuthUser, setAuthUserWithJwt, setBusy, setError } from "./auth.updaters";
import { exhaustMap, finalize, map, Observable, tap } from "rxjs";
import { tapResponse } from "@ngrx/operators";
import { Router } from "@angular/router";
import { ROUTES } from "../../../core/constants/routes.constants";
import { ApiError, DefaultErrors, isApiError } from "../../../models/api/api-error.model";
import { User } from "../../../models/auth/user.model";
import { ErrorType } from "../../../core/enums/error-type.enum";
import { AuthResponse } from "../../../models/auth/auth-response.model";

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
        const hasValidAccessToken = computed(() =>
            !!store.accessToken() && !!store.user() && !!store.expiresAt() && store.expiresAt()! > new Date());

        return {
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
                            next: response => patchState(store, setAuthUserWithJwt(response)),
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
                            localStorage.removeItem('user');
                            router.navigate([ROUTES.AUTH.LOGIN]);
                        })
                    )
                )
            )),

            refresh: (): Observable<AuthResponse> => {
                return store._authService.refresh().pipe(
                    tap({
                        next: (response) => patchState(store, setAuthUserWithJwt(response)),
                        error: (err) => {
                            if (!isApiError(err)) return;

                            const apiErr = err as ApiError;
                            if (apiErr.type === ErrorType.Unauthorized) {
                                router.navigate([ROUTES.AUTH.LOGIN]);
                                localStorage.removeItem('user'); // doesn`t work as expected
                                patchState(store, initialAuthSlice);
                            }
                        }
                    })
                );
            },
        };
    }),
    withHooks(store => ({
        onInit: () => {
            const userJson = localStorage.getItem('user');
            if (userJson) {
                const user = JSON.parse(userJson) as User;
                patchState(store, setAuthUser(user));
            }

            effect(() => {
                const state = getState(store);
                const userJson = JSON.stringify(state.user);
                localStorage.setItem('user', userJson);
            })
        }
    })),
    withDevtools('auth-store')
);