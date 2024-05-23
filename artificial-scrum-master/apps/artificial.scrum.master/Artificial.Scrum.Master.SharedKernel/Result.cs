using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Artificial.Scrum.Master.SharedKernel;

public class Error
{
    public Exception Exception { get; private set; }

    public Error(Exception exception)
    {
        Exception = exception;
    }
}

public class Result
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

public sealed class Result<T> : Result
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

    public static implicit operator Result<T>(T value) => OnSuccess(value);
    public static implicit operator Result<T>(Exception exception) => OnError(exception);
}
