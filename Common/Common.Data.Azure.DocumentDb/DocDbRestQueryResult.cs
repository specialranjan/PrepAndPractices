namespace Common.Data.Azure.DocumentDb
{
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The document database rest query result.
    /// </summary>
    public class DocDbRestQueryResult
    {
        /// <summary>
        /// Gets or sets the result set.
        /// </summary>
        public JArray ResultSet { get; set; }

        /// <summary>
        /// Gets or sets the total results.
        /// </summary>
        public int TotalResults { get; set; }

        /// <summary>
        /// Gets or sets the continuation token.
        /// </summary>
        public string ContinuationToken { get; set; }
    }
}
