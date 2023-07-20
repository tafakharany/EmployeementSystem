namespace EmploymentSystem.Application.DTOs.Common;

public static class ResponseCodes
{
    public static string ProcessedSuccessfully { get; } = "0";
    public static string ValidationError { get; } = "1";
    public static string DuplicatedApplication { get; } = "2";
    public static string NotFound { get; } = "3";
    public static string FailedToProcess { get; } = "4";
    public static string AuthenticationFailed { get; } = "5";
    public static string GeneralError { get; } = "99";
}