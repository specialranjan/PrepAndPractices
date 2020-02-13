namespace Common.Data.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    using Dapper;

    using Microsoft.Azure;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

    /// <summary>
    /// The SQL storage context.
    /// </summary>
    public class SqlStorageContext : IStorageContext, IDisposable
    {
        /// <summary>
        /// The max time out.
        /// </summary>
        private const int MaxTimeOut = 120;
        private const string Default = "default";
        private const string DefaultSQLConnection = "default sql connection";
        private const string DefaultSQLCommand = "default sql command";
        private const string AltSQL = "alt sql";

        /// <summary>
        /// The connection string.
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlStorageContext"/> class.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        public SqlStorageContext(string connectionString)
        {
            this.connectionString = ResolveKey(connectionString);
        }

        /// <summary>
        /// Execute Stored Procedure
        /// </summary>
        /// <param name="procedureName">procedure Name</param>
        /// <param name="parameters">parameters dictionary</param>
        /// <returns>Task of string</returns>
        public async Task<string> ExecuteStoredProcedureAsync(string procedureName, Dictionary<string, object> parameters)
            => await this.ExecuteStoredProcedureAsync(procedureName, parameters, -1).ConfigureAwait(false);

        /// <summary>
        /// Execute Stored Procedure
        /// </summary>
        /// <param name="procedureName">procedure Name</param>
        /// <param name="parameters">parameters dictionary</param>
        /// <param name="timeOutSecs">Timeout for Command in Seconds</param>
        /// <returns>Task of string</returns>
        public async Task<string> ExecuteStoredProcedureAsync(string procedureName, Dictionary<string, object> parameters, int timeOutSecs)
        {
            const int RetryCountMin = 3;
            const int RetryCountMax = 5;
            const int MinBackOffTimeMsecs = 100;
            const int MaxBackOffTimeMsecs = 30;
            RetryManager.SetDefault(
                new RetryManager(
                    new List<RetryStrategy>
                        {
                            new ExponentialBackoff(
                                name: Default,
                                retryCount: RetryCountMin,
                                minBackoff: TimeSpan.FromMilliseconds(MinBackOffTimeMsecs),
                                maxBackoff: TimeSpan.FromSeconds(MaxBackOffTimeMsecs),
                                deltaBackoff: TimeSpan.FromSeconds(1),
                                firstFastRetry: true),
                            new ExponentialBackoff(
                                name: DefaultSQLConnection,
                                retryCount: RetryCountMin,
                                minBackoff: TimeSpan.FromMilliseconds(MinBackOffTimeMsecs),
                                maxBackoff: TimeSpan.FromSeconds(MaxBackOffTimeMsecs),
                                deltaBackoff: TimeSpan.FromSeconds(1),
                                firstFastRetry: true),
                            new ExponentialBackoff(
                                name: DefaultSQLCommand,
                                retryCount: RetryCountMin,
                                minBackoff: TimeSpan.FromMilliseconds(MinBackOffTimeMsecs),
                                maxBackoff: TimeSpan.FromSeconds(MaxBackOffTimeMsecs),
                                deltaBackoff: TimeSpan.FromSeconds(1),
                                firstFastRetry: true),
                            new ExponentialBackoff(
                                name: AltSQL,
                                retryCount: RetryCountMax,
                                minBackoff: TimeSpan.FromMilliseconds(MinBackOffTimeMsecs),
                                maxBackoff: TimeSpan.FromSeconds(MaxBackOffTimeMsecs),
                                deltaBackoff: TimeSpan.FromSeconds(1),
                                firstFastRetry: true),
                        },
                    Default,
                    new Dictionary<string, string>
                        {
                            {
                                RetryManagerSqlExtensions
                                .DefaultStrategyConnectionTechnologyName,
                                DefaultSQLConnection
                            },
                            {
                                RetryManagerSqlExtensions.DefaultStrategyCommandTechnologyName,
                                DefaultSQLCommand
                            }
                        }),
                false);

            return await Task.Run(
                () =>
                {
                    var retryConnectionPolicy = RetryManager.Instance.GetDefaultSqlConnectionRetryPolicy();
                    var retryCommandPolicy = RetryManager.Instance.GetDefaultSqlCommandRetryPolicy();

                    using (
                        ReliableSqlConnection conn = new ReliableSqlConnection(
                            this.connectionString,
                            retryConnectionPolicy,
                            retryCommandPolicy))
                    {
                        SqlParameter outResult = new SqlParameter("@result", SqlDbType.NVarChar, -1)
                        {
                            Direction =
                                                             ParameterDirection
                                                             .Output
                        };
                        conn.Open();
                        var command = conn.CreateCommand();
                        command.CommandText = procedureName;
                        command.CommandType = CommandType.StoredProcedure;
                        if (timeOutSecs > 0)
                        {
                            command.CommandTimeout = (timeOutSecs > MaxTimeOut) ? MaxTimeOut : timeOutSecs;
                        }

                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }

                        command.Parameters.Add(outResult);
                        conn.ExecuteCommand(command);
                        return outResult.Value.ToString();
                    }
                }).ConfigureAwait(false);
        }

        /// <summary>
        /// The execute stored procedure.
        /// </summary>
        /// <param name="procedureName">
        /// The procedure name.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <typeparam name="T">Generic Type
        /// </typeparam>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IList<T>> ExecuteStoredProcedureAsync<T>(string procedureName, object parameters)
            => await this.ExecuteStoredProcedureAsync<T>(procedureName, parameters, -1).ConfigureAwait(false);

        /// <summary>
        /// The execute stored procedure.
        /// </summary>
        /// <param name="procedureName">
        /// The procedure name.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="timeOutSecs">
        /// The time out seconds.
        /// </param>
        /// <typeparam name="T">Generic type
        /// </typeparam>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IList<T>> ExecuteStoredProcedureAsync<T>(
            string procedureName,
            object parameters,
            int timeOutSecs)
        {
            const int RetryCountMin = 3;
            const int RetryCountMax = 5;
            const int MinBackOffTimeMsecs = 100;
            const int MaxBackOffTimeMsecs = 30;
            RetryManager.SetDefault(
                new RetryManager(
                    new List<RetryStrategy>
                        {
                            new ExponentialBackoff(
                                name: Default,
                                retryCount: RetryCountMin,
                                minBackoff: TimeSpan.FromMilliseconds(MinBackOffTimeMsecs),
                                maxBackoff: TimeSpan.FromSeconds(MaxBackOffTimeMsecs),
                                deltaBackoff: TimeSpan.FromSeconds(1),
                                firstFastRetry: true),
                            new ExponentialBackoff(
                                name: DefaultSQLConnection,
                                retryCount: RetryCountMin,
                                minBackoff: TimeSpan.FromMilliseconds(100),
                                maxBackoff: TimeSpan.FromSeconds(MaxBackOffTimeMsecs),
                                deltaBackoff: TimeSpan.FromSeconds(1),
                                firstFastRetry: true),
                            new ExponentialBackoff(
                                name: DefaultSQLCommand,
                                retryCount: RetryCountMin,
                                minBackoff: TimeSpan.FromMilliseconds(MinBackOffTimeMsecs),
                                maxBackoff: TimeSpan.FromSeconds(MaxBackOffTimeMsecs),
                                deltaBackoff: TimeSpan.FromSeconds(1),
                                firstFastRetry: true),
                            new ExponentialBackoff(
                                name: "alt sql",
                                retryCount: RetryCountMax,
                                minBackoff: TimeSpan.FromMilliseconds(MinBackOffTimeMsecs),
                                maxBackoff: TimeSpan.FromSeconds(MaxBackOffTimeMsecs),
                                deltaBackoff: TimeSpan.FromSeconds(1),
                                firstFastRetry: true),
                        },
                    Default,
                    new Dictionary<string, string>
                        {
                            {
                                RetryManagerSqlExtensions
                                .DefaultStrategyConnectionTechnologyName,
                                DefaultSQLConnection
                            },
                            {
                                RetryManagerSqlExtensions.DefaultStrategyCommandTechnologyName,
                                DefaultSQLCommand
                            }
                        }),
                false);

            var retryConnectionPolicy = RetryManager.Instance.GetDefaultSqlConnectionRetryPolicy();
            var retryCommandPolicy = RetryManager.Instance.GetDefaultSqlCommandRetryPolicy();

            List<T> records;
            using (
                ReliableSqlConnection conn = new ReliableSqlConnection(
                                                 this.connectionString,
                                                 retryConnectionPolicy,
                                                 retryCommandPolicy))
            {
                conn.Open();

                using (
                    var result =
                        await
                            conn.QueryMultipleAsync(procedureName, parameters, commandType: CommandType.StoredProcedure)
                                .ConfigureAwait(false))
                {
                    records = (await result.ReadAsync<T>().ConfigureAwait(false)).ToList();
                }
            }

            return records;
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="isDisposing">
        /// The is disposing.
        /// </param>
        protected virtual void Dispose(bool isDisposing)
        {
        }

        /// <summary>
        /// The resolve key.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string ResolveKey(string key)
        {
            if (string.IsNullOrEmpty(key) || key.Contains(";"))
            {
                return key;
            }

            return CloudConfigurationManager.GetSetting(key) ?? key;
        }
    }
}
