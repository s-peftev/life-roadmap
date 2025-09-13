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
import { setMyProfile, setProfilePhoto } from "./profile.updaters";

export const ProfileStore = signalStore(
    { providedIn: 'root' },
    withState(initialProfileSlice),
    withBusy(),
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
                            error: _ => {},
                            finalize: () => patchState(store, setIdle())
                        })
                    )
                ),
            )),
            uploadProfilePhoto: rxMethod<File>(input$ => input$.pipe(
                tap(_ => patchState(store, setBusy())),
                exhaustMap(file => 
                    store._profileService.uploadProfilePhoto(file).pipe(
                        tapResponse({
                            next: photoUrl => patchState(store, setProfilePhoto(photoUrl)),
                            error: _ => {}, //todo handle errors
                            finalize: () => patchState(store, setIdle())
                        })
                    )
                ),

            )),
            resetState: (): void => patchState(store, initialProfileSlice),
        }
    }),
    withDevtools('profile-store')
);