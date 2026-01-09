import { UserSearchField } from "../../core/enums/search-fields/user-search-field.enum";
import { UserSortField } from "../../core/enums/sort/user-sort-field.enum";
import { PaginatedRequest } from "../paginated-request.model";

export interface UsersForAdminRequest extends PaginatedRequest<UserSortField> {
    search: string,
    searchIn: readonly UserSearchField[]
}