import { Role } from "../../core/enums/role.enum";

export interface UserForAdmin {
  id: string;
  userName: string;
  email: string | null;
  isEmailConfirmed: boolean;
  firstName: string | null;
  lastName: string | null;
  profilePhotoUrl: string | null;
  birthDate: string | null;
  createdAt: string;
  lastActive: string;
  roles: Role[];
}