import { User } from "../../../models/auth/user.model";

export interface AuthSlice {
    user: User | null;
    accessToken: string | null;
    expiresAt: Date | null;
}

export const initialAuthSlice: AuthSlice = {
    user: null,
    accessToken: null,
    expiresAt: null
}