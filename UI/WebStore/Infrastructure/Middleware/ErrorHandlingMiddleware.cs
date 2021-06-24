using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Infrastructure.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _Next;
        private readonly ILogger<ErrorHandlingMiddleware> _Logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _Next = next;
            _Logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _Next(context);
            }
            catch (Exception e)
            {
                await HandleExeptionAsync(context, e);
                throw;
            }            
        }

        private Task HandleExeptionAsync(HttpContext context, Exception e)
        {
            _Logger.LogError(e, "Error on request {0}", context.Request.Path);

            return Task.CompletedTask;
        }
    }
}
