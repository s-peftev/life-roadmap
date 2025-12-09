import { inject, Injectable } from '@angular/core';
import { ApiClientService } from '../../../core/services/app/api-client.service';
import { Observable } from 'rxjs';
import { UserForAdmin } from '../../../models/admin/user-for-admin.model';
import { AdminApi } from '../../../core/constants/api/admin-api.constants';
import { PaginatedResult } from '../../../models/paginated-result.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private _apiClient = inject(ApiClientService);

  public getUserList(pageNumber: number = environment.paginationDefaults.pageNumber, pageSize: number = environment.paginationDefaults.pageSize)
    : Observable<PaginatedResult<UserForAdmin>> {
    return this._apiClient.get<PaginatedResult<UserForAdmin>>(AdminApi.USERS, { params: { pageNumber, pageSize } });
  }

  public deleteUserPhoto(userId: string): Observable<void> {
    return this._apiClient.deleteVoid(AdminApi.USER_PROFILE_PHOTO(userId));
  }
}
