using Microsoft.AspNetCore.Mvc;
using System;

namespace PaymentGateway.Common
{
    public static class ResultExtension
    {
        public static IActionResult ToHttpCodeResult(this Result result) => ReturnObjectResult(result);

        private static IActionResult ReturnResult(Result result)
        {
            if (result.NotSucceeded)
            {
                return new BadRequestObjectResult(result.Error);
            }

            return new OkResult();
        }

        private static IActionResult ReturnObjectResult(Result result)
        {
            if (result.GetValue() == null)
                return ReturnResult(result);

            return new OkObjectResult(result.GetValue());
        }



        public static Result<T> FailedFromResultStatus<T>(this T obj) where T : ResultStatus
        {
            if (obj.IsSuccess)
                throw new InvalidOperationException("Result status is success");

            return Result.Failed<T>(new Result.ErrorResult(obj.ErrorMessage, obj.ErrorCode));
        }

        public static Result<T> SuitableOutput<T>(this Result<T> dataResult) where T : ResultStatus
        {
            if (dataResult.NotSucceeded)
                return Result.Failed<T>(dataResult.Error);

            var data = dataResult.Value;
            if (data.IsNotSuccess)
                return data.FailedFromResultStatus();

            return Result.Success(dataResult.Value);
        }
    }
}
