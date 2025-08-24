namespace LR.Domain.Enums
{
    public enum ErrorType : int
    {
        None = 0,
        NotFound = 1,
        Validation = 2,
        Unauthorized = 3,
        Conflict = 4,
        InternalServerError = 5
    }
}
