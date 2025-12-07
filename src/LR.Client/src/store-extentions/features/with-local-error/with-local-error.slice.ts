import { ApiError } from "../../../models/api/api-error.model";

export interface LocalErrorSlice {
    readonly error: ApiError | null;
}

export const initialLocalErrorSlice: LocalErrorSlice = {
    error: null
}