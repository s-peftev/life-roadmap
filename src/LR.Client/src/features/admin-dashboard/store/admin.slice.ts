import { UserForAdmin } from "../../../models/admin/user-for-admin.model";
import { PaginatedResult } from "../../../models/paginated-result.model";

export interface AdminSlice {
    readonly userList: PaginatedResult<UserForAdmin>
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
    }
}