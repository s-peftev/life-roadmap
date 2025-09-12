import { PartialStateUpdater } from "@ngrx/signals";
import { AuthSlice } from "./auth.slice";
import { ApiError } from "../../../models/api/api-error.model";
import { AccessToken } from "../../../models/auth/access-token.model";

export function setBusy(isBusy: boolean): PartialStateUpdater<AuthSlice> {
    return _ => ({ isBusy });
}

export function setPasswordResetRequested(isPasswordResetRequested: boolean): PartialStateUpdater<AuthSlice> {
    return _ => ({ isPasswordResetRequested });
}

export function setAccessToken(accessToken: AccessToken): PartialStateUpdater<AuthSlice> {
    return _ => ({
        accessToken: accessToken.tokenValue,
        expiresAt: new Date(accessToken.expiresAtUtc),
    });
}

export function setError(error: ApiError): PartialStateUpdater<AuthSlice> {
    return _ => ({ error });
}

export function clearError(): PartialStateUpdater<AuthSlice> {
    return _ => ({ error: null });
}