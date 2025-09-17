import { patchState, signalStore, withComputed, withHooks, withMethods, withProps, withState } from "@ngrx/signals";
import { initialAuthSlice } from "./auth.slice";
import { computed, effect, inject } from "@angular/core";
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { toSignal } from '@angular/core/rxjs-interop';
import { withDevtools } from "@angular-architects/ngrx-toolkit";
import { AuthService } from "../services/auth-service.service";
import { LoginRequest } from "../../../models/auth/login-request.model";
import { setAccessToken, setPasswordResetRequested } from "./auth.updaters";
import { catchError, exhaustMap, filter, finalize, map, Observable, of, tap, throwError } from "rxjs";
import { tapResponse } from "@ngrx/operators";
import { NavigationEnd, Router } from "@angular/router";
import { ROUTES } from "../../../core/constants/routes.constants";
import { ApiError, DefaultErrors, isApiError } from "../../../models/api/api-error.model";
import { RegisterRequest } from "../../../models/auth/register-request.model";
import { ForgotPasswordRequest } from "../../../models/auth/forgot-password-request.model";
import { ResetPasswordRequest } from "../../../models/auth/reset-password-request.model";
import { AccessToken } from "../../../models/auth/access-token.model";
import { withBusy } from "../../../store-extentions/features/with-busy/with-busy.feature";
import { setBusy, setIdle } from "../../../store-extentions/features/with-busy/with-busy.updaters";
import { ProfileStore } from "../../settings/profile-settings/store/profile.store";
import { withLocalError } from "../../../store-extentions/features/with-local-error/with-local-error.feature";
import { clearError, setError } from "../../../store-extentions/features/with-local-error/with-local-error.updaters";
import { ChangePasswordRequest } from "../../../models/auth/change-password-request.model";

export const AuthStore = signalStore(
    { providedIn: 'root' },
    withState(initialAuthSlice),
    withBusy(),
    withLocalError(),
    withProps(() => {
        const _authService = inject(AuthService);
        const _profileStore = inject(ProfileStore);

        return {
            _authService,
            _profileStore
        }
    }),
    withComputed((store) => {
        const hasValidAccessToken = computed(() =>
            !!store.accessToken() && !!store.expiresAt() && store.expiresAt()! > new Date());

        return {
            hasValidAccessToken
        };
    }),
    withMethods((store) => {
        const router = inject(Router);

        return {
            login: rxMethod<LoginRequest>(input$ => input$.pipe(
                tap(_ => patchState(store, setBusy())),
                exhaustMap(request =>
                    store._authService.login(request).pipe(
                        tapResponse({
                            next: response => {
                                store._profileStore.getMyProfile();
                                patchState(store, setAccessToken(response), clearError());
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
                                patchState(store, setIdle());
                            }
                        })
                    )
                )
            )),

            register: rxMethod<RegisterRequest>(input$ => input$.pipe(
                tap(_ => patchState(store, setBusy())),
                exhaustMap(request =>
                    store._authService.register(request).pipe(
                        tapResponse({
                            next: response => {
                                store._profileStore.getMyProfile();
                                patchState(store, setAccessToken(response), clearError());
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
                                patchState(store, setIdle());
                            }
                        })
                    )
                )
            )),

            logout: rxMethod<void>(trigger$ => trigger$.pipe(
                tap(_ => patchState(store, setBusy())),
                exhaustMap(_ =>
                    store._authService.logout().pipe(
                        finalize(() => {
                            patchState(store, initialAuthSlice, setIdle());
                            store._profileStore.resetState();
                            router.navigate([ROUTES.AUTH.LOGIN]);
                        })
                    )
                )
            )),

            refresh: (): Observable<AccessToken> => {
                return store._authService.refresh().pipe(
                    tap((response) => patchState(store, setAccessToken(response))),
                    catchError((err) => {
                        patchState(store, initialAuthSlice);
                        return throwError(() => err);
                    })
                );
            },

            resetPasswordRequest: rxMethod<ForgotPasswordRequest>(input$ => input$.pipe(
                tap(_ => patchState(store, setBusy())),
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
                                patchState(store, setIdle());
                            }
                        }))
                    )
                )
            )),

            resetPassword: rxMethod<ResetPasswordRequest>(input$ => input$.pipe(
                tap(_ => patchState(store, setBusy())),
                exhaustMap(request =>
                    store._authService.resetPassword(request).pipe(
                        tapResponse({
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
                                patchState(store, setIdle());
                            }
                        })
                    )
                )
            )),

            changePassword: (request: ChangePasswordRequest) => {
                return of(request).pipe(
                    tap(_ => patchState(store, setBusy())),
                    exhaustMap(req => 
                        store._authService.changePassword(req).pipe(
                            tapResponse({
                                next: () => patchState(store, clearError()),
                                error: (err: any) => {
                                    if (isApiError(err.error.error)) {
                                        const apiErr = err.error.error as ApiError;
                                        patchState(store, setError(apiErr));
                                    } else {
                                        patchState(store, setError(DefaultErrors.UnexpectedError));
                                    }
                                },
                                finalize: () => patchState(store, setIdle())
                            })
                        )
                    )
                )
            },

            setPasswordResetRequested: (isPasswordResetRequested: boolean) =>
                patchState(store, setPasswordResetRequested(isPasswordResetRequested)),

            // temporary test method
            testUsers: rxMethod<void>(trigger$ => trigger$.pipe(
                tap(_ => patchState(store, setBusy())),
                exhaustMap(_ => {
                    return store._authService.testUserList().pipe(
                        tapResponse(({
                            next: resp => patchState(store, { testUsersList: resp }),
                            error: (err: any) => {
                                patchState(store, { error: err.status })
                            },
                            finalize: () => {
                                patchState(store, setIdle())
                            }
                        }))
                    )
                })
            )),
        };
    }),
    withHooks(store => {
        const router = inject(Router);

        const urlSig = toSignal(router.events.pipe(
            filter(e => e instanceof NavigationEnd),
            map(() => router.url)
        ), { initialValue: router.url });

        return {
            onInit: () => {
                //clear error after route changing
                let first = true;
                effect(() => {
                    urlSig();

                    if (first) {
                        first = false;
                        return;
                    }
                    patchState(store, clearError());
                });
            }
        }
    }),
    withDevtools('auth-store')
);
