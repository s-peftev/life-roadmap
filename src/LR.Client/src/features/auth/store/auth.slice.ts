import { ApiError } from "../../../models/api/api-error.model";
import { User } from "../../../models/auth/user.model";

export interface AuthSlice {
    user: User | null;
    accessToken: string | null;
    expiresAt: Date | null;
    isBusy: boolean;
    error: ApiError | null;
    isPasswordResetRequested: boolean;
    // temporary fields:
    testUsersList: User[] | null;
    tempResetPasswordLink: string | null
}

export const initialAuthSlice: AuthSlice = {
    user: null,
    accessToken: null,
    expiresAt: null,
    isBusy: false,
    error: null,
    isPasswordResetRequested: false,
    testUsersList: null,
    tempResetPasswordLink: null
}