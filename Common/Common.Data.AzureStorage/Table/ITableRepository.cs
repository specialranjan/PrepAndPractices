namespace Common.Data.AzureStorage.Table
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// The Table Repository Interface
    /// </summary>
    public interface ITableRepository
    {
        /// <summary>
        /// Get a reference to Azure Table Storage if it's not exists it will creates new one
        /// </summary>
        /// <typeparam name="T">Table Entity</typeparam>
        /// <param name="tableName">The name of the Azure Table to create, if the parameter is empty it will take the Name of T object</param>
        /// <returns>Reference of Created or existing CloudTable</returns>
        Task<CloudTable> CreateIfNotExistsAsync<T>(string tableName) where T : ITableEntity;

        /// <summary>
        /// Get a reference to Azure Table Storage if it's not exists it will creates new one
        /// </summary>
        /// <typeparam name="T">Table Entity</typeparam>
        /// <param name="tableName">The name of the Azure Table to create, if the parameter is empty it will take the Name of T object</param>
        /// <returns>Reference of Created or existing CloudTable Sync</returns>
        CloudTable CreateIfNotExists<T>(string tableName) where T : ITableEntity;

        /// <summary>
        /// Delete the Azure Table Storage
        /// </summary>
        /// <typeparam name="T">The Table Entity</typeparam>
        /// <param name="tableName">The name of the Azure Table to delete, if the parameter is empty it will take the Name of T object</param>
        /// <returns>[true] if the entity is deleted</returns>
        Task<bool> DeleteIfExistsAsync<T>(string tableName) where T : ITableEntity;

        /// <summary>
        /// Gets an entity from the table with the specified partition key and row Key
        /// </summary>
        /// <typeparam name="T">ITableEntity type</typeparam>
        /// <param name="partitionKey">The partition key of the entities to be returned.</param>
        /// <param name="rowKey">The row key of the entities to be returned.</param>
        /// <param name="tableName">The name of the Azure Table to query from, if the parameter is empty it will take the Name of T object</param>
        /// <returns>The retrieved entity</returns>
        Task<T> RetrieveAsync<T>(string partitionKey, string rowKey, string tableName) where T : ITableEntity;

        /// <summary>
        /// Gets all entities from the table with the specified partition key
        /// </summary>
        /// <typeparam name="T">ITableEntity type</typeparam>
        /// <param name="partitionKey">The partition key of the entities to be returned.</param>
        /// <param name="tableName">The name of the Azure Table to query from, if the parameter is empty it will take the Name of T object</param>
        /// <returns>The retrieved entities collection</returns>
        IEnumerable<T> RetrieveMany<T>(string partitionKey, string tableName) where T : ITableEntity, new();

        /// <summary>
        /// Gets all entities from the table with the specified predicate expression
        /// </summary>
        /// <typeparam name="T">ITableEntity type</typeparam>
        /// <param name="predicate">the predicate expression to use for filtering</param>
        /// <param name="tableName">The name of the Azure Table to query from, if the parameter is empty it will take the Name of T object</param>
        /// <returns>The retrieved entities collection</returns>
        IEnumerable<T> RetrieveMany<T>(Expression<Func<T, bool>> predicate, string tableName) where T : ITableEntity, new();

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
        /// The page size
        /// </param>
        /// <typeparam name="T">
        /// The Table Entity
        /// </typeparam>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        IEnumerable<T> RetrieveMany<T>(Expression<Func<T, bool>> predicate, string tableName, int pageSize) where T : ITableEntity, new();

        /// <summary>
        /// Create an entity if it doesn't exist or merges the new values to the existing one
        /// </summary>
        /// <typeparam name="T">ITableEntity type</typeparam>
        /// <param name="entity">the entity to insert or merge</param>
        /// <param name="tableName">The name of the Azure Table to query from, if the parameter is empty it will take the Name of T object</param>
        /// <returns>The inserted or merged entity</returns>
        Task<T> InsertOrMergeAsync<T>(T entity, string tableName) where T : ITableEntity;

        /// <summary>
        /// Create an entity if it doesn't exist or replace the existing one
        /// </summary>
        /// <typeparam name="T">ITableEntity type</typeparam>
        /// <param name="entity">The entity to insert or replace</param>
        /// <param name="tableName">The name of the Azure Table to query from, if the parameter is empty it will take the Name of T object</param>
        /// <returns>The inserted entity</returns>
        Task<T> InsertOrReplaceAsync<T>(T entity, string tableName) where T : ITableEntity;

        /// <summary>
        /// Merge given entity into Azure Table Storage
        /// </summary>
        /// <typeparam name="T">ITableEntity type</typeparam>
        /// <param name="entity">The entity to merge</param>
        /// <param name="tableName">The name of the Azure Table to query from, if the parameter is empty it will take the Name of T object</param>
        /// <returns>The merged entity</returns>
        Task<T> MergeAsync<T>(T entity, string tableName) where T : ITableEntity;

        /// <summary>
        /// Create an entity
        /// </summary>
        /// <typeparam name="T">ITableEntity type</typeparam>
        /// <param name="entity">The entity to insert</param>
        /// <param name="tableName">The name of the Azure Table to query from, if the parameter is empty it will take the Name of T object</param>
        /// <returns>The inserted entity</returns>
        Task<T> InsertAsync<T>(T entity, string tableName) where T : ITableEntity;

        /// <summary>
        /// Create entities in batch
        /// </summary>
        /// <typeparam name="T">ITableEntity type</typeparam>
        /// <param name="entities">The entities to insert in batch</param>
        /// <param name="tableName">The name of the Azure Table to query from, if the parameter is empty it will take the Name of T object</param>
        /// <returns>The inserted entities</returns>
        Task<IEnumerable<T>> InsertBatchAsync<T>(IEnumerable<T> entities, string tableName) where T : ITableEntity;

        /// <summary>
        /// Delete entity from azure table storage
        /// </summary>
        /// <typeparam name="T">ITableEntity type</typeparam>
        /// <param name="entity">The entity to delete</param>
        /// <param name="tableName">The name of the Azure Table to query from, if the parameter is empty it will take the Name of T object</param>
        /// <returns>The Task</returns>
        Task DeleteAsync<T>(T entity, string tableName) where T : ITableEntity;

        /// <summary>
        /// Delete entities from table given predicate expression from azure table storage
        /// </summary>
        /// <typeparam name="T">ITable Entity</typeparam>
        /// <param name="predicate">the predicate expression to use for filtering the entities to delete</param>
        /// <param name="tableName">The name of the Azure Table to query from, if the parameter is empty it will take the Name of T object</param>
        /// <returns>The deleted entities</returns>
        Task<IEnumerable<T>> DeleteBatchAsync<T>(Expression<Func<T, bool>> predicate, string tableName) where T : ITableEntity, new();

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
        Task<T> Replace<T>(T entity, string tableName, bool forceReplace = false, Action<T> updateAction = null, int retry = 3) where T : ITableEntity;

        /// <summary>Retrieves the many.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="continuationToken">The continuation token.</param>
        /// <returns></returns>
        Task<TableQuerySegment<T>> RetrieveManyAsync<T>(Expression<Func<T, bool>> predicate, string tableName, TableContinuationToken continuationToken = null) where T : ITableEntity, new();
    }
}
