using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;


namespace Common.Web.Extensions
{
    public static class RequestExtensions
    {
        /// <summary>
        /// Helper method that returns the first value of the header <paramref name="headerName" />.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="headerName">Name of the header.</param>
        /// <returns>
        /// First value of header <paramref name="headerName" />. Null if header not found.
        /// </returns>
        public static string GetFirstHeaderValue(this HttpRequestMessage request, string headerName)
        {
            IEnumerable<string> values;
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return request.Headers.TryGetValues(headerName, out values)
                ? values?.FirstOrDefault()
                : null;
        }

        public static string GetFirstHeaderValue(this HttpRequestBase request, string headerName)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return request.Headers.GetValues(headerName)?.FirstOrDefault();
        }

        /// <summary>
        /// Helper method that performs content negotiation and creates a <see cref="HttpResponseMessage" /> representing an error
        /// with an instance of <see cref="ObjectContent{T}" /> wrapping an <see cref="HttpError" /> with message <paramref name="message" />
        /// and error code <paramref name="errorCode" /> if provided.
        /// If no formatter is found, this method returns a response with status 406 NotAcceptable.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="statusCode">The status code of the created response.</param>
        /// <param name="errorCode">The error code</param>
        /// <param name="message">The error message.</param>
        /// <returns>
        /// An error response with error message <paramref name="message" />, error code <paramref name="errorCode" /> (if provided)
        /// and status code <paramref name="statusCode" />.
        /// </returns>
        /// <remarks>
        /// This method requires that <paramref name="request" /> has been associated with an instance of
        /// <see cref="HttpConfiguration" />.
        /// </remarks>
        public static HttpResponseMessage CreateErrorResponse(this HttpRequestMessage request, HttpStatusCode statusCode, int errorCode, string message)
        {
            return CreateErrorResponse(request, statusCode, errorCode.ToString(), message);
        }

        /// <summary>
        /// Helper method that performs content negotiation and creates a <see cref="HttpResponseMessage" /> representing an error
        /// with an instance of <see cref="ObjectContent{T}" /> wrapping an <see cref="HttpError" /> with message <paramref name="message" />
        /// and error code <paramref name="errorCode" /> if provided.
        /// If no formatter is found, this method returns a response with status 406 NotAcceptable.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="statusCode">The status code of the created response.</param>
        /// <param name="errorCode">The error code</param>
        /// <param name="message">The error message.</param>
        /// <returns>
        /// An error response with error message <paramref name="message" />, error code <paramref name="errorCode" /> (if provided)
        /// and status code <paramref name="statusCode" />.
        /// </returns>
        /// <remarks>
        /// This method requires that <paramref name="request" /> has been associated with an instance of
        /// <see cref="HttpConfiguration" />.
        /// </remarks>
        public static HttpResponseMessage CreateErrorResponse(this HttpRequestMessage request, HttpStatusCode statusCode, string errorCode, string message)
        {
            var err = new HttpError { { "errorMessage", message } };
            if (!string.IsNullOrWhiteSpace(errorCode))
            {
                err.Add("errorID", errorCode);
            }

            return request.CreateErrorResponse(statusCode, err);
        }

        public static HttpResponseMessage CreateErrorResponse(this HttpActionContext context, HttpStatusCode statusCode, int errorCode, string message)
        {
            return CreateErrorResponse(context, statusCode, errorCode.ToString(), message);
        }

        public static HttpResponseMessage CreateErrorResponse(this HttpActionContext context, HttpStatusCode statusCode, string errorCode, string message)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Request != null)
            {
                return context.Request.CreateErrorResponse(statusCode, errorCode, message);
            }

            var err = new HttpError { { "errorMessage", message } };

            if (!string.IsNullOrWhiteSpace(errorCode))
            {
                err.Add("errorID", errorCode);
            }

            return ReturnResponseMessageWithContent(statusCode, err);
        }

        public static HttpResponseMessage CreateErrorResponse(this HttpActionContext context, HttpStatusCode statusCode, Exception exception)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            if (context.Request != null)
            {
                return context.Request.CreateErrorResponse(statusCode, exception.Message);
            }

            var err = new HttpError(exception, true);

            return ReturnResponseMessageWithContent(statusCode, err);
        }

        /// <summary>
        /// Helper method that performs content negotiation and creates a <see cref="HttpResponseMessage"/> representing an error
        /// with an instance of <see cref="ObjectContent{T}"/> wrapping an <see cref="HttpError"/> for model state <paramref name="modelState"/>.
        /// If no formatter is found, this method returns a response with status 406 NotAcceptable.
        /// </summary>
        /// <remarks>
        /// This method requires that <paramref name="context"/> has been associated with an instance of
        /// <see cref="HttpConfiguration"/>.
        /// </remarks>
        /// <param name="context"></param>
        /// <param name="statusCode">The status code of the created response.</param>
        /// <param name="modelState">The model state.</param>
        /// <returns>An error response for <paramref name="modelState"/> with status code <paramref name="statusCode"/>.</returns>
        public static HttpResponseMessage CreateErrorResponse(this HttpActionContext context, HttpStatusCode statusCode, ModelStateDictionary modelState)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Request != null)
            {
                return context.Request.CreateErrorResponse(statusCode, modelState);
            }

            var err = new HttpError(modelState, true);

            return ReturnResponseMessageWithContent(statusCode, err);
        }

        public static HttpResponseMessage CreateResponse(this HttpActionContext context, HttpStatusCode statusCode)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Request != null)
            {
                return context.Request.CreateResponse(statusCode);
            }

            return new HttpResponseMessage(statusCode);
        }

        public static HttpResponseMessage CreateResponse<T>(this HttpActionContext context, HttpStatusCode statusCode, T value)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Request != null)
            {
                return context.Request.CreateResponse(statusCode, value);
            }

            return new HttpResponseMessage(statusCode) { Content = new ObjectContent<T>(value, new JsonMediaTypeFormatter()) };
        }


        private static HttpResponseMessage ReturnResponseMessageWithContent<T>(HttpStatusCode statusCode, T content)
        {
            return new HttpResponseMessage(statusCode) { Content = new ObjectContent<T>(content, new JsonMediaTypeFormatter()) };
        }


        public static HttpRequestBase GetRequest()
        {
            return HttpContextFactory.Current.Request;
        }
    }
}
