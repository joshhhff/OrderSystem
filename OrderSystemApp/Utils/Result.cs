namespace CO550WebApp.Utils;


/// <summary>
/// Represents the result of an operation, which can either succeed or fail.
/// The Value contains the data you want to return, whereas the IsSuccess property
/// indicates whether an error has occurred.
/// 
/// For instance, if you wanted to check a user existed, you may do something like this:
/// 
///     if (userExists) return Result<bool>.Ok(true); // pass in true for the user existing
///     if (!userExists) return Result<bool>.Ok(false); // pass in false for not found
/// 
/// Both results are "Ok" but the values are different. Whereas if an error occurred:
/// 
///     return Result<bool>.Err("Error message"); // the Value will be null and IsSuccess will be false
/// 
/// </summary>
/// <typeparam name="T">The type of the value being returned</typeparam>
public sealed class Result<T>
    where T : notnull
{

    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    public readonly bool IsSuccess;

    /// <summary>
    /// Holds the value returned from the operation, if successful.
    /// </summary>
    public readonly T? Value;

    /// <summary>
    /// Holds an error message if the operation failed.
    /// </summary>
    public readonly string? Error;


    /// constructor must be private
    private Result(T? value, bool isSuccess, string? error)
    {
        Value = value;
        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>
    /// Creates a successful Result containing the given value.
    /// </summary>
    /// <param name="value">The value of the operation result.</param>
    /// <returns>A successful Result.</returns>
    public static Result<T> Ok(T value)
    {
        return new(value, true, null);
    }

    /// <summary>
    /// Creates a failed Result containing the provided error message.
    /// </summary>
    /// <param name="error">The error message for the failed operation.</param>
    /// <returns>A failed Result.</returns>
    public static Result<T> Err(string error)
    {
        return new(default, false, error);
    }
}