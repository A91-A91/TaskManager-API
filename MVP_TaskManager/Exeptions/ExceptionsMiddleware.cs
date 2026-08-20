
using Microsoft.AspNetCore.Diagnostics;
namespace MVP_TaskManager.Exeptions
{
    public static class ExceptionsMiddleware
    {
        public static void UseExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exception = context.Features
                        .Get<IExceptionHandlerFeature>()?
                        .Error;

                    var statusCode = exception switch
                    {
                        UnauthorizedAccessException => StatusCodes.Status403Forbidden,
                        InvalidOperationException => StatusCodes.Status400BadRequest,
                        _ => StatusCodes.Status500InternalServerError
                    };

                    await Results.Problem(
                        statusCode: statusCode,
                        title: exception?.GetType().Name,
                        detail: exception?.Message
                    ).ExecuteAsync(context);
                });
            });
        }
    }
}
