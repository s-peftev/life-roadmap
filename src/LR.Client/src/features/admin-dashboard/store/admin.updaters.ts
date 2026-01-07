import { PartialStateUpdater } from "@ngrx/signals";
import { UserForAdmin } from "../../../models/admin/user-for-admin.model";
import { AdminSlice } from "./admin.slice";
import { PaginatedResult } from "../../../models/paginated-result.model";
import { TextSearchable } from "../../../core/interfaces/text-searchable.interface";
import { UserSearchField } from "../../../core/enums/search-fields/user-search-field.enum";

export function setUserList(userList: PaginatedResult<UserForAdmin>): PartialStateUpdater<AdminSlice> {
    return _ => ({
        userList
    })
}

export function setCurrentPage(pageNumber: number): PartialStateUpdater<AdminSlice> {
    return state => ({
        userList: {
            ...state.userList,
            metadata: {
                ...state.userList.metadata,
                currentPage: pageNumber
            }
        }
    })
}

export function setSearch(textSearch: TextSearchable<UserSearchField>): PartialStateUpdater<AdminSlice> {
    return state => ({
        ...state,
        textSearch
    })
}