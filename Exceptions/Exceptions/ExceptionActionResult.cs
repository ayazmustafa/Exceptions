using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Exceptions.Exceptions
{
    internal sealed class ExceptionActionResult : ActionResult
    {
        private readonly Exception _exception;
        internal Exception Exception { get { return _exception; } }

        public ExceptionActionResult(Exception exception)
        {
            _exception = exception;
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            var httpResponse = context.HttpContext.Response;
            httpResponse.Headers.Add("Exception-Type", _exception.GetType().Name);
            httpResponse.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "We have a problem";
            httpResponse.StatusCode = (int)HttpStatusCode.BadRequest;
            await httpResponse.WriteAsync(_exception.Message).ConfigureAwait(false);
        }
    }
}
