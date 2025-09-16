import { ApiError } from "../../../../models/api/api-error.model";

export interface ProfileSlice {
    readonly userName: string | null;
    readonly firstName: string | null;
    readonly lastName: string | null;
    readonly email: string | null;
    readonly isEmailConfirmed: boolean;
    readonly profilePhotoUrl: string | null;
    readonly birthDate: string | null;
}

export const initialProfileSlice: ProfileSlice = {
    userName: null,
    firstName: null,
    lastName: null,
    email: null,
    isEmailConfirmed: false,
    profilePhotoUrl: null,
    birthDate: null,
}