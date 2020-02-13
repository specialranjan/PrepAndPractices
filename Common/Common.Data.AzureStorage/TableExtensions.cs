using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data.AzureStorage
{
    public static class TableExtensions
    {
        public static async Task<T> RetrieveEntitiesAsync<T>(this CloudTable table, string partitionKey, string rowKey) where T : ITableEntity
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            var retreive = TableOperation.Retrieve<T>(partitionKey, rowKey);
            var res = await table.ExecuteAsync(retreive).ConfigureAwait(false);
            var entity = (T)res.Result;
            return entity;
        }

        public static async Task<IEnumerable<T>> RetrieveEntitiesAsync<T>(this CloudTable table, string partitionKey) where T : ITableEntity, new()
        {
            return await table.QueryEntitiesAsync<T>(pk => pk.PartitionKey == partitionKey).ConfigureAwait(false);
        }

        public static async Task<IEnumerable<T>> QueryEntitiesAsync<T>(this CloudTable table, Expression<Func<T, bool>> predicate) where T : ITableEntity, new()
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            var query = table.CreateQuery<T>().Where(predicate);
            if (query == null)
            {
                return null;
            }

            return await Task.FromResult<IEnumerable<T>>(query.ToArray()).ConfigureAwait(false);
        }

        public static async Task InsertOrMergeAsync<T>(this CloudTable table, T entity) where T : ITableEntity
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            var ope = TableOperation.InsertOrMerge(entity);
            await table.ExecuteAsync(ope).ConfigureAwait(false);
        }

        public static async Task InsertOrReplaceAsync<T>(this CloudTable table, T entity) where T : ITableEntity
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            var ope = TableOperation.InsertOrReplace(entity);
            await table.ExecuteAsync(ope).ConfigureAwait(false);
        }

        public static async Task MergeAsync<T>(this CloudTable table, T entity) where T : ITableEntity
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            var ope = TableOperation.Merge(entity);
            await table.ExecuteAsync(ope).ConfigureAwait(false);
        }

        public static async Task InsertAsync<T>(this CloudTable table, T entity) where T : ITableEntity
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            var ope = TableOperation.Insert(entity);
            await table.ExecuteAsync(ope).ConfigureAwait(false);
        }
        public static async Task DeleteAsync<T>(this CloudTable table, T entity) where T : ITableEntity
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            var ope = TableOperation.Delete(entity);
            await table.ExecuteAsync(ope).ConfigureAwait(false);
        }
    }
}
