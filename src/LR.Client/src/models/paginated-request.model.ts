import { SortDescriptor } from "../core/interfaces/sort-descriptor.interface"

export interface PaginatedRequest<T extends string> {
    pageNumber?: number,
    pageSize?: number
    sort?: SortDescriptor<T>[]
}