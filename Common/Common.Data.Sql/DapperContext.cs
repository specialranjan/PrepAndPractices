using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;

namespace Common.Data.Sql
{
    /// <summary>
    /// The Dapper Context Class
    /// </summary>
    public class DapperContext : IDapperContext
    {
        #region Fields

        private bool _disposed;
        private readonly DbConnection _connection;
        private DbCommand _command;
        private readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();
        private readonly LinkedList<DapperUnitOfWork> _uows = new LinkedList<DapperUnitOfWork>();

        #endregion

        #region Constructor

        /// <summary>
        /// The Constructor
        /// </summary>
        /// <param name="connectionFactory"></param>
        public DapperContext(IConnectionFactory connectionFactory)
        {
            if (connectionFactory == null)
            {
                throw new ArgumentNullException(nameof(connectionFactory));
            }

            // Here we initialize the connection with the database
            _connection = connectionFactory.Initialize();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Begins a transaction and starts a Unit of Work
        /// </summary>
        /// <returns></returns>
        public IDapperUnitOfWork StartUnitOfWork()
        {
            var transaction = _connection.BeginTransaction();

            var unitOfWork = new DapperUnitOfWork(transaction, RemoveTransaction, RemoveTransaction);

            _rwLock.EnterWriteLock();
            _uows.AddLast(unitOfWork);
            _rwLock.ExitWriteLock();

            return unitOfWork;
        }

        /// <summary>
        /// Creates a command object and sets the CommandTimeout and the transaction.
        /// </summary>
        /// <returns>A command object</returns>
        public DbCommand InitializeCommand()
        {
            _command = _connection.CreateCommand();
            const int CommandTimeoutTime = 1800;
            _command.CommandTimeout = CommandTimeoutTime;
            _rwLock.EnterReadLock();
            if (_uows.Count > 0)
            {
                _command.Transaction = _uows.First.Value.Transaction;
            }

            _rwLock.ExitReadLock();
            return _command;
        }

        /// <summary>
        /// The DB Connection
        /// </summary>
        public DbConnection DbConnection
        {
            get
            {
                if (_connection == null)
                {
                    throw new ArgumentException("Connection has not been initialized");
                }

                return _connection;
            }
        }

        /// <summary>
        /// Remove Transaction
        /// </summary>
        /// <param name="unitOfWork"></param>
        private void RemoveTransaction(DapperUnitOfWork unitOfWork)
        {
            _rwLock.EnterWriteLock();
            _uows.Remove(unitOfWork);
            _rwLock.ExitWriteLock();
        }

        /// <summary>
        /// The Dispose MEthod
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed objects
                }

                // Dispose unmanaged objects
                _rwLock.Dispose();

                if (_connection != null)
                {
                    _connection.Dispose();
                }

                if (_command != null)
                {
                    _command.Dispose();
                }

                _disposed = true;
            }
        }

        // Use C# destructor syntax for finalization code.
        ~DapperContext()
        {
            Dispose(false);
        }

        #endregion
    }
}
