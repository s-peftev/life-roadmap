import { UserSearchField } from "../../../core/enums/search-fields/user-search-field.enum";
import { TextSearchable } from "../../../core/interfaces/text-searchable.interface";
import { UserForAdmin } from "../../../models/admin/user-for-admin.model";
import { PaginatedResult } from "../../../models/paginated-result.model";

export interface AdminSlice {
    readonly userList: PaginatedResult<UserForAdmin>
    readonly textSearch: TextSearchable<UserSearchField>
}

export const initialAdminSlice: AdminSlice = {
    userList: {
        metadata: {
            currentPage: 0,
            pageSize: 0,
            totalCount: 0,
            totalPages: 0
        },
        items: []
    },
    textSearch: {
        searchText: '',
        fields: new Set(
            Object.values(UserSearchField)
            .filter(v => typeof v === 'number') as UserSearchField[]
        )
    }
}