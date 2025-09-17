import { UserForAdmin } from "../../../models/admin/user-for-admin.model";

export interface AdminSlice {
    readonly userList: UserForAdmin[] | null;
}

export const initialAdminSlice: AdminSlice = {
    userList: null,
}