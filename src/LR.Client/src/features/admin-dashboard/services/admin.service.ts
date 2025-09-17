import { inject, Injectable } from '@angular/core';
import { ApiClientService } from '../../../core/services/app/api-client.service';
import { Observable } from 'rxjs';
import { UserForAdmin } from '../../../models/admin/user-for-admin.model';
import { AdminApi } from '../../../core/constants/api/admin-api.constants';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private _apiClient = inject(ApiClientService);

  public getUserList(): Observable<UserForAdmin[]> {
    return this._apiClient.get<UserForAdmin[]>(AdminApi.USERS);
  }

  public deleteUserPhoto(userId: string): Observable<void> {
    return this._apiClient.deleteVoid(AdminApi.USER_PROFILE_PHOTO(userId));
  }
}
