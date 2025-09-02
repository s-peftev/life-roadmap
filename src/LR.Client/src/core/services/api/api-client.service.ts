import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { ApiResponse } from '../../../models/api/api-response.model';
import { ApiError, DefaultErrors } from '../../../models/api/api-error.model';
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiClientService {
  private http = inject(HttpClient);

  private unwrapResponse<T>(response: ApiResponse<T>): T {
    if (!response.success) throw (response.error ?? DefaultErrors.UnknownError) as ApiError;
    if (!response.data) throw DefaultErrors.NoDataError;

    return response.data;
  }

  public get<T>(url: string, options?: { [key: string]: any }): Observable<T> {

    return this.http.get<ApiResponse<T>>(url, { ...options, observe: 'body' })
      .pipe(
        map(this.unwrapResponse)
      );
  }

  public post<T, R = T>(url: string, body?: T, options?: { [key: string]: any }): Observable<R> {

    return this.http.post<ApiResponse<R>>(url, body ?? null, { ...options, observe: 'body' })
      .pipe(
        map(this.unwrapResponse)
      );
  }

  public put<T, R = T>(url: string, body?: T, options?: { [key: string]: any }): Observable<R> {
    
    return this.http.put<ApiResponse<R>>(url, body ?? null, { ...options, observe: 'body' })
      .pipe(
        map(this.unwrapResponse)
      );
  }

  public delete<T>(url: string, options?: { [key: string]: any }): Observable<T> {
    
    return this.http.delete<ApiResponse<T>>(url, { ...options, observe: 'body' })
      .pipe(
        map(this.unwrapResponse)
      );
  }
}