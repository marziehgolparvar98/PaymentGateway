using System.Diagnostics;
using System.Text.Json.Serialization;

namespace PaymentGateway.Common
{
    public class Result<T> : Result
    {
        private readonly T _value;

        protected internal Result(T value, bool succeeded, string error, int errorCode)
            : base(succeeded, error, errorCode)
        {
            _value = value;
        }

        protected internal Result(T value, bool succeeded, IEnumerable<ErrorResult> errors)
            : base(succeeded, errors)
        {
            _value = value;
        }

        public T Value
        {
            get
            {
                if (!Succeeded)
                    throw new InvalidOperationException("There is no value for failure.");

                return _value;
            }
        }

        public override object GetValue() => _value;
    }

    public class Result
    {
        private static readonly Result SuccessResult = new Result(true, null, 0);

        protected Result(bool succeeded, string message, int errorCode)
        {
            if (succeeded)
            {
                if (!string.IsNullOrWhiteSpace(message))
                    throw new ArgumentException("There should be no error message for success.", nameof(message));
            }
            else
            {
                if (message == null)
                    throw new ArgumentNullException(nameof(message), "There must be error message for failure.");
            }

            Succeeded = succeeded;
            Error = new List<ErrorResult>();
            Error.Add(new ErrorResult(message, errorCode));
        }

        protected Result(bool succeeded, IEnumerable<ErrorResult> errors)
        {
            //if (succeeded)
            //{
            //    if (!string.IsNullOrWhiteSpace(message))
            //        throw new ArgumentException("There should be no error message for success.", nameof(message));
            //}
            //else
            //{
            //    if (message == null)
            //        throw new ArgumentNullException(nameof(message), "There must be error message for failure.");
            //}

            Succeeded = succeeded;
            Error = new List<ErrorResult>();
            Error = errors.ToList();
        }

        public bool NotSucceeded => !Succeeded;
        public bool Succeeded { get; }
        public ICollection<ErrorResult> Error { get; }

        [DebuggerStepThrough]
        public static Result Success()
        {
            return SuccessResult;
        }

        [DebuggerStepThrough]
        public static Result Failed(string message)
        {
            return new Result(false, message, 0);
        }

        [DebuggerStepThrough]
        public static Result Failed(IEnumerable<ErrorResult> errors)
        {
            return new Result(false, errors);
        }

        [DebuggerStepThrough]
        public static Result Failed(ErrorResult error)
        {
            return new Result(false, error.Message, error.Code);
        }

        [DebuggerStepThrough]
        public static Result Failed(string message, int errorCode)
        {
            return new Result(false, message, errorCode);
        }

        [DebuggerStepThrough]
        public static Result<T> Failed<T>(string message)
        {
            return new Result<T>(default, false, message, 0);
        }

        [DebuggerStepThrough]
        public static Result<T> Failed<T>(ErrorResult error)
        {
            return new Result<T>(default, false, error.Message, error.Code);
        }

        [DebuggerStepThrough]
        public static Result<T> Failed<T>(IEnumerable<ErrorResult> errors)
        {
            return new Result<T>(default, false, errors);
        }

        [DebuggerStepThrough]
        public static Result<T> Failed<T>(string message, int errorCode)
        {
            return new Result<T>(default, false, message, errorCode);
        }

        [DebuggerStepThrough]
        public static Result<T> Success<T>(T value)
        {
            return new Result<T>(value, true, string.Empty, 0);
        }

        public override string ToString()
        {
            return Succeeded
                ? "Succeeded"
                : $"Failed : {Error}";
        }

        public virtual object GetValue() => null;

        public class ErrorResult
        {

            public ErrorResult(string message, int code)
            {
                Message = message;
                Code = code;
            }

            public string Message { get; set; }
            public int Code { get; set; }
        }

        public static IEnumerable<ErrorResult> CreateCustomErrorResult(IDictionary<int, string> messages)
        {
            return messages.Select(x => new ErrorResult(x.Value, x.Key));
        }
    }

    public class ResultStatus
    {
        public string ErrorMessage { get; set; }
        public int ErrorCode { get; set; }
        public bool IsSuccess { get; set; }

        [JsonIgnore]
        public bool IsNotSuccess => !IsSuccess;
    }
}
