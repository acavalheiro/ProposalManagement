using ProposalManagement.Infrastructure.Enums;

namespace ProposalManagement.Infrastructure.Shared;

public record Error
{
    private Error(
        string code,
        string description,
        ErrorType errorType
    )
    {
        Code = code;
        Description = description;
        ErrorType = errorType;
    }

    public string Code { get; }

    public string Description { get; }

    public ErrorType ErrorType { get; }

    public static Error Failure(string code, string description) =>
        new(code, description, ErrorType.Failure);

    public static Error NotFound(string code, string description) =>
        new(code, description, ErrorType.NotFound);

    public static Error Validation(string code, string description) =>
        new(code, description, ErrorType.Validation);
    
    public static Error Exception(string code, string description) =>
        new(code, description, ErrorType.Exception);
    
}

public static class Errors
{
    public static Error NotFound(string id) =>
        Error.NotFound("Error.NotFound", $"Configuration with Id: {id} not found");


    public static Error CreateFailure => 
        Error.Failure("Error.Failure", $"Something went wrong in creating configuration");
    
    public static Error CreateException (string exceptionMessage) => 
        Error.Exception("Error.Exception", $"Something went wrong ${exceptionMessage}");


}