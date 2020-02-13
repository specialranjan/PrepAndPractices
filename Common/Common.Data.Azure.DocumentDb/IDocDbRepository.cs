namespace Common.Data.Azure.DocumentDb
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;

    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The Document Database Utility interface.
    /// </summary>
    public interface IDocDbRepository
    {
        /// <summary>
        /// Initializes the database
        /// </summary>
        /// <returns>Async Task</returns>
        Task InitializeDatabaseAsync();

        /// <summary>
        /// Initializes the collection
        /// </summary>
        /// <returns>Async Task</returns>
        Task InitializeCollectionAsync();

        /// <summary>
        /// Queries the collection
        /// https://msdn.microsoft.com/en-us/library/azure/dn783363.aspx
        /// </summary>
        /// <param name="queryString">Query string</param>
        /// <param name="queryParams">Query parameters</param>
        /// <param name="pageSize">Page size for result paging</param>
        /// <param name="continuationToken">Continuation token for next page</param>
        /// <returns>One page of results, with metadata</returns>
        Task<DocDbRestQueryResult> QueryCollectionAsync(
            string queryString, Dictionary<string, object> queryParams, int pageSize = -1, string continuationToken = null);

        /// <summary>
        /// The query partitioned collection async.
        /// </summary>
        /// <param name="queryString">
        /// The query string.
        /// </param>
        /// <param name="queryParams">
        /// The query parameters.
        /// </param>
        /// <param name="partitionId">
        /// The partition id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<DocDbRestQueryResult> QueryPartitionedCollectionAsync(
            string queryString,
            Dictionary<string, object> queryParams,
            string partitionId);

        /// <summary>
        /// The query partitioned collection async.
        /// </summary>
        /// <param name="queryString">
        /// The query string.
        /// </param>
        /// <param name="queryParams">
        /// The query parameters.
        /// </param>
        /// <param name="partitionId">
        /// The partition id.
        /// </param>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <param name="maxDegreeOfParallelism">
        /// The max degree of parallelism.
        /// </param>
        /// <param name="continuationToken">
        /// The continuation token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<DocDbRestQueryResult> QueryPartitionedCollectionAsync(
            string queryString,
            Dictionary<string, object> queryParams,
            string partitionId,
            int pageSize,
            int maxDegreeOfParallelism,
            string continuationToken);

        /// <summary>
        /// The query partitioned collection async.
        /// </summary>
        /// <param name="queryString">
        /// The query string.
        /// </param>
        /// <param name="queryParams">
        /// The query parameters.
        /// </param>
        /// <param name="partitionId">
        /// The partition id.
        /// </param>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <param name="continuationToken">
        /// The continuation token.
        /// </param>
        /// <param name="enableCrossPartitionQuery">
        /// The enable cross partition query.
        /// </param>
        /// <param name="enableScanInQuery">
        /// The enable scan in query.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<DocDbRestQueryResult> QueryPartitionedCollectionAsync(
            string queryString,
            Dictionary<string, object> queryParams,
            string partitionId,
            int pageSize,
            string continuationToken,
            bool enableCrossPartitionQuery,
            bool enableScanInQuery);

        /// <summary>
        /// The query partitioned collection async.
        /// </summary>
        /// <param name="queryString">
        /// The query string.
        /// </param>
        /// <param name="queryParams">
        /// The query parameters.
        /// </param>
        /// <param name="partitionId">
        /// The partition id.
        /// </param>
        /// <param name="enableCrossPartitionQuery">
        /// The enable cross partition query.
        /// </param>
        /// <param name="enableScanInQuery">
        /// The enable scan in query.
        /// </param>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <param name="continuationToken">
        /// The continuation token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<DocDbRestQueryResult> QueryPartitionedCollectionAsync(
            string queryString,
            Dictionary<string, object> queryParams,
            string partitionId,
            bool enableCrossPartitionQuery,
            bool enableScanInQuery,
            int pageSize,
            string continuationToken);

        /// <summary>
        /// Saves a new document
        /// </summary>
        /// <param name="document">Document instance</param>
        /// <returns>Save document instance</returns>
        Task<JObject> SaveNewDocumentAsync(JObject document);

        /// <summary>
        /// The save new document async.
        /// </summary>
        /// <param name="document">
        /// The document.
        /// </param>
        /// <param name="partitionId">
        /// The partition id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<JObject> SaveNewDocumentAsync(JObject document, string partitionId);

        /// <summary>
        /// Update the record for an existing document.
        /// </summary>
        /// <param name="updatedDocument">Document to be updated</param>
        /// <returns>Updated document</returns>
        Task<JObject> UpdateDocumentAsync(JObject updatedDocument);

        /// <summary>
        /// Update the record for an existing document in partitioned collection.
        /// </summary>
        /// <param name="updatedDocument">
        /// The updated document.
        /// </param>
        /// <param name="partitionId">
        /// The partition id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<JObject> UpdateDocumentAsync(JObject updatedDocument, string partitionId);

        /// <summary>
        /// Update record for the existing document with access condition
        /// </summary>
        /// <param name="updatedDocument">The updated document.</param>
        /// <param name="accessCondition">The access condition.</param>
        /// <returns>Flag, whether the document was update or not</returns>
        Task<bool> UpdateDocumentAsync(Document updatedDocument, AccessCondition accessCondition);

        /// <summary>
        /// Update record for the existing document with access condition
        /// </summary>
        /// <param name="updatedDocument">The updated document.</param>
        /// <param name="accessCondition">The access condition.</param>
        /// <param name="partitionId">The partition identifier.</param>
        /// <returns>Flag, whether the document was update or not</returns>
        Task<bool> UpdateDocumentAsync(Document updatedDocument, AccessCondition accessCondition, string partitionId);

        /// <summary>
        /// Remove a document from the DocumentDB. If it succeeds the method will return asynchronously.
        /// If it fails for any reason it will let any exception thrown bubble up.
        /// </summary>
        /// <param name="document">Document to be deleted</param>
        /// <returns>Async Task</returns>
        Task DeleteDocumentAsync(JObject document);

        /// <summary>
        /// Remove a document from the DocumentDB partitioned collection. If it succeeds the method will return asynchronously.
        /// If it fails for any reason it will let any exception thrown bubble up.
        /// </summary>
        /// <param name="document">
        /// The document.
        /// </param>
        /// <param name="partitionId">
        /// The partition id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task DeleteDocumentAsync(JObject document, string partitionId);

        /// <summary>
        ///  Executes stored procedure
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure</param>
        /// <param name="args">documents arguments</param>
        /// <returns>Async Task</returns>
        Task<object> ExecuteStoredProcedureAsync(string storedProcedureName, dynamic[] args);

        /// <summary>
        /// The create document collection if not exists async.
        /// </summary>
        /// <param name="partitionId">
        /// The partition id.
        /// </param>
        /// <param name="indexingMode">
        /// The indexing mode.
        /// </param>
        /// <param name="defaultTtl">The default TTL</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task CreateDocumentCollectionIfNotExistsAsync(string partitionId, IndexingMode indexingMode, int? defaultTtl);

        /// <summary>
        /// Creates the document collection if not exists asynchronous.
        /// </summary>
        /// <param name="indexingPolicy">The indexing policy.</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task CreateDocumentCollectionIfNotExistsAsync(IndexingPolicy indexingPolicy = null);

        /// <summary>
        /// The create document collection if not exists async.
        /// </summary>
        /// <param name="partitionId">
        /// The partition id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task CreateDocumentCollectionIfNotExistsAsync(string partitionId);

        /// <summary>
        /// This method ensures stored procedure exist if not then it creates the stored procedure.
        /// </summary>
        /// <param name="storedProcId">
        /// The stored procedure id/name.
        /// </param>
        /// <param name="body">
        /// The stored procedure body.
        /// </param>
        /// <param name="forceOverwrite">
        /// flag gives an option to overwrite the stored procedure if it already exist.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task EnsureStoredProcedureExistAsync(string storedProcId, string body, bool forceOverwrite);

        /// <summary>
        /// The execute stored procedure async.
        /// </summary>
        /// <param name="storedProcedureName">
        /// The stored procedure name.
        /// </param>
        /// <param name="partitionKey">
        /// The partition key.
        /// </param>
        /// <param name="args">
        /// Stored Procedure parameters.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<object> ExecuteStoredProcedureAsync(string storedProcedureName, string partitionKey, dynamic[] args);

        /// <summary>
        /// Queries the partitioned collection for complete result asynchronous.
        /// </summary>
        /// <param name="queryString">The query string.</param>
        /// <param name="queryParams">The query parameters.</param>
        /// <param name="partitionId">The partition identifier.</param>
        /// <param name="enableCrossPartitionQuery">if set to <c>true</c> [enable cross partition query].</param>
        /// <param name="enableScanInQuery">if set to <c>true</c> [enable scan in query].</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="maxDegreeOfParallelism">The maximum degree of parallelism.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<List<DocDbRestQueryResult>> QueryPartitionedCollectionForCompleteResultAsync(
            string queryString,
            Dictionary<string, object> queryParams,
            string partitionId,
            bool enableCrossPartitionQuery,
            bool enableScanInQuery,
            int pageSize,
            int maxDegreeOfParallelism);

        /// <summary>
        ///  Executes stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure</param>
        /// <param name="args">documents arguments</param>
        /// <param name="vin">vin parameter</param>
        /// <param name="tokens">The tokens dictionary.</param>
        /// <param name="delay">The delay.</param>
        /// <returns>Async Task</returns>
        Task<List<object>> ExecuteStoredProcedureAsync(string storedProcedureName, dynamic[] args, string vin, Dictionary<string, string> tokens, int delay);

        /// <summary>
        /// The delete document collection async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<bool> DeleteDocumentCollectionAsync();
    }
}
