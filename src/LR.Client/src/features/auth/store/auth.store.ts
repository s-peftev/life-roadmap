import { getState, patchState, signalStore, withComputed, withHooks, withMethods, withProps, withState } from "@ngrx/signals";
import { initialAuthSlice } from "./auth.slice";
import { computed, effect, inject } from "@angular/core";
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { withDevtools } from "@angular-architects/ngrx-toolkit";
import { AuthService } from "../services/auth-service.service";
import { LoginRequest } from "../../../models/auth/login-request.model";
import { clearError, setAuthUser, setAuthUserWithJwt, setBusy, setError, setPasswordResetRequested } from "./auth.updaters";
import { catchError, exhaustMap, finalize, map, Observable, tap, throwError } from "rxjs";
import { tapResponse } from "@ngrx/operators";
import { Router } from "@angular/router";
import { ROUTES } from "../../../core/constants/routes.constants";
import { ApiError, DefaultErrors, isApiError } from "../../../models/api/api-error.model";
import { User } from "../../../models/auth/user.model";
import { AuthResponse } from "../../../models/auth/auth-response.model";
import { RegisterRequest } from "../../../models/auth/register-request.model";
import { ForgotPasswordRequest } from "../../../models/auth/forgot-password-request.model";
import { ResetPasswordRequest } from "../../../models/auth/reset-password-request.model";

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
                            next: response => {
                                patchState(store, setAuthUserWithJwt(response), clearError());
                                router.navigate([ROUTES.DASHBOARD]);
                            },
                            error: (err: any) => {
                                if (isApiError(err.error.error)) {
                                    const apiErr = err.error.error as ApiError;
                                    patchState(store, setError(apiErr));
                                } else {
                                    patchState(store, setError(DefaultErrors.UnexpectedError))
                                }
                            },
                            finalize: () => {
                                patchState(store, setBusy(false));
                            }
                        })
                    )
                )
            )),

            register: rxMethod<RegisterRequest>(input$ => input$.pipe(
                tap(_ => patchState(store, setBusy(true))),
                exhaustMap(request =>
                    store._authService.register(request).pipe(
                        tapResponse({
                            next: response => {
                                patchState(store, setAuthUserWithJwt(response), clearError());
                                router.navigate([ROUTES.DASHBOARD]);
                            },
                            error: (err: any) => {
                                if (isApiError(err.error.error)) {
                                    const apiErr = err.error.error as ApiError;
                                    patchState(store, setError(apiErr));
                                } else {
                                    patchState(store, setError(DefaultErrors.UnexpectedError))
                                }
                            },
                            finalize: () => {
                                patchState(store, setBusy(false));
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

            refresh: (): Observable<AuthResponse> => {
                return store._authService.refresh().pipe(
                    tap((response) => patchState(store, setAuthUserWithJwt(response))),
                    catchError((err) => {
                        patchState(store, initialAuthSlice);
                        return throwError(() => err);
                    })
                );
            },

            resetPasswordRequest: rxMethod<ForgotPasswordRequest>(input$ => input$.pipe(
                tap(_ => patchState(store, setBusy(true))),
                exhaustMap(request =>
                    store._authService.resetPasswordRequest(request).pipe(
                        tapResponse(({
                            //todo after implementing backend email service change "next" handler
                            next: response => 
                                patchState(store, { tempResetPasswordLink: response }, setPasswordResetRequested(true)),
                            error: (err: any) => {
                                if (isApiError(err.error.error)) {
                                    const apiErr = err.error.error as ApiError;
                                    patchState(store, setError(apiErr));
                                } else {
                                    patchState(store, setError(DefaultErrors.UnexpectedError))
                                }
                            },
                            finalize: () => {
                                patchState(store, setBusy(false));
                            }
                        }))
                    )
                )
            )),

            resetPassword: rxMethod<ResetPasswordRequest>(input$ => input$.pipe(
                tap(_ => patchState(store, setBusy(true))),
                exhaustMap(request =>
                    store._authService.resetPassword(request).pipe(
                        tapResponse(({
                            next: _ => router.navigate([ROUTES.AUTH.LOGIN]),
                            error: (err: any) => {
                                if (isApiError(err.error.error)) {
                                    const apiErr = err.error.error as ApiError;
                                    patchState(store, setError(apiErr));
                                } else {
                                    patchState(store, setError(DefaultErrors.UnexpectedError))
                                }
                            },
                            finalize: () => {
                                patchState(store, setBusy(false));
                            }
                        }))
                    )
                )
            )),

            setPasswordResetRequested: (isPasswordResetRequested: boolean) => 
                patchState(store, setPasswordResetRequested(isPasswordResetRequested)),
            // temporary test method
            testUsers: rxMethod<void>(trigger$ => trigger$.pipe(
                tap(_ => patchState(store, setBusy(true))),
                exhaustMap(_ => {
                    return store._authService.testUserList().pipe(
                        tapResponse(({
                            next: resp => patchState(store, { testUsersList: resp }),
                            error: (err: any) => {
                                patchState(store, { error: err.status })
                            },
                            finalize: () => {
                                patchState(store, setBusy(false))
                            }
                        }))
                    )
                })
            )),
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