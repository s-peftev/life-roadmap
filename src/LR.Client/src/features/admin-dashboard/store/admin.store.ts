import { patchState, signalStore, withMethods, withProps, withState } from "@ngrx/signals";
import { initialAdminSlice } from "./admin.slice";
import { withBusy } from "../../../store-extentions/features/with-busy/with-busy.feature";
import { withLocalError } from "../../../store-extentions/features/with-local-error/with-local-error.feature";
import { inject } from "@angular/core";
import { AdminService } from "../services/admin.service";
import { withDevtools } from "@angular-architects/ngrx-toolkit";
import { rxMethod } from "@ngrx/signals/rxjs-interop";
import { exhaustMap, tap } from "rxjs";
import { setBusy, setIdle } from "../../../store-extentions/features/with-busy/with-busy.updaters";
import { tapResponse } from "@ngrx/operators";
import { setUserList } from "./admin.updaters";

export const AdminStore = signalStore(
    { providedIn: 'root' },
    withState(initialAdminSlice),
    withBusy(),
    withLocalError(),
    withProps(() => {
        const _adminService = inject(AdminService)

        return {
            _adminService
        }
    }),
    withMethods((store) => {
        const _refreshUserList = rxMethod<void>(trigger$ => trigger$.pipe(
                tap(_ => patchState(store, setBusy())),
                exhaustMap(_ => 
                    store._adminService.getUserList().pipe(
                        tapResponse({
                            next: response => patchState(store, setUserList(response)),
                            error: () => {},
                            finalize: () => patchState(store, setIdle())
                        })
                    )
                )
            ));

        return {
            loadUserList: _refreshUserList,

            deleteUserPhoto: rxMethod<string>(input$ => input$.pipe(
                tap(_ => patchState(store, setBusy())),
                exhaustMap(userId => 
                    store._adminService.deleteUserPhoto(userId).pipe(
                        tapResponse({
                            next: () => _refreshUserList(),
                            error: () => {},
                            finalize: () => patchState(store, setIdle())
                        })
                    )
                )
            )),
        }
    }),
    withDevtools('admin-store')
);