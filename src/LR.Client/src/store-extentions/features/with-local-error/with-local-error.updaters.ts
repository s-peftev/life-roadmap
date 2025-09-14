import { PartialStateUpdater } from "@ngrx/signals";
import { ApiError } from "../../../models/api/api-error.model";
import { LocalErrorSlice } from "./with-local-error.slice";

export function setError(error: ApiError): PartialStateUpdater<LocalErrorSlice> {
    return _ => ({ error });
}

export function clearError(): PartialStateUpdater<LocalErrorSlice> {
    return _ => ({ error: null });
}