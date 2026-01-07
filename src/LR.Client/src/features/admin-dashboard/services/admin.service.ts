import { inject, Injectable } from '@angular/core';
import { ApiClientService } from '../../../core/services/app/api-client.service';
import { Observable } from 'rxjs';
import { UserForAdmin } from '../../../models/admin/user-for-admin.model';
import { AdminApi } from '../../../core/constants/api/admin-api.constants';
import { PaginatedResult } from '../../../models/paginated-result.model';
import { environment } from '../../../environments/environment';
import { UsersForAdminRequest } from '../../../models/admin/users-for-admin-request.model';
import { HttpParams } from '@angular/common/http';
import { UserSearchField } from '../../../core/enums/search-fields/user-search-field.enum';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private _apiClient = inject(ApiClientService);

  public getUserList(request: UsersForAdminRequest): Observable<PaginatedResult<UserForAdmin>> {
    const pageNumber = request.pageNumber ?? environment.paginationDefaults.pageNumber;
    const pageSize = request.pageSize ?? environment.paginationDefaults.pageSize;

    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    if (request.search?.trim()) {
      params = params.set('search', request.search.trim());

      if (request.searchIn && request.searchIn.length > 0) {
        for (const field of request.searchIn) {
          params = params.append('searchIn', UserSearchField[field]);
        }
      }
    }
    
    return this._apiClient.get<PaginatedResult<UserForAdmin>>(AdminApi.USERS, { params });
  }

  public deleteUserPhoto(userId: string): Observable<void> {
    return this._apiClient.deleteVoid(AdminApi.USER_PROFILE_PHOTO(userId));
  }
}