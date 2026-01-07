import { PartialStateUpdater } from "@ngrx/signals";
import { UserForAdmin } from "../../../models/admin/user-for-admin.model";
import { AdminSlice } from "./admin.slice";
import { TextSearchable } from "../../../core/interfaces/text-searchable.interface";
import { UserSearchField } from "../../../core/enums/search-fields/user-search-field.enum";
import { PaginatedResult } from "../../../models/paginated-result.model";

export function setUserList(paginatedResult: PaginatedResult<UserForAdmin>): PartialStateUpdater<AdminSlice> {
    return store => ({
        ...store,
        userList: paginatedResult.items,
        totalCount: paginatedResult.metadata.totalCount,
        totalPages: paginatedResult.metadata.totalPages
    })
}

export function setCurrentPage(pageNumber: number): PartialStateUpdater<AdminSlice> {
    return state => ({
        ...state,
        currentPage: pageNumber
    })
}

export function setSearch(textSearch: TextSearchable<UserSearchField>): PartialStateUpdater<AdminSlice> {
    return state => ({
        ...state,
        textSearch
    })
}