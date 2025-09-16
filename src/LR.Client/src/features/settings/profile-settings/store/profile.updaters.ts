import { PartialStateUpdater } from "@ngrx/signals";
import { MyProfileResponse } from "../../../../models/user-profile/my-profile-response.model";
import { ProfileSlice } from "./profile.slice";
import { ChangePersonalInfoRequest } from "../../../../models/user-profile/change-personal-info-request";

export function setMyProfile(myProfile: MyProfileResponse): PartialStateUpdater<ProfileSlice> {

    
    return _ => ({
        userName: myProfile.userName,
        firstName: myProfile.firstName ?? null,
        lastName: myProfile.lastName ?? null,
        email: myProfile.email ?? null,
        isEmailConfirmed: myProfile.isEmailConfirmed,
        profilePhotoUrl: myProfile.profilePhotoUrl ?? null,
        birthDate: myProfile.birthDate,
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

export function setPersonalInfo(personalInfo: ChangePersonalInfoRequest): PartialStateUpdater<ProfileSlice> {
        return _ => ({
        firstName: personalInfo.firstName,
        lastName: personalInfo.lastName,
        birthDate: personalInfo.birthDate,
    }) 
}