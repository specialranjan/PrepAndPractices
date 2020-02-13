namespace Common.Data.Azure.DocumentDb
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Security;
    using System.Threading.Tasks;

    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The document database utility.
    /// </summary>
    public class DocDbRepository : IDocDbRepository, IDisposable
    {
        /// <summary>
        /// The document database name.
        /// </summary>
        private readonly string databaseName;

        /// <summary>
        /// The document database client
        /// </summary>
        private readonly IDocumentClient documentDbClient;

        /// <summary>
        /// The collection name.
        /// </summary>
        private readonly string collectionName;

        /// <summary>
        /// The collection self link.
        /// </summary>
        private readonly string collectionSelfLink;

        /// <summary>
        /// The key.
        /// </summary>
        private readonly SecureString key;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocDbRepository"/> class.
        /// </summary>
        /// <param name="docDbEndpoint">DocumentDB end point</param>
        /// <param name="docDbKey">DocumentDB authorization key</param>
        /// <param name="databaseName">DocumentDB database name</param>
        /// <param name="collectionName">DocumentDB collection name</param>
        public DocDbRepository(
            string docDbEndpoint,
            string docDbKey,
            string databaseName,
            string collectionName)
        {
            if (docDbKey == null)
            {
                throw new ArgumentNullException(nameof(docDbKey));
            }

            this.databaseName = databaseName;
            this.collectionName = collectionName;
            this.collectionSelfLink = string.Format(
                CultureInfo.InvariantCulture,
                "dbs/{0}/colls/{1}",
                this.databaseName,
                this.collectionName);
            this.key = new SecureString();
            foreach (var c in docDbKey)
            {
                this.key.AppendChar(c);
            }

            this.documentDbClient = this.CreateClient(docDbEndpoint, ConnectionMode.Direct, Protocol.Tcp);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocDbRepository"/> class.
        /// </summary>
        /// <param name="docDbEndpoint">
        /// The doc DB endpoint.
        /// </param>
        /// <param name="docDbKey">
        /// The doc DB key.
        /// </param>
        /// <param name="databaseName">
        /// The database name.
        /// </param>
        /// <param name="collectionName">
        /// The collection name.
        /// </param>
        /// <param name="connectionMode">
        /// The connection mode.
        /// </param>
        /// <param name="protocol">
        /// The protocol.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// invalid argument
        /// </exception>
        public DocDbRepository(
            string docDbEndpoint,
            string docDbKey,
            string databaseName,
            string collectionName,
            ConnectionMode connectionMode,
            Protocol protocol)
        {
            if (docDbKey == null)
            {
                throw new ArgumentNullException(nameof(docDbKey));
            }

            this.databaseName = databaseName;
            this.collectionName = collectionName;
            this.collectionSelfLink = string.Format(
                CultureInfo.InvariantCulture,
                "dbs/{0}/colls/{1}",
                this.databaseName,
                this.collectionName);
            this.key = new SecureString();
            foreach (var c in docDbKey)
            {
                this.key.AppendChar(c);
            }

            this.documentDbClient = this.CreateClient(docDbEndpoint, connectionMode, protocol);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocDbRepository"/> class.
        /// </summary>
        /// <param name="docDbEndpoint">The document database endpoint.</param>
        /// <param name="docDbKey">The document database key.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="retryOptions">The retry options.</param>
        /// <exception cref="ArgumentNullException">Invalid document database key argument.</exception>
        public DocDbRepository(
            string docDbEndpoint,
            string docDbKey,
            string databaseName,
            string collectionName,
            RetryOptions retryOptions)
        {
            if (docDbKey == null)
            {
                throw new ArgumentNullException(nameof(docDbKey));
            }

            this.databaseName = databaseName;
            this.collectionName = collectionName;
            this.collectionSelfLink = string.Format(
                CultureInfo.InvariantCulture,
                "dbs/{0}/colls/{1}",
                this.databaseName,
                this.collectionName);
            this.key = new SecureString();
            foreach (var c in docDbKey)
            {
                this.key.AppendChar(c);
            }

            this.documentDbClient = this.CreateClient(docDbEndpoint, ConnectionMode.Direct, Protocol.Tcp, retryOptions);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocDbRepository"/> class.
        /// </summary>
        /// <param name="docDbEndpoint">The document database endpoint.</param>
        /// <param name="docDbKey">The document database key.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="connectionMode">The connection mode.</param>
        /// <param name="protocol">The protocol.</param>
        /// <param name="retryOptions">The retry options.</param>
        /// <exception cref="ArgumentNullException">Invalid document database key argument.</exception>
        public DocDbRepository(
            string docDbEndpoint,
            string docDbKey,
            string databaseName,
            string collectionName,
            ConnectionMode connectionMode,
            Protocol protocol,
            RetryOptions retryOptions)
        {
            if (docDbKey == null)
            {
                throw new ArgumentNullException(nameof(docDbKey));
            }

            this.databaseName = databaseName;
            this.collectionName = collectionName;
            this.collectionSelfLink = string.Format(
                CultureInfo.InvariantCulture,
                "dbs/{0}/colls/{1}",
                this.databaseName,
                this.collectionName);
            this.key = new SecureString();
            foreach (var c in docDbKey)
            {
                this.key.AppendChar(c);
            }

            this.documentDbClient = this.CreateClient(docDbEndpoint, connectionMode, protocol, retryOptions);
        }

        /// <summary>
        /// Initializes the database
        /// </summary>
        /// <returns>Async Task</returns>
        public async Task InitializeDatabaseAsync()
        {
            await this.CreateDatabaseIfNotExistsAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Initializes the collection
        /// </summary>
        /// <returns>Async Task</returns>
        public async Task InitializeCollectionAsync()
        {
            await this.CreateDocumentCollectionIfNotExistsAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Queries the collection
        /// https://msdn.microsoft.com/en-us/library/azure/dn783363.aspx
        /// </summary>
        /// <param name="queryString">Query string</param>
        /// <param name="queryParams">Query parameters</param>
        /// <param name="pageSize">Page size for result paging</param>
        /// <param name="continuationToken">Continuation token for next page</param>
        /// <returns>One page of results, with meta data</returns>
        public async Task<DocDbRestQueryResult> QueryCollectionAsync(
            string queryString,
            Dictionary<string, object> queryParams,
            int pageSize = -1,
            string continuationToken = null)
        {
            if (string.IsNullOrWhiteSpace(queryString))
            {
                throw new ArgumentNullException(nameof(queryString));
            }

            var query =
                this.documentDbClient.CreateDocumentQuery<JObject>(
                        UriFactory.CreateDocumentCollectionUri(this.databaseName, this.collectionName),
                        new SqlQuerySpec
                        {
                            QueryText = queryString,
                            Parameters = ToSqlQueryParamtereCollection(queryParams)
                        },
                        new FeedOptions { MaxItemCount = pageSize, RequestContinuation = continuationToken })
                    .AsDocumentQuery();

            var response = await query.ExecuteNextAsync<JObject>().ConfigureAwait(false);
            var result = new DocDbRestQueryResult
            {
                ResultSet = new JArray(response.ToArray()),
                TotalResults = response.Count,
                ContinuationToken = query.HasMoreResults ? response.ResponseContinuation : null
            };

            return result;
        }

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
        public async Task<DocDbRestQueryResult> QueryPartitionedCollectionAsync(
            string queryString,
            Dictionary<string, object> queryParams,
            string partitionId)
        {
            return await this.QueryDocumentCollectionAsync(queryString, queryParams, partitionId, true, true, -1, -1, null).ConfigureAwait(false);
        }

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
        /// <param name="enableCrossPartitionQuery">enable Cross Partition Query</param>
        /// <param name="enableScanInQuery">enable Scan In Query</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="maxDegreeOfParallelism">The max degree of parallelism</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<List<DocDbRestQueryResult>> QueryPartitionedCollectionForCompleteResultAsync(
            string queryString,
            Dictionary<string, object> queryParams,
            string partitionId,
            bool enableCrossPartitionQuery,
            bool enableScanInQuery,
            int pageSize,
            int maxDegreeOfParallelism)
        {
            var isValid = true;
            string continuationToken = null;
            var result = new List<DocDbRestQueryResult>();
            while (isValid)
            {
                var response = await this.QueryDocumentCollectionAsync(queryString, queryParams, partitionId, enableCrossPartitionQuery, enableScanInQuery, pageSize, maxDegreeOfParallelism, continuationToken).ConfigureAwait(false);
                result.Add(response);
                continuationToken = response.ContinuationToken;
                if (string.IsNullOrWhiteSpace(response.ContinuationToken))
                {
                    isValid = false;
                }
            }

            return result;
        }

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
        public async Task<DocDbRestQueryResult> QueryPartitionedCollectionAsync(
            string queryString,
            Dictionary<string, object> queryParams,
            string partitionId,
            int pageSize,
            int maxDegreeOfParallelism,
            string continuationToken)
        {
            return await this.QueryDocumentCollectionAsync(queryString, queryParams, partitionId, true, true, pageSize, maxDegreeOfParallelism, continuationToken).ConfigureAwait(false);
        }

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
        public async Task<DocDbRestQueryResult> QueryPartitionedCollectionAsync(
            string queryString,
            Dictionary<string, object> queryParams,
            string partitionId,
            int pageSize,
            string continuationToken,
            bool enableCrossPartitionQuery,
            bool enableScanInQuery)
        {
            return await this.QueryDocumentCollectionAsync(queryString, queryParams, partitionId, true, true, pageSize, -1, continuationToken).ConfigureAwait(false);
        }

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
        public async Task<DocDbRestQueryResult> QueryPartitionedCollectionAsync(
            string queryString,
            Dictionary<string, object> queryParams,
            string partitionId,
            bool enableCrossPartitionQuery,
            bool enableScanInQuery,
            int pageSize,
            string continuationToken)
        {
            return await this.QueryDocumentCollectionAsync(queryString, queryParams, partitionId, enableCrossPartitionQuery, enableScanInQuery, pageSize, -1, continuationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Saves a new document
        /// </summary>
        /// <param name="document">Document instance</param>
        /// <returns>Save document instance</returns>
        public async Task<JObject> SaveNewDocumentAsync(JObject document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (document["id"] == null)
            {
                document["id"] = Guid.NewGuid().ToString();
            }

            var response =
                await
                    this.documentDbClient.CreateDocumentAsync(
                        UriFactory.CreateDocumentCollectionUri(this.databaseName, this.collectionName),
                        document).ConfigureAwait(false);
            return JObject.Parse(response.Resource.ToString());
        }

        /// <summary>
        /// The save new document async in Partitioned Collection.
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
        /// <exception cref="ArgumentNullException">
        /// Document is null
        /// </exception>
        public async Task<JObject> SaveNewDocumentAsync(JObject document, string partitionId)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (string.IsNullOrEmpty(partitionId))
            {
                throw new ArgumentNullException(nameof(partitionId));
            }

            if (document["id"] == null)
            {
                document["id"] = Guid.NewGuid().ToString();
            }

            var response =
                await
                    this.documentDbClient.CreateDocumentAsync(
                        UriFactory.CreateDocumentCollectionUri(this.databaseName, this.collectionName),
                        document,
                        new RequestOptions { PartitionKey = new PartitionKey(partitionId) }).ConfigureAwait(false);
            return JObject.Parse(response.Resource.ToString());
        }

        /// <summary>
        /// Update the record for an existing document.
        /// </summary>
        /// <param name="updatedDocument">Document to be updated</param>
        /// <returns>Updated document</returns>
        public async Task<JObject> UpdateDocumentAsync(JObject updatedDocument)
        {
            if (updatedDocument == null)
            {
                throw new ArgumentNullException(nameof(updatedDocument));
            }

            var response =
                await
                    this.documentDbClient.ReplaceDocumentAsync(
                        UriFactory.CreateDocumentUri(
                            this.databaseName,
                            this.collectionName,
                            updatedDocument["id"].Value<string>()),
                        updatedDocument).ConfigureAwait(false);
            return JObject.Parse(response.Resource.ToString());
        }

        /// <summary>
        /// The update document in partitioned collection.
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
        /// <exception cref="ArgumentNullException">
        /// Null argument exception
        /// </exception>
        public async Task<JObject> UpdateDocumentAsync(JObject updatedDocument, string partitionId)
        {
            if (updatedDocument == null)
            {
                throw new ArgumentNullException(nameof(updatedDocument));
            }

            if (updatedDocument["id"] == null)
            {
                throw new ArgumentNullException(nameof(updatedDocument), "Id can not be null");
            }

            if (string.IsNullOrWhiteSpace(partitionId))
            {
                throw new ArgumentNullException(nameof(partitionId));
            }

            var response =
                await
                    this.documentDbClient.ReplaceDocumentAsync(
                        UriFactory.CreateDocumentUri(
                            this.databaseName,
                            this.collectionName,
                            updatedDocument["id"].Value<string>()),
                        updatedDocument,
                        new RequestOptions { PartitionKey = new PartitionKey(partitionId) }).ConfigureAwait(false);
            return JObject.Parse(response.Resource.ToString());
        }

        /// <summary>
        /// Update record for the existing document with access condition
        /// </summary>
        /// <param name="updatedDocument">The updated document.</param>
        /// <param name="accessCondition">The access condition.</param>
        /// <returns>
        /// Flag, whether the document was update or not
        /// </returns>
        /// <exception cref="ArgumentNullException">Invalid access condition</exception>
        public async Task<bool> UpdateDocumentAsync(Document updatedDocument, AccessCondition accessCondition)
        {
            //Requires.NotNull(updatedDocument, nameof(updatedDocument));

            if (accessCondition == null)
            {
                throw new ArgumentNullException(nameof(accessCondition));
            }

            var requestOption = new RequestOptions
            {
                AccessCondition = accessCondition
            };

            try
            {
                await this.documentDbClient.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(this.databaseName, this.collectionName), updatedDocument, requestOption, true).ConfigureAwait(false);
            }
            catch (DocumentClientException e) when (e.StatusCode.HasValue && e.StatusCode.Value == HttpStatusCode.PreconditionFailed)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Update record for the existing document with access condition
        /// </summary>
        /// <param name="updatedDocument">The updated document.</param>
        /// <param name="accessCondition">The access condition.</param>
        /// <param name="partitionId">The partition identifier.</param>
        /// <returns>
        /// Flag, whether the document was update or not
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// partitionId
        /// or
        /// accessCondition
        /// </exception>
        public async Task<bool> UpdateDocumentAsync(Document updatedDocument, AccessCondition accessCondition, string partitionId)
        {
            //Requires.NotNull(updatedDocument, nameof(updatedDocument));

            if (string.IsNullOrWhiteSpace(partitionId))
            {
                throw new ArgumentNullException(nameof(partitionId));
            }

            if (accessCondition == null)
            {
                throw new ArgumentNullException(nameof(accessCondition));
            }

            var requestOption = new RequestOptions
            {
                AccessCondition = accessCondition,
                PartitionKey = new PartitionKey(partitionId)
            };

            try
            {
                await this.documentDbClient.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(this.databaseName, this.collectionName), updatedDocument, requestOption, true).ConfigureAwait(false);
            }
            catch (DocumentClientException e) when (e.StatusCode.HasValue && e.StatusCode.Value == HttpStatusCode.PreconditionFailed)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Remove a document from the DocumentDB. If it succeeds the method will return asynchronously.
        /// If it fails for any reason it will let any exception thrown bubble up.
        /// </summary>
        /// <param name="document">Document to be deleted</param>
        /// <returns>Async Task</returns>
        public async Task DeleteDocumentAsync(JObject document)
        {
            var rid = SchemaHelper.GetDocDbId(document);
            await
                this.documentDbClient.DeleteDocumentAsync(
                    UriFactory.CreateDocumentUri(this.databaseName, this.collectionName, rid)).ConfigureAwait(false);
        }

        /// <summary>
        /// The delete document asynchronous.
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
        public async Task DeleteDocumentAsync(JObject document, string partitionId)
        {
            var rid = SchemaHelper.GetDocDbId(document);
            await
               this.documentDbClient.DeleteDocumentAsync(
                   UriFactory.CreateDocumentUri(this.databaseName, this.collectionName, rid),
                   new RequestOptions { PartitionKey = new PartitionKey(partitionId) }).ConfigureAwait(false);
        }

        /// <summary>
        /// The delete document collection asynchronous.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<bool> DeleteDocumentCollectionAsync()
        {
            try
            {
                await
                    this.documentDbClient.DeleteDocumentCollectionAsync(
                        UriFactory.CreateDocumentCollectionUri(this.databaseName, this.collectionName)).ConfigureAwait(false);
                return true;
            }
            catch (DocumentClientException de)
            {
                return de.StatusCode == HttpStatusCode.NotFound;
            }
        }

        /// <summary>
        ///  Executes stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure</param>
        /// <param name="args">documents arguments</param>
        /// <returns>Asynchronous Task</returns>
        public async Task<object> ExecuteStoredProcedureAsync(string storedProcedureName, dynamic[] args)
        {
            if (storedProcedureName == null)
            {
                throw new ArgumentNullException(nameof(storedProcedureName));
            }

            var sprocList = this.documentDbClient.CreateStoredProcedureQuery(this.collectionSelfLink).ToList();
            var sproc = sprocList.Where(s => s.Id == storedProcedureName).AsEnumerable().FirstOrDefault();

            if (sproc == null)
            {
                throw new ArgumentNullException(
                          $"Stored Procedure named '{storedProcedureName}' does not exist in DocumentDB.");
            }

            // execute the batch.
            var response =
                await this.documentDbClient.ExecuteStoredProcedureAsync<object>(sproc.SelfLink, args).ConfigureAwait(false);
            return response.Response;
        }

        /// <summary>
        ///  Executes stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure</param>
        /// <param name="args">documents arguments</param>
        /// <param name="vin">VIN parameter</param>
        /// <param name="tokens">The tokens</param>
        /// <param name="delay">The delay</param>
        /// <returns>Asynchronous Task</returns>
        public async Task<List<object>> ExecuteStoredProcedureAsync(string storedProcedureName, dynamic[] args, string vin, Dictionary<string, string> tokens, int delay)
        {
            var result = new List<object>();
            var isValid = true;
            while (isValid)
            {
                if (args == null)
                {
                    throw new ArgumentNullException(nameof(args));
                }

                args[args.Length - 1] = tokens;
                var res = await this.ExecuteStoredProcedureAsync(storedProcedureName, vin, args).ConfigureAwait(false);

                var storedProcResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(res.ToString());

                object objToken;
                const string ObjTokenString = "continuationTokens";
                storedProcResponse.TryGetValue(ObjTokenString, out objToken);
                if (objToken == null)
                {
                    throw new InvalidOperationException();
                }

                tokens = JsonConvert.DeserializeObject<Dictionary<string, string>>(objToken.ToString());

                object objResult;
                const string ObjResultString = "result";
                storedProcResponse.TryGetValue(ObjResultString, out objResult);
                result.Add(objResult);
                if (tokens.All(t => string.IsNullOrWhiteSpace(t.Value)))
                {
                    isValid = false;
                }
                else
                {
                    await Task.Delay(delay).ConfigureAwait(false);
                }
            }

            return result;
        }

        /// <summary>
        /// Dispose Document Database Client.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The create document collection if not exists async.
        /// </summary>
        /// <param name="partitionId">
        /// The partition id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task CreateDocumentCollectionIfNotExistsAsync(string partitionId)
        {
            await this.CreateDocumentPartitionedCollectionIfNotExistsAsync(partitionId, IndexingMode.Consistent, null).ConfigureAwait(false);
        }

        /// <summary>
        /// The create document collection if not exists async.
        /// </summary>
        /// <param name="indexingPolicy">The indexing policy</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task CreateDocumentCollectionIfNotExistsAsync(IndexingPolicy indexingPolicy = null)
        {
            try
            {
                await
                    this.documentDbClient.ReadDocumentCollectionAsync(
                            UriFactory.CreateDocumentCollectionUri(this.databaseName, this.collectionName))
                        .ConfigureAwait(false);
            }
            catch (DocumentClientException de)
            {
                // If the document collection does not exist, create a new collection
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    var collectionInfo = new DocumentCollection
                    {
                        Id = this.collectionName,
                        IndexingPolicy = indexingPolicy ??
                                new IndexingPolicy(new RangeIndex(DataType.String) { Precision = -1 })
                    };

                    await
                        this.documentDbClient.CreateDocumentCollectionAsync(
                            UriFactory.CreateDatabaseUri(this.databaseName),
                            collectionInfo).ConfigureAwait(false);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// The create document collection if not exists asynchronous.
        /// </summary>
        /// <param name="partitionId">
        /// The partition id.
        /// </param>
        /// <param name="indexingMode">
        /// The indexing mode.
        /// </param>
        /// <param name="defaultTtl">The default Time to live.</param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task CreateDocumentCollectionIfNotExistsAsync(
            string partitionId,
            IndexingMode indexingMode,
            int? defaultTtl)
        {
            await this.CreateDocumentPartitionedCollectionIfNotExistsAsync(partitionId, indexingMode, defaultTtl).ConfigureAwait(false);
        }

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
        public async Task EnsureStoredProcedureExistAsync(string storedProcId, string body, bool forceOverwrite)
        {
            var sprocList = this.documentDbClient.CreateStoredProcedureQuery(this.collectionSelfLink).ToList();
            var sproc = sprocList.Where(s => s.Id == storedProcId).AsEnumerable().FirstOrDefault();

            if (sproc != null)
            {
                if (forceOverwrite)
                {
                    await this.documentDbClient.DeleteStoredProcedureAsync(sproc.SelfLink).ConfigureAwait(false);
                }
                else
                {
                    return;
                }
            }

            await this.documentDbClient.CreateStoredProcedureAsync(
                    UriFactory.CreateDocumentCollectionUri(this.databaseName, this.collectionName),
                    new StoredProcedure { Id = storedProcId, Body = body }).ConfigureAwait(false);
        }

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
        /// <exception cref="ArgumentNullException">
        /// Exception is thrown if the stored procedure or partitionKey is empty
        /// </exception>
        public async Task<object> ExecuteStoredProcedureAsync(string storedProcedureName, string partitionKey, dynamic[] args)
        {
            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException(nameof(storedProcedureName));
            }

            if (string.IsNullOrEmpty(partitionKey))
            {
                throw new ArgumentNullException(nameof(partitionKey));
            }

            var sprocList = this.documentDbClient.CreateStoredProcedureQuery(this.collectionSelfLink).ToList();
            var sproc = sprocList.Where(s => s.Id == storedProcedureName).AsEnumerable().FirstOrDefault();

            if (sproc == null)
            {
                throw new ArgumentNullException(
                          $"Stored Procedure named '{storedProcedureName}' does not exist in DocumentDB.");
            }

            // execute the stored procedure.
            var scriptResult =
                await
                    this.documentDbClient.ExecuteStoredProcedureAsync<object>(
                        sproc.SelfLink,
                        new RequestOptions { PartitionKey = new PartitionKey(partitionKey) },
                        args).ConfigureAwait(false);
            return scriptResult.Response;
        }

        /// <summary>
        /// Dispose Document Database client object.
        /// </summary>
        /// <param name="disposing">
        /// Flag representing dispose action.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.key != null)
                {
                    this.key.Dispose();
                }
            }
        }

        /// <summary>
        /// Converts the SQL parameter collection
        /// </summary>
        /// <param name="queryParams">Query parameters</param>
        /// <returns>SQL parameter collection</returns>
        private static SqlParameterCollection ToSqlQueryParamtereCollection(Dictionary<string, object> queryParams)
        {
            var coll = new SqlParameterCollection();
            foreach (var paramKey in queryParams.Keys)
            {
                coll.Add(new SqlParameter(paramKey, queryParams[paramKey]));
            }

            return coll;
        }

        /// <summary>
        /// The create document partitioned collection if not exists async.
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
        /// <exception cref="ArgumentNullException">
        /// null value in parameters
        /// </exception>
        private async Task CreateDocumentPartitionedCollectionIfNotExistsAsync(
            string partitionId,
            IndexingMode indexingMode,
            int? defaultTtl)
        {
            if (string.IsNullOrWhiteSpace(partitionId))
            {
                throw new ArgumentNullException(nameof(partitionId));
            }

            try
            {
                await
                    this.documentDbClient.ReadDocumentCollectionAsync(
                            UriFactory.CreateDocumentCollectionUri(this.databaseName, this.collectionName))
                        .ConfigureAwait(false);
            }
            catch (DocumentClientException de)
            {
                // If the document collection does not exist, create a new partitioned collection
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    var partitionKey = new PartitionKeyDefinition();
                    partitionKey.Paths.Add(partitionId);
                    var collectionInfo = new DocumentCollection
                    {
                        Id = this.collectionName,
                        PartitionKey = partitionKey,
                        IndexingPolicy = new IndexingPolicy { Automatic = true, IndexingMode = indexingMode },
                        DefaultTimeToLive = defaultTtl
                    };
                    await this.documentDbClient.CreateDocumentCollectionAsync(UriFactory.CreateDatabaseUri(this.databaseName), collectionInfo).ConfigureAwait(false);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// The create database if not exists.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task CreateDatabaseIfNotExistsAsync()
        {
            // Check to verify a database with the id=FamilyDB does not exist
            try
            {
                await this.documentDbClient.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(this.databaseName)).ConfigureAwait(false);

                // Can we log this?
            }
            catch (DocumentClientException de)
            {
                // If the database does not exist, create a new database
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await this.documentDbClient.CreateDatabaseAsync(new Database { Id = this.databaseName }).ConfigureAwait(false);

                    // Can we Log ?
                    // "Created {0}", databaseName
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// The query document collection async.
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
        /// <param name="maxDegreeOfParallelism">
        /// The max degree of parallelism.
        /// </param>
        /// <param name="continuationToken">
        /// The continuation token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// Document DB query result
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Null argument
        /// </exception>
        private async Task<DocDbRestQueryResult> QueryDocumentCollectionAsync(
            string queryString,
            Dictionary<string, object> queryParams,
            string partitionId,
            bool enableCrossPartitionQuery,
            bool enableScanInQuery,
            int pageSize,
            int maxDegreeOfParallelism,
            string continuationToken)
        {
            if (string.IsNullOrWhiteSpace(queryString))
            {
                throw new ArgumentNullException(nameof(queryString));
            }

            var feedOptions = new FeedOptions
            {
                PartitionKey = new PartitionKey(partitionId),
                EnableCrossPartitionQuery = enableCrossPartitionQuery,
                EnableScanInQuery = enableScanInQuery,
                MaxItemCount = pageSize,
                RequestContinuation = continuationToken,
                MaxDegreeOfParallelism = maxDegreeOfParallelism
            };

            var query =
                this.documentDbClient.CreateDocumentQuery<JObject>(
                        UriFactory.CreateDocumentCollectionUri(this.databaseName, this.collectionName),
                        new SqlQuerySpec
                        {
                            QueryText = queryString,
                            Parameters = ToSqlQueryParamtereCollection(queryParams)
                        },
                        feedOptions)
                    .AsDocumentQuery();

            var response = await query.ExecuteNextAsync<JObject>().ConfigureAwait(false);
            var result = new DocDbRestQueryResult
            {
                ResultSet = new JArray(response.ToArray()),
                TotalResults = response.Count,
                ContinuationToken = query.HasMoreResults ? response.ResponseContinuation : null
            };

            return result;
        }

        /// <summary>
        /// Creates the client.
        /// </summary>
        /// <param name="docDbEndpoint">The document database endpoint.</param>
        /// <param name="connectionMode">The connection mode.</param>
        /// <param name="protocol">The protocol.</param>
        /// <returns>The document database client.</returns>
        private IDocumentClient CreateClient(string docDbEndpoint, ConnectionMode connectionMode, Protocol protocol)
        {
            var documentClient = new DocumentClient(
                new Uri(docDbEndpoint),
                this.key,
                new ConnectionPolicy
                {
                    ConnectionMode = connectionMode,
                    ConnectionProtocol = protocol
                });

            return documentClient;
        }

        /// <summary>
        /// Creates the client.
        /// </summary>
        /// <param name="docDbEndpoint">The document database endpoint.</param>
        /// <param name="connectionMode">The connection mode.</param>
        /// <param name="protocol">The protocol.</param>
        /// <param name="retryOptions">The retry options.</param>
        /// <returns>The document database client.</returns>
        private IDocumentClient CreateClient(string docDbEndpoint, ConnectionMode connectionMode, Protocol protocol, RetryOptions retryOptions)
        {
            var documentClient = new DocumentClient(
                new Uri(docDbEndpoint),
                this.key,
                new ConnectionPolicy
                {
                    ConnectionMode = connectionMode,
                    ConnectionProtocol = protocol,
                    RetryOptions = retryOptions
                });

            return documentClient;
        }
    }
}
