namespace Common.Data.Sql
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    /// <summary>
    /// Concrete implementation of IConnectionFactory
    /// </summary>
    public class ConnectionFactory : IConnectionFactory, IDisposable
    {
        #region Fields

        private readonly ConnectionStringSettings _connectionStringSettings;
        private SqlConnection _connection;

        #endregion

        #region Contructors

        /// <summary>
        /// Point of entry
        /// </summary>
        /// <param name="connectionStringSettings">The connection string setting object</param>
        public ConnectionFactory(ConnectionStringSettings connectionStringSettings)
        {
            if (connectionStringSettings == null)
            {
                throw new ArgumentNullException(nameof(connectionStringSettings));
            }

            _connectionStringSettings = connectionStringSettings;
        }

        #endregion

        #region IConnectionFactory Members

        /// <summary>
        /// Initializes the database connection
        /// </summary>
        /// <returns></returns>
        public DbConnection Initialize()
        {
            if (_connection != null && (_connection.State == ConnectionState.Connecting || _connection.State == ConnectionState.Open))
            {
                return _connection;
            }

            _connection = new SqlConnection(_connectionStringSettings.ConnectionString);
            _connection.Open();

            return _connection;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this._connection != null)
            {
                this._connection.Dispose();
            }
        }

        #endregion
    }
}
