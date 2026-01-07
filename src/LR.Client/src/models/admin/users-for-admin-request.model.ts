import { UserSearchField } from "../../core/enums/search-fields/user-search-field.enum";
import { PaginatedRequest } from "../paginated-request.model";

export interface UsersForAdminRequest extends PaginatedRequest {
    search: string,
    searchIn: Set<UserSearchField>
}