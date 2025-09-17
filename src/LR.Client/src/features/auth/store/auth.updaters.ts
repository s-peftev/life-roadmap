import { PartialStateUpdater } from "@ngrx/signals";
import { AuthSlice } from "./auth.slice";
import { AccessToken } from "../../../models/auth/access-token.model";
import { Role } from "../../../core/enums/role.enum";

export function setPasswordResetRequested(isPasswordResetRequested: boolean): PartialStateUpdater<AuthSlice> {
    return _ => ({ isPasswordResetRequested });
}

export function setAccessToken(accessToken: AccessToken): PartialStateUpdater<AuthSlice> {
    return _ => ({
        accessToken: accessToken.tokenValue,
        expiresAt: new Date(accessToken.expiresAtUtc),
    });
}

export function setRoles(roles: Role[]): PartialStateUpdater<AuthSlice> {
    return _ => ({
        roles
    })
}