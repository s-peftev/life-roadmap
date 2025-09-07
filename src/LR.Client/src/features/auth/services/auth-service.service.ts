import { inject, Injectable } from '@angular/core';
import { LoginRequest } from '../../../models/auth/login-request.model';
import { AuthResponse } from '../../../models/auth/auth-response.model';
import { Observable } from 'rxjs';
import { AuthApi } from '../../../core/constants/api/auth-api.constants';
import { RegisterRequest } from '../../../models/auth/register-request.model';
import { ApiClientService } from '../../../core/services/app/api-client.service';
import { User } from '../../../models/auth/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _apiClient = inject(ApiClientService);

  public login(loginRequest: LoginRequest): Observable<AuthResponse> {
    return this._apiClient.post<LoginRequest, AuthResponse>(AuthApi.LOGIN, loginRequest, { withCredentials: true });
  }

  public register(registerRequest: RegisterRequest): Observable<AuthResponse> {
    return this._apiClient.post<RegisterRequest, AuthResponse>(AuthApi.REGISTER, registerRequest, { withCredentials: true });
  }

  public logout(): Observable<void> {
    return this._apiClient.postVoid(AuthApi.LOGOUT, null, { withCredentials: true });
  }

  public refresh(): Observable<AuthResponse> {
    return this._apiClient.postNoIntercept<AuthResponse>(AuthApi.REFRESH, undefined, { withCredentials: true });
  }

  public testUserList(): Observable<User[]> {
    return this._apiClient.get<User[]>('https://localhost:5001/api/userTest');
  }
}
