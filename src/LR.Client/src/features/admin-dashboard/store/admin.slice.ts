import { UserSearchField } from "../../../core/enums/search-fields/user-search-field.enum";
import { UserSortField } from "../../../core/enums/sort/user-sort-field.enum";
import { SortDescriptor } from "../../../core/interfaces/sort-descriptor.interface";
import { TextSearchable } from "../../../core/interfaces/text-searchable.interface";
import { environment } from "../../../environments/environment";
import { UserForAdmin } from "../../../models/admin/user-for-admin.model";

export interface AdminSlice {
    readonly userList: UserForAdmin[],
    readonly currentPage: number,
    readonly pageSize: number,
    readonly totalCount: number,
    readonly totalPages: number,
    readonly textSearch: TextSearchable<UserSearchField>,
    readonly sortCriteria: SortDescriptor<UserSortField>[]
}

export const initialAdminSlice: AdminSlice = {
    userList: [],
    currentPage: 1,
    pageSize: environment.paginationDefaults.pageSize,
    totalCount: 0,
    totalPages: 0,
    textSearch: {
        searchText: '',
        fields: Object.values(UserSearchField)
            .filter(v => typeof v === 'number') as UserSearchField[]
    },
    sortCriteria: []
}