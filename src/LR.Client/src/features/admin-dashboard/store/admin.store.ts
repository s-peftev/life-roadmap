import { patchState, signalStore, withHooks, withMethods, withProps, withState } from "@ngrx/signals";
import { initialAdminSlice } from "./admin.slice";
import { withBusy } from "../../../store-extentions/features/with-busy/with-busy.feature";
import { withLocalError } from "../../../store-extentions/features/with-local-error/with-local-error.feature";
import { computed, effect, inject, untracked } from "@angular/core";
import { AdminService } from "../services/admin.service";
import { withDevtools } from "@angular-architects/ngrx-toolkit";
import { rxMethod } from "@ngrx/signals/rxjs-interop";
import { exhaustMap, switchMap, tap } from "rxjs";
import { setBusy, setIdle } from "../../../store-extentions/features/with-busy/with-busy.updaters";
import { tapResponse } from "@ngrx/operators";
import { setCurrentPage, setSearch, setSortCriteria, setUserList } from "./admin.updaters";
import { UsersForAdminRequest } from "../../../models/admin/users-for-admin-request.model";
import { TextSearchable } from "../../../core/interfaces/text-searchable.interface";
import { UserSearchField } from "../../../core/enums/search-fields/user-search-field.enum";
import { SortDescriptor } from "../../../core/interfaces/sort-descriptor.interface";
import { UserSortField } from "../../../core/enums/sort/user-sort-field.enum";

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
                switchMap(request => 
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
                            next: () => _refreshUserList({
                                pageNumber: store.userList().metadata.currentPage,
                                pageSize: store.userList().metadata.pageSize,
                                search: store.textSearch().searchText.trim(),
                                searchIn: store.textSearch().fields
                            }),
                            error: () => {},
                            finalize: () => patchState(store, setIdle())
                        })
                    )
                )
            )),

            setCurrentPage: (pageNumber: number) => patchState(store, setCurrentPage(pageNumber)),

            setSearch: (textSearch: TextSearchable<UserSearchField>) => patchState(store, setSearch(textSearch)),

            setSortCriteria: (sortCriteria: SortDescriptor<UserSortField>[]) => patchState(store, setSortCriteria(sortCriteria))
        }
    }),
    withHooks((store) => {
        return {
            onInit: () => {
                store.loadUserList({
                    pageNumber: store.userList().metadata.currentPage,
                    pageSize: store.userList().metadata.pageSize,
                    search: '',
                    searchIn: store.textSearch().fields
                });

                const changePageSig = computed(() => store.userList().metadata.currentPage);
                const searchTextSig = computed(() => store.textSearch().searchText);
                const searchFieldsSig = computed(() => store.textSearch().fields);

                effect(() => {
                    const page = changePageSig();

                    untracked(() => {
                        store.loadUserList({
                            pageNumber: page,
                            pageSize: store.userList().metadata.pageSize,
                            search: store.textSearch().searchText.trim(),
                            searchIn: store.textSearch().fields,
                            sort: store.sortCriteria()
                        });
                    });
                });

                effect(() => {
                    const search = searchTextSig().trim();

                    untracked(() => {
                        store.setCurrentPage(1);
                        store.loadUserList({
                            pageNumber: 1,
                            pageSize: store.userList().metadata.pageSize,
                            search,
                            searchIn: store.textSearch().fields,
                            sort: store.sortCriteria()
                        });
                    });
                });

                effect(() => {
                    const searchIn = searchFieldsSig();

                    untracked(() => {
                        const search = store.textSearch().searchText.trim();
                        if (!search) return;

                        store.setCurrentPage(1);
                        store.loadUserList({
                            pageNumber: 1,
                            pageSize: store.userList().metadata.pageSize,
                            search: search,
                            searchIn: searchIn,
                            sort: store.sortCriteria()
                        });
                    });
                });

                effect(() => {
                    const sort = store.sortCriteria();

                    untracked(() => {
                        store.setCurrentPage(1);
                        store.loadUserList({
                            pageNumber: 1,
                            pageSize: store.userList().metadata.pageSize,
                            search: store.textSearch().searchText.trim(),
                            searchIn: store.textSearch().fields,
                            sort
                        });
                    });
                });
            }
        }
    }),
    withDevtools('admin-store')
);