import { PartialStateUpdater } from "@ngrx/signals";
import { UserForAdmin } from "../../../models/admin/user-for-admin.model";
import { AdminSlice } from "./admin.slice";

export function setUserList(userList: UserForAdmin[]): PartialStateUpdater<AdminSlice> {
    return _ => ({
        userList
    })
}