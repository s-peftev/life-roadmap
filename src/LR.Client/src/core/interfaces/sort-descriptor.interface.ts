import { SortOrder } from "../types/utils/sort-order.type";

export interface SortDescriptor<T extends string> {
    field: T;
    order: SortOrder;
}