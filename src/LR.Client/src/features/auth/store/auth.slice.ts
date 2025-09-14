import { User } from "../../../models/auth/user.model";

export interface AuthSlice {
    readonly accessToken: string | null;
    readonly expiresAt: Date | null;
    readonly isPasswordResetRequested: boolean;
    
    // temporary fields:
    readonly testUsersList: User[] | null;
    readonly tempResetPasswordLink: string | null
}

export const initialAuthSlice: AuthSlice = {
    accessToken: null,
    expiresAt: null,
    isPasswordResetRequested: false,

    // temporary fields:
    testUsersList: null,
    tempResetPasswordLink: null
}