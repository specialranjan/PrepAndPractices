namespace Common.Data.AzureStorage.Table
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Threading.Tasks;
    using Common.Data.AzureStorage.Utils;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.RetryPolicies;
    using Microsoft.WindowsAzure.Storage.Table;
    using Microsoft.WindowsAzure.Storage.Table.Queryable;

    /// <summary>
    /// Repository class for Windows Azure storage tables access
    /// </summary>
    public class TableRepository : ITableRepository
    {
        /// <summary>
        /// The table client
        /// </summary>
        private readonly CloudTableClient tableClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string pointing to the storage account (this can be local or hosted in Windows Azure)</param>
        public TableRepository(string connectionString)
        {
            Validate.String(connectionString, nameof(connectionString));

            var storageAccount = CloudStorageAccount.Parse(connectionString);

            var requestOptions = new TableRequestOptions
            {
                RetryPolicy = new ExponentialRetry(TimeSpan.FromMilliseconds(200), 3)
            };

            this.tableClient = storageAccount.CreateCloudTableClient();
            this.tableClient.DefaultRequestOptions = requestOptions;
        }

        /// <summary>
        /// Get a reference to Azure Table Storage if it's not exists it will creates new one
        /// </summary>
        /// <typeparam name="T">Table Entity</typeparam>
        /// <param name="tableName">The name of the Azure Table to create</param>
        /// <returns>Reference of Created or existing CloudTable</returns>
        public async Task<CloudTable> CreateIfNotExistsAsync<T>(string tableName) where T : ITableEntity
        {
            Validate.TableName(tableName, nameof(tableName));

            var table = this.tableClient.GetTableReference(tableName);

            // Get table reference, create it if it's not exists
            await table.CreateIfNotExistsAsync().ConfigureAwait(false);
            return table;
        }

        /// <summary>
        /// Get a reference to Azure Table Storage if it's not exists it will creates new one
        /// </summary>
        /// <typeparam name="T">Table Entity</typeparam>
        /// <param name="tableName">The name of the Azure Table to create</param>
        /// <returns>Reference of Created or existing CloudTable Sync</returns>
        public CloudTable CreateIfNotExists<T>(string tableName) where T : ITableEntity
        {
            Validate.TableName(tableName, nameof(tableName));

            var table = this.tableClient.GetTableReference(tableName);

            // Get table reference, create it if it's not exists
            table.CreateIfNotExists();
            return table;
        }

        /// <summary>
        /// Delete the Azure Table Storage
        /// </summary>
        /// <typeparam name="T">The Table Entity</typeparam>
        /// <param name="tableName">The name of the Azure Table to delete</param>
        /// <returns>[true] if the entity is deleted</returns>
        public async Task<bool> DeleteIfExistsAsync<T>(string tableName) where T : ITableEntity
        {
            Validate.TableName(tableName, nameof(tableName));

            var table = this.tableClient.GetTableReference(tableName);
            return await table.DeleteIfExistsAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Gets an entity from the table with the specified partition key and row Key
        /// </summary>
        /// <typeparam name="T">The Table Entity type</typeparam>
        /// <param name="partitionKey">The partition key of the entities to be returned.</param>
        /// <param name="rowKey">The row key of the entities to be returned.</param>
        /// <param name="tableName">The name of the Azure Table to query from</param>
        /// <returns>The retrieved entity</returns>
        public async Task<T> RetrieveAsync<T>(string partitionKey, string rowKey, string tableName) where T : ITableEntity
        {
            Validate.TablePropertyValue(partitionKey, nameof(partitionKey));
            Validate.TablePropertyValue(rowKey, nameof(rowKey));

            var retreive = TableOperation.Retrieve<T>(partitionKey, rowKey);

            // Get table reference
            var table = this.tableClient.GetTableReference(tableName);
            var res = await table.ExecuteAsync(retreive).ConfigureAwait(false);
            return (T)res.Result;
        }

        /// <summary>
        /// Gets all entities from the table with the specified partition key
        /// </summary>
        /// <typeparam name="T">ITableEntity type</typeparam>
        /// <param name="partitionKey">The partition key of the entities to be returned.</param>
        /// <param name="tableName">The name of the Azure Table to query from</param>
        /// <returns>The retrieved entities collection</returns>
        public IEnumerable<T> RetrieveMany<T>(string partitionKey, string tableName) where T : ITableEntity, new()
        {
            Validate.TablePropertyValue(partitionKey, nameof(partitionKey));

            return this.RetrieveMany<T>(pk => pk.PartitionKey == partitionKey, tableName);
        }

        /// <summary>
        /// Gets all entities from the table with the specified predicate expression
        /// </summary>
        /// <typeparam name="T">ITableEntity type</typeparam>
        /// <param name="predicate">the predicate expression to use for filtering</param>
        /// <param name="tableName">The name of the Azure Table to query from</param>
        /// <returns>The retrieved entities collection</returns>
        public IEnumerable<T> RetrieveMany<T>(Expression<Func<T, bool>> predicate, string tableName) where T : ITableEntity, new()
        {
            if (predicate == null)
            {
                return this.RetirieveTopRecords<T>(tableName);
            }

            // Get table reference
            var table = this.tableClient.GetTableReference(tableName);

            return table.CreateQuery<T>().Where(predicate);
        }

        /// <summary>
        /// The retrieve many async.
        /// </summary>
        /// <param name="predicate">
        /// The predicate.
        /// </param>
        /// <param name="tableName">
        /// The table name.
        /// </param>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <typeparam name="T">
        /// The Table Entity
        /// </typeparam>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// The query list.
        /// </returns>
        public IEnumerable<T> RetrieveMany<T>(Expression<Func<T, bool>> predicate, string tableName, int pageSize) where T : ITableEntity, new()
        {
            if (predicate == null)
            {
                return this.RetirieveTopRecords<T>(tableName);
            }

            // Get table reference
            var table = this.tableClient.GetTableReference(tableName);

            if (pageSize > 0)
            {
                return table.CreateQuery<T>().Where(predicate).Take(pageSize);
            }

            return table.CreateQuery<T>().Where(predicate);
        }

        /// <summary>
        /// Create an entity if it doesn't exist or merges the new values to the existing one
        /// </summary>
        /// <typeparam name="T">ITableEntity type</typeparam>
        /// <param name="entity">the entity to insert or merge</param>
        /// <param name="tableName">The name of the Azure Table to query from</param>
        /// <returns>The inserted or merged entity</returns>
        public async Task<T> InsertOrMergeAsync<T>(T entity, string tableName) where T : ITableEntity
        {
            Validate.Null(entity, nameof(entity));

            var ope = TableOperation.InsertOrMerge(entity);

            // Get table reference
            var table = this.tableClient.GetTableReference(tableName);
            var res = await table.ExecuteAsync(ope).ConfigureAwait(false);
            return (T)res.Result;
        }

        /// <summary>
        /// Create an entity if it doesn't exist or replace the existing one
        /// </summary>
        /// <typeparam name="T">ITableEntity type</typeparam>
        /// <param name="entity">The entity to insert or replace</param>
        /// <param name="tableName">The name of the Azure Table to query from</param>
        /// <returns>The inserted entity</returns>
        public async Task<T> InsertOrReplaceAsync<T>(T entity, string tableName) where T : ITableEntity
        {
            Validate.Null(entity, nameof(entity));

            var ope = TableOperation.InsertOrReplace(entity);

            // Get table reference
            var table = this.tableClient.GetTableReference(tableName);
            var res = await table.ExecuteAsync(ope).ConfigureAwait(false);
            return (T)res.Result;
        }

        /// <summary>
        /// Merge given entity into Azure Table Storage
        /// </summary>
        /// <typeparam name="T">ITableEntity type</typeparam>
        /// <param name="entity">The entity to merge</param>
        /// <param name="tableName">The name of the Azure Table to query from</param>
        /// <returns>The merged entity</returns>
        public async Task<T> MergeAsync<T>(T entity, string tableName) where T : ITableEntity
        {
            Validate.Null(entity, nameof(entity));

            var ope = TableOperation.Merge(entity);

            // Get table reference
            var table = this.tableClient.GetTableReference(tableName);
            var res = await table.ExecuteAsync(ope).ConfigureAwait(false);
            return (T)res.Result;
        }

        /// <summary>
        /// Create an entity
        /// </summary>
        /// <typeparam name="T">ITableEntity type</typeparam>
        /// <param name="entity">The entity to insert</param>
        /// <param name="tableName">The name of the Azure Table to query from</param>
        /// <returns>The inserted entity</returns>
        public async Task<T> InsertAsync<T>(T entity, string tableName) where T : ITableEntity
        {
            Validate.Null(entity, nameof(entity));

            var ope = TableOperation.Insert(entity);

            // Get table reference
            var table = this.tableClient.GetTableReference(tableName);
            var res = await table.ExecuteAsync(ope).ConfigureAwait(false);
            return (T)res.Result;
        }

        /// <summary>
        /// Create entities in batch
        /// </summary>
        /// <typeparam name="T">ITableEntity type</typeparam>
        /// <param name="entities">The entities to insert in batch</param>
        /// <param name="tableName">The name of the Azure Table to query from</param>
        /// <returns>The inserted entities</returns>
        public async Task<IEnumerable<T>> InsertBatchAsync<T>(IEnumerable<T> entities, string tableName) where T : ITableEntity
        {
            Validate.Null(entities, nameof(entities));

            var counter = 0;
            var result = new List<T>();
            var batchOperation = new TableBatchOperation();

            // Get table reference
            var table = this.tableClient.GetTableReference(tableName);
            foreach (var entity in entities)
            {
                batchOperation.InsertOrReplace(entity);
                counter++;

                // Batch operations are limited to 100 items
                // when we reach 100, we commit and clear the operation
                const int BatchSizeLimit = 100;
                if (counter == BatchSizeLimit)
                {
                    var added = await table.ExecuteBatchAsync(batchOperation).ConfigureAwait(false);
                    result.AddRange(added.Select(x => (T)x.Result));

                    batchOperation.Clear();
                    counter = 0;
                }
            }

            if (counter > 0)
            {
                var added = await table.ExecuteBatchAsync(batchOperation).ConfigureAwait(false);
                result.AddRange(added.Select(x => (T)x.Result));
            }

            return result;
        }

        /// <summary>
        /// Delete entity from azure table storage
        /// </summary>
        /// <typeparam name="T">ITableEntity type</typeparam>
        /// <param name="entity">The entity to delete</param>
        /// <param name="tableName">The name of the Azure Table to query from</param>
        /// <returns>The Task</returns>
        public async Task DeleteAsync<T>(T entity, string tableName) where T : ITableEntity
        {
            Validate.Null(entity, nameof(entity));

            TableOperation ope = TableOperation.Delete(entity);

            // Get table reference
            var table = this.tableClient.GetTableReference(tableName);
            await table.ExecuteAsync(ope).ConfigureAwait(false);
        }

        /// <summary>
        /// Delete entities from table given predicate expression from azure table storage
        /// </summary>
        /// <typeparam name="T">The Table Entity</typeparam>
        /// <param name="predicate">the predicate expression to use for filtering the entities to delete</param>
        /// <param name="tableName">The name of the Azure Table to query from</param>
        /// <returns>The deleted entities</returns>
        public async Task<IEnumerable<T>> DeleteBatchAsync<T>(Expression<Func<T, bool>> predicate, string tableName) where T : ITableEntity, new()
        {
            Validate.Null(predicate, nameof(predicate));

            var counter = 0;
            var result = new List<T>();
            var batchOperation = new TableBatchOperation();

            // Get entities to delete
            var entities = this.RetrieveMany<T>(predicate, tableName);

            // Get table reference, create it if it's not exists
            var table = await this.CreateIfNotExistsAsync<T>(tableName).ConfigureAwait(false);

            foreach (var entity in entities)
            {
                batchOperation.Delete(entity);
                counter++;

                // Batch operations are limited to 100 items
                // when we reach 100, we commit and clear the operation
                const int batchSizeLimit = 100;
                if (counter == batchSizeLimit)
                {
                    var deleted = await table.ExecuteBatchAsync(batchOperation).ConfigureAwait(false);
                    result.AddRange(deleted.Select(x => (T)x.Result));

                    batchOperation.Clear();
                    counter = 0;
                }
            }

            if (counter > 0)
            {
                var deleted = await table.ExecuteBatchAsync(batchOperation).ConfigureAwait(false);
                result.AddRange(deleted.Select(x => (T)x.Result));
            }

            return result;
        }

        /// <summary>
        /// Update an entity with leveraging optimistic concurrency
        /// </summary>
        /// <typeparam name="T">ITableEntity type</typeparam>
        /// <param name="entity">The entity to update</param>
        /// <param name="tableName">The name of the Azure Table to query from</param>
        /// <param name="forceReplace">The Force Replace</param>
        /// <param name="updateAction">Custom update action to be executed on entity</param>
        /// <param name="retry">number of retry in case of update fail for concurrency reason</param>
        /// <returns>The updated entity</returns>
        public async Task<T> Replace<T>(T entity, string tableName, bool forceReplace = false, Action<T> updateAction = null, int retry = 3) where T : ITableEntity
        {
            TableResult res = null;

            Validate.Null(entity, nameof(entity));

            if (object.Equals(entity, default(T)))
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (forceReplace)
            {
                entity.ETag = "*";
            }

            var ope = TableOperation.Replace(entity);

            // Get table reference
            var table = this.tableClient.GetTableReference(tableName);

            // Execute update to be done if any
            updateAction?.Invoke(entity);
            try
            {
                res = await table.ExecuteAsync(ope).ConfigureAwait(false);
            }
            catch (Microsoft.WindowsAzure.Storage.StorageException ex)
            {
                if (retry > 0 && ex.InnerException is System.Net.WebException && ((HttpWebResponse)((System.Net.WebException)ex.InnerException).Response).StatusCode == HttpStatusCode.PreconditionFailed)
                {
                    TableOperation retrieveOp = TableOperation.Retrieve<T>(entity.PartitionKey, entity.RowKey);
                    var updatedEntity = (T)(await table.ExecuteAsync(retrieveOp).ConfigureAwait(false)).Result;
                    return await this.Replace(updatedEntity, tableName, forceReplace, updateAction, --retry).ConfigureAwait(false);
                }
                else
                {
                    throw;
                }
            }

            return (T)res?.Result;
        }

        /// <summary>Retrieves the many.</summary>
        /// <typeparam name="T">ITableEntity type</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="continuationToken">The continuation token.</param>
        /// <returns>Returns Table query segment</returns>
        public async Task<TableQuerySegment<T>> RetrieveManyAsync<T>(Expression<Func<T, bool>> predicate, string tableName, TableContinuationToken continuationToken = null) where T : ITableEntity, new()
        {
            // Get table reference
            var table = this.tableClient.GetTableReference(tableName);
            TableQuery<T> tableQuery = predicate != null ? table.CreateQuery<T>().Where(predicate).AsTableQuery() : table.CreateQuery<T>();

            var tableQueryResult = await table.ExecuteQuerySegmentedAsync(tableQuery, continuationToken).ConfigureAwait(false);

            return tableQueryResult;
        }

        /// <summary>
        /// The retrieve top records.
        /// </summary>
        /// <param name="tableName">
        /// The table name.
        /// </param>
        /// <typeparam name="T">
        /// The table entity
        /// </typeparam>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        private IEnumerable<T> RetirieveTopRecords<T>(string tableName) where T : ITableEntity, new()
        {
            string filter = TableQuery.GenerateFilterConditionForDate(
                            "Timestamp",
                            QueryComparisons.GreaterThanOrEqual,
                            DateTimeOffset.UtcNow.AddDays(-1).Date);

            TableQuery<T> tableQuery = new TableQuery<T> { TakeCount = 1000 };
            tableQuery.Where(filter);
            var table = this.tableClient.GetTableReference(tableName);

            return table.ExecuteQuery(tableQuery).ToList();
        }
    }
}
