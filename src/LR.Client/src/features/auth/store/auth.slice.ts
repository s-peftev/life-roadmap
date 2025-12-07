import { Role } from "../../../core/enums/role.enum";

export interface AuthSlice {
    readonly accessToken: string | null;
    readonly expiresAt: Date | null;
    readonly isPasswordResetRequested: boolean;
    readonly roles: Role[] | null;
    
    // temporary fields:
    readonly tempResetPasswordLink: string | null
}

export const initialAuthSlice: AuthSlice = {
    accessToken: null,
    expiresAt: null,
    isPasswordResetRequested: false,
    roles: null,

    // temporary fields:
    tempResetPasswordLink: null
}