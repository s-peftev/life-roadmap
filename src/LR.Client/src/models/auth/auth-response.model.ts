import { AccessToken } from "./access-token.model";
import { User } from "./user.model";

export interface AuthResponse {
  accessToken: AccessToken;
  user: User;
}