import { PartialStateUpdater } from "@ngrx/signals";
import { AuthSlice } from "./auth.slice";
import { AuthResponse } from "../../../models/auth/auth-response.model";
import { ApiError } from "../../../models/api/api-error.model";
import { User } from "../../../models/auth/user.model";

export function setBusy(isBusy: boolean): PartialStateUpdater<AuthSlice> {
    return _ => ({ isBusy });
}

export function setAuthUserWithJwt(authResponse: AuthResponse): PartialStateUpdater<AuthSlice> {
    return _ => ({
        accessToken: authResponse.accessToken.tokenValue,
        expiresAt: new Date(authResponse.accessToken.expiresAtUtc),
        user: authResponse.user
    });
}

export function setAuthUser(user: User): PartialStateUpdater<AuthSlice> {
    return _ => ({
        user: user
    });
}

export function setError(error: ApiError): PartialStateUpdater<AuthSlice> {
    return _ => ({ error });
}