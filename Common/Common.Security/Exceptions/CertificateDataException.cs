namespace Common.Security.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the <see cref="CertificateDataException" />
    /// </summary>
    [Serializable]
    public class CertificateDataException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CertificateDataException"/> class.
        /// </summary>
        public CertificateDataException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CertificateDataException"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string"/></param>
        public CertificateDataException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CertificateDataException"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string"/></param>
        /// <param name="innerException">The innerException<see cref="Exception"/></param>
        public CertificateDataException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CertificateDataException"/> class.
        /// </summary>
        /// <param name="info">The info<see cref="SerializationInfo"/></param>
        /// <param name="context">The context<see cref="StreamingContext"/></param>
        protected CertificateDataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
