import { patchState, signalStore, withHooks, withMethods, withProps, withState } from "@ngrx/signals";
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
import { setCurrentPage, setSearch, setUserList } from "./admin.updaters";
import { UsersForAdminRequest } from "../../../models/admin/users-for-admin-request.model";
import { TextSearchable } from "../../../core/interfaces/text-searchable.interface";
import { UserSearchField } from "../../../core/enums/search-fields/user-search-field.enum";

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
        const _refreshUserList = rxMethod<UsersForAdminRequest>(input$ => input$.pipe(
                tap(_ => patchState(store, setBusy())),
                exhaustMap(request => 
                    store._adminService.getUserList(request).pipe(
                        tapResponse({
                            next: response => patchState(store, setUserList(response)),
                            error: () => {},
                            finalize: () => patchState(store, setIdle())
                        })
                    )
                )
            ));

        return {
            loadUserList: (request: UsersForAdminRequest) => _refreshUserList(request),

            deleteUserPhoto: rxMethod<string>(input$ => input$.pipe(
                tap(_ => patchState(store, setBusy())),
                exhaustMap(userId => 
                    store._adminService.deleteUserPhoto(userId).pipe(
                        tapResponse({
                            next: () => {},
                            error: () => {},
                            finalize: () => patchState(store, setIdle())
                        })
                    )
                )
            )),

            setCurrentPage: (pageNumber: number) => patchState(store, setCurrentPage(pageNumber)),

            setSearch: (textSearch: TextSearchable<UserSearchField>) => patchState(store, setSearch(textSearch))
        }
    }),
    withHooks((store) => {
        return {
            onInit: () => {
                const page = store.userList().metadata.currentPage;
                const search = store.textSearch().searchText;

                if (!search?.trim()) return;

                const request: UsersForAdminRequest = {
                    pageNumber: page,
                    pageSize: store.userList().metadata.pageSize,
                    search: search.trim(),
                    searchIn: store.textSearch().fields
                };

                store.loadUserList(request);
            }
        }
    }),
    withDevtools('admin-store')
);