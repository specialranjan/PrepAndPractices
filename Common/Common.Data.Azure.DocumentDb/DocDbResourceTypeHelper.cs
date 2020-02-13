namespace Common.Data.Azure.DocumentDb
{
    using System;

    /// <summary>
    /// The document database resource type helper.
    /// </summary>
    public static class DocDbResourceTypeHelper
    {
        /// <summary>
        /// The get resource type string.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Throws InvalidOperationException
        /// </exception>
        public static string GetResourceTypeString(DocDbResourceType type)
        {
            switch (type)
            {
                case DocDbResourceType.Database:
                    return "dbs";
                case DocDbResourceType.Collection:
                    return "colls";
                case DocDbResourceType.Document:
                    return "docs";
                default:
                    throw new InvalidOperationException("Unknown DocDbResourceType");
            }
        }

        /// <summary>
        /// The get result set key.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Throws InvalidOperationException
        /// </exception>
        public static string GetResultSetKey(DocDbResourceType type)
        {
            switch (type)
            {
                case DocDbResourceType.Database:
                    return "Databases";
                case DocDbResourceType.Collection:
                    return "DocumentCollections";
                case DocDbResourceType.Document:
                    return "Documents";
                default:
                    throw new InvalidOperationException("Unknown DocDbResourceType");
            }
        }
    }
}
