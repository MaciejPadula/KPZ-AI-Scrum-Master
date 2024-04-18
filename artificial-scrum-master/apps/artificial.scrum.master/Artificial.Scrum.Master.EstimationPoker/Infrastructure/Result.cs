namespace Artificial.Scrum.Master.EstimationPoker.Infrastructure;

internal class Error
{
    public Exception Exception { get; private set; }

    public Error(Exception exception)
    {
        Exception = exception;
    }
}

internal class Result
{
    protected Result(Error? error = null)
    {
        Error = error;
    }

    public Error? Error { get; init; }
    public bool IsSuccess => Error is null;

    public static Result OnSuccess() => new();
    public static Result OnError(Exception exception) => new(error: new(exception));
}

file class ErrorValueAccessedException : Exception { }

internal sealed class Result<T> : Result
{
    private Result(
        T? value = default,
        Error? error = null) : base(error)
    {
        _value = value;
    }

    private T? _value;
    public T? Value
    {
        get
        {
            return _value ?? throw Error?.Exception ?? throw new ErrorValueAccessedException();
        }
        init
        {
            _value = value;
        }
    }

    public static Result<T> OnSuccess(T value) => new(value: value);
    public new static Result<T> OnError(Exception exception) => new(error: new(exception));
}
