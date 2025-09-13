import { environment } from "../../../environments/environment";

const baseApiUrl = environment.apiUrl + 'userProfile/';

export const UserProfileApi = {
    ME: baseApiUrl + 'me',
    PROFILE_PHOTO: {
        UPLOAD: baseApiUrl + 'photo/upload'
    },
}