import { PartialStateUpdater } from "@ngrx/signals";
import { AuthSlice } from "./auth.slice";
import { AuthResponse } from "../../../models/auth/auth-response.model";

export function setBusy(isBusy: boolean): PartialStateUpdater<AuthSlice> {
    return _ => ({ isBusy });
}

export function setAuthenticatedUser(authResponse: AuthResponse): PartialStateUpdater<AuthSlice> {
    return _ => ({
        accessToken: authResponse.accessToken.tokenValue,
        expiresAt: new Date(authResponse.accessToken.expiresAtUtc),
        user: authResponse.user
    });
}