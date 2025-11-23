using Microsoft.AspNetCore.Mvc;
using ScotlandsMountains.Shared;

namespace ScotlandsMountains.Api.Extensions;

public static class ResultExtensions
{
    public static IActionResult Map<T>(this Result<T> result, Func<T, IActionResult> onSuccess)
    {
        if (result.IsFailure)
        {
            return result.Error.Type switch
            {
                Errors.NotFound => new NotFoundResult(),
                Errors.BadRequest => new BadRequestResult(),
                Errors.Unknown => new StatusCodeResult(StatusCodes.Status500InternalServerError),
                _ => new BadRequestResult()
            };
        }
        return onSuccess(result.Value);
    }
}
