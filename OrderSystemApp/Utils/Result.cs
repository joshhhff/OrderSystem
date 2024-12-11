namespace CO550WebApp.Utils;

public sealed class Result<T>(T? value, bool isSuccess, string? error)
{
    public readonly bool IsSuccess = isSuccess;

    public readonly T? Value = value;
    public readonly string? Error = error;

    public static Result<T> Ok(T value)
    {
        return new(value, true, null);
    }

    public static Result<T> Err(string error)
    {
        return new(default, false, error);
    }
}