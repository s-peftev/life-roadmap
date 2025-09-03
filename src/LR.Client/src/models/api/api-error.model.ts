import { ErrorType } from "../../core/enums/error-type.enum";

export interface ApiError {
  id: string;
  type: ErrorType;
  description: string;
  details?: string[];
}

export const DefaultErrors: Record<'UnknownError' | 'NoDataError' | 'UnexpectedError', ApiError> = {
    UnknownError: {
        id: 'unknown',
        type: ErrorType.None,
        description: 'Unknown error'
    },
    NoDataError: {
        id: 'no-data',
        type: ErrorType.InternalServerError,
        description: 'No data returned'
    }, 
    UnexpectedError: {
        id: 'unexpected',
        type: ErrorType.InternalServerError,
        description: 'Unexpected HTTP error'
    }
}

export function isApiError(err: unknown): err is ApiError {
    return typeof err === 'object' && err !== null &&
           'id' in err && 'type' in err && 'description' in err;
}