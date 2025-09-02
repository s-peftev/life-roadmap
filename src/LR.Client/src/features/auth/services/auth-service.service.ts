import { inject, Injectable } from '@angular/core';
import { LoginRequest } from '../../../models/auth/login-request.model';
import { AuthResponse } from '../../../models/auth/auth-response.model';
import { Observable } from 'rxjs';
import { AuthApi } from '../../../core/constants/api/auth-api.constants';
import { RegisterRequest } from '../../../models/auth/register-request.model';
import { ApiClientService } from '../../../core/services/api/api-client.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _apiClient = inject(ApiClientService);

  public login(loginRequest: LoginRequest): Observable<AuthResponse> {
    return this._apiClient.post<LoginRequest, AuthResponse>(AuthApi.LOGIN, loginRequest);
  }

  public register(registerRequest: RegisterRequest): Observable<AuthResponse> {
    return this._apiClient.post<RegisterRequest, AuthResponse>(AuthApi.REGISTER, registerRequest);
  }

  public logout(): Observable<AuthResponse> {
    return this._apiClient.post<AuthResponse>(AuthApi.LOGOUT);
  }

  public refresh(): Observable<AuthResponse> {
    return this._apiClient.post<AuthResponse>(AuthApi.REFRESH);
  }
}
