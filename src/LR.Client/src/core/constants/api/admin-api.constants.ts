import { environment } from "../../../environments/environment";

const baseApiUrl = environment.apiUrl + 'admin/';

export const AdminApi = {
    USERS: baseApiUrl + 'users',
    USER_PROFILE_PHOTO: (userId: string) =>
    `${baseApiUrl}users/${userId}/photo`,
}