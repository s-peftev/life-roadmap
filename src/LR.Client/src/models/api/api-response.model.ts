import { ApiError } from "./api-error.model";

export interface ApiResponse<T> {
  success: boolean;
  data?: T | null;
  error?: ApiError | null;
}