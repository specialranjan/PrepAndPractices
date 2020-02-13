namespace Common.Wrapper.HttpClient
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// The ExceptionHandlingHelper Interface
    /// </summary>
    public interface IExceptionHandlingHelper
    {
        /// <summary>
        /// Gets the error mappings.
        /// </summary>
        /// <returns>The error mappings.</returns>
        IReadOnlyDictionary<HttpStatusCode, Func<HttpResponseMessage, Uri, Task<WebException>>> GetErrorMappings();
    }
}
