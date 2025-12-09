import { PartialStateUpdater } from "@ngrx/signals";
import { UserForAdmin } from "../../../models/admin/user-for-admin.model";
import { AdminSlice } from "./admin.slice";
import { PaginatedResult } from "../../../models/paginated-result.model";

export function setUserList(userList: PaginatedResult<UserForAdmin>): PartialStateUpdater<AdminSlice> {
    return _ => ({
        userList
    })
}