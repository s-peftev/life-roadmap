import { PartialStateUpdater } from "@ngrx/signals";
import { MyProfileResponse } from "../../../../models/user-profile/my-profile-response.model";
import { ProfileSlice } from "./profile.slice";

export function setMyProfile(myProfile: MyProfileResponse): PartialStateUpdater<ProfileSlice> {
    return _ => ({
        userName: myProfile.userName,
        firstName: myProfile.firstName ?? null,
        lastName: myProfile.lastName ?? null,
        email: myProfile.email ?? null,
        isEmailConfirmed: myProfile.isEmailConfirmed,
        profilePhotoUrl: myProfile.profilePhotoUrl ?? null,
        birthDate: myProfile.birthDate ? new Date(myProfile.birthDate) : null
    }) 
}

export function setProfilePhoto(profilePhotoUrl: string | null): PartialStateUpdater<ProfileSlice> {
    return _ => ({
        profilePhotoUrl
    }) 
}

export function setUsername(userName: string): PartialStateUpdater<ProfileSlice> {
        return _ => ({
        userName
    }) 
}