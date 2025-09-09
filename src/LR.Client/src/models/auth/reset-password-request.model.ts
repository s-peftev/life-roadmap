export interface ResetPasswordRequest {
    userId: string;
    token: string;
    password: string;
}