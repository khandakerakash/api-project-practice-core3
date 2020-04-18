using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Utility.Exceptions;
using Utility.Helpers;

namespace API.Middleware
{
    public class MyAppExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;

        public MyAppExceptionMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }
        
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Call the next delegate/middleware in the pipeline
                await _next(context);
            }
            catch (Exception e)
            {
                await HandleProjectExceptions(e, context);
            }
        }

        private async Task HandleProjectExceptions(Exception exception, HttpContext context)
        {
            var code = HttpStatusCode.InternalServerError;
            
            var error = new ErrorResponse();
            error.StatusCode = (int) code;
            error.Message = exception.Message;

            if (_env.IsDevelopment())
            {
                error.DevMessage = exception.StackTrace;
            }
            else
            {
                error.DevMessage = exception.Message;
            }
            

            switch (exception)
            {
                case MyAppException myAppException:
                    error.StatusCode = (int) HttpStatusCode.NotFound;
                    break;
                case UnauthorizedAccessException unauthorizedAccessException:
                    error.StatusCode = (int) HttpStatusCode.Unauthorized;
                    error.Message = "You're not authorized!";
                    break;
                default:
                    break;
            }

            var result = JsonConvert.SerializeObject(error);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = error.StatusCode;
            await context.Response.WriteAsync(result);
        }
    }
}