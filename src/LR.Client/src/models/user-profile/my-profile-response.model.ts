export interface MyProfileResponse {
    userName: string;
    firstName: string | null;
    lastName: string | null;
    email: string | null;
    isEmailConfirmed: boolean;
    profilePhotoUrl: string | null;
    birthDate: string | null;
}