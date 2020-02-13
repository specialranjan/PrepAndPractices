using System;
using System.Web;

namespace Common.Web
{
    public static class HttpContextFactory
    {
        private static HttpContextBase _mContext;
        /// <summary>
        /// Gets the current Context.
        /// </summary>
        /// <value>
        /// The current Context.
        /// </value>
        /// <exception cref="System.InvalidOperationException">HttpContext not available</exception>
        public static HttpContextBase Current
        {
            get
            {
                if (_mContext != null)
                {
                    return _mContext;
                }

                if (HttpContext.Current == null)
                {
                    throw new InvalidOperationException("HttpContext not available");
                }

                return new HttpContextWrapper(HttpContext.Current);
            }
        }

        /// <summary>
        /// Sets the current context.
        /// </summary>
        /// <param name="context">The context.</param>
        public static void SetCurrentContext(HttpContextBase context)
        {
            _mContext = context;
        }
    }
}
