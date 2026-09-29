namespace DeskFlow.API.Exceptions;

public class BusinessConflictException : Exception
{
    public BusinessConflictException(string message)
        : base(message)
    {
    }
}