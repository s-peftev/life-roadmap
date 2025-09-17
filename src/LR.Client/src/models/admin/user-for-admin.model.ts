import { Role } from "../../core/enums/role.enum";

export interface UserForAdmin {
  userName: string;
  email: string | null;
  isEmailConfirmed: boolean;
  firstName: string | null;
  lastName: string | null;
  profilePhotoUrl: string | null;
  birthDate: string | null; // ISO string yyyy-mm-dd
  createdAt: string; // ISO string
  lastActive: string; // ISO string
  roles: Role[];
}