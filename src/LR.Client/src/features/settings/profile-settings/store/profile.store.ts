import { patchState, signalStore, withComputed, withMethods, withProps, withState } from "@ngrx/signals";
import { initialProfileSlice } from "./profile.slice";
import { withDevtools } from "@angular-architects/ngrx-toolkit";
import { computed, inject } from "@angular/core";
import { ProfileService } from "../services/profile.service";
import { withBusy } from "../../../../store-extentions/features/with-busy/with-busy.feature";
import { rxMethod } from "@ngrx/signals/rxjs-interop";
import { exhaustMap, tap } from "rxjs";
import { setBusy, setIdle } from "../../../../store-extentions/features/with-busy/with-busy.updaters";
import { tapResponse } from "@ngrx/operators";
import { setMyProfile, setProfilePhoto, setUsername } from "./profile.updaters";
import { ApiError, DefaultErrors, isApiError } from "../../../../models/api/api-error.model";
import { withLocalError } from "../../../../store-extentions/features/with-local-error/with-local-error.feature";
import { setError } from "../../../../store-extentions/features/with-local-error/with-local-error.updaters";
import { ChangeUserNameRequest } from "../../../../models/user-profile/change-username-request";

export const ProfileStore = signalStore(
    { providedIn: 'root' },
    withState(initialProfileSlice),
    withBusy(),
    withLocalError(),
    withProps(() => {
        const _profileService = inject(ProfileService);

        return {
            _profileService
        }
    }),
    withComputed((store) => {
        const hasProfilePhoto = computed(() => !!store.profilePhotoUrl());

        return {
            hasProfilePhoto
        }
    }),
    withMethods((store) => {
        return {
            getMyProfile: rxMethod<void>(trigger$ => trigger$.pipe(
                tap(_ => patchState(store, setBusy())),
                exhaustMap(_ =>
                    store._profileService.getMyProfile().pipe(
                        tapResponse({
                            next: response => patchState(store, setMyProfile(response)),
                            error: () => {},
                            finalize: () => patchState(store, setIdle())
                        })
                    )
                ),
            )),

            //rewrite on hot observable for toggling edit mode in component
            changeUserName: rxMethod<ChangeUserNameRequest>(input$ => input$.pipe(
                tap(_ => patchState(store, setBusy())),
                exhaustMap(request =>
                    store._profileService.changeUsername(request).pipe(
                        tapResponse({
                            next: _ => patchState(store, setUsername(request.newUsername)),
                            error:  (err: any) => {
                                if (isApiError(err.error.error)) {
                                    const apiErr = err.error.error as ApiError;
                                    patchState(store, setError(apiErr));
                                } else {
                                    patchState(store, setError(DefaultErrors.UnexpectedError))
                                }
                            },
                            finalize: () => patchState(store, setIdle())
                        })
                    )
                )
            )),

            uploadProfilePhoto: rxMethod<File>(input$ => input$.pipe(
                tap(_ => patchState(store, setBusy())),
                exhaustMap(file =>
                    store._profileService.uploadProfilePhoto(file).pipe(
                        tapResponse({
                            next: photoUrl => patchState(store, setProfilePhoto(photoUrl)),
                            error:  (err: any) => {
                                if (isApiError(err.error.error)) {
                                    const apiErr = err.error.error as ApiError;
                                    patchState(store, setError(apiErr));
                                } else {
                                    patchState(store, setError(DefaultErrors.UnexpectedError))
                                }
                            },
                            finalize: () => patchState(store, setIdle())
                        })
                    )
                ),
            )),

            deleteProfilePhoto: rxMethod<void>(trigger$ => trigger$.pipe(
                tap(_ => patchState(store, setBusy())),
                exhaustMap(_ =>
                    store._profileService.deleteProfilePhoto().pipe(
                        tapResponse({
                            next: _ => patchState(store, setProfilePhoto(null)),
                            error: () => {},
                            finalize: () => patchState(store, setIdle())
                        })
                    )
                )
            )),
            
            resetState: (): void => patchState(store, initialProfileSlice),
        }
    }),
    withDevtools('profile-store')
);