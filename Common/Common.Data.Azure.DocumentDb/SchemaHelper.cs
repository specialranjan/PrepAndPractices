namespace Common.Data.Azure.DocumentDb
{
    using System;

    /// <summary>
    /// The schema helper.
    /// </summary>
    public static class SchemaHelper
    {
        /// <summary>
        /// _rid is used internally by the DocDB and is required for use with DocDB.
        /// (_rid is resource id)
        /// </summary>
        /// <param name="document">Device data</param>
        /// <returns>_rid property value as string, or empty string if not found</returns>
        public static string GetDocDbRid(dynamic document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var rid = document._rid;

            return rid == null ? string.Empty : rid.ToString();
        }

        /// <summary>
        /// id is used internally by the DocDB and is sometimes required.
        /// </summary>
        /// <param name="document">Device data</param>
        /// <returns>Value of the id, or empty string if not found</returns>
        public static string GetDocDbId(dynamic document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var id = document.id;

            return id == null ? string.Empty : id.ToString();
        }
    }
}
