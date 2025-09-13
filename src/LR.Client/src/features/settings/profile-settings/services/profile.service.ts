import { inject, Injectable } from '@angular/core';
import { ApiClientService } from '../../../../core/services/app/api-client.service';
import { Observable } from 'rxjs';
import { MyProfileResponse } from '../../../../models/user-profile/my-profile-response.model';
import { UserProfileApi } from '../../../../core/constants/api/user-profile-api.constants';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  private _apiClient = inject(ApiClientService);
  
  public getMyProfile(): Observable<MyProfileResponse> {
    return this._apiClient.get<MyProfileResponse>(UserProfileApi.ME);
  }

  public uploadProfilePhoto(photo: File): Observable<string> {
    const formData = new FormData();
    formData.append('file', photo);

    return this._apiClient.post<FormData, string>(UserProfileApi.PROFILE_PHOTO.UPLOAD, formData);
  }
}
