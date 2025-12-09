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
        const _refreshUserList = rxMethod<number>(input$ => input$.pipe(
                tap(_ => patchState(store, setBusy())),
                exhaustMap(pageNumber => 
                    store._adminService.getUserList(pageNumber).pipe(
                        tapResponse({
                            next: response => patchState(store, setUserList(response)),
                            error: () => {},
                            finalize: () => patchState(store, setIdle())
                        })
                    )
                )
            ));

        return {
            loadUserList: (pageNumber: number) => _refreshUserList(pageNumber),

            deleteUserPhoto: rxMethod<string>(input$ => input$.pipe(
                tap(_ => patchState(store, setBusy())),
                exhaustMap(userId => 
                    store._adminService.deleteUserPhoto(userId).pipe(
                        tapResponse({
                            next: () => _refreshUserList(store.userList().metadata.currentPage),
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