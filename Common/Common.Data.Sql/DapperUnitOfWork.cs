namespace Common.Data.Sql
{
    using System;
    using System.Data.Common;

    /// <summary>
    /// Concrete implementation of IDapperUnitOfWork
    /// </summary>
    public class DapperUnitOfWork : IDapperUnitOfWork
    {
        #region Fields

        private bool _disposed = false;
        private readonly Action<DapperUnitOfWork> _rolledBack;
        private readonly Action<DapperUnitOfWork> _committed;

        #endregion

        #region Constructors

        /// <summary>
        /// Point of entry
        /// </summary>
        /// <param name="transaction">The current transaction for which the UoW is applied   </param>
        /// <param name="rolledBack">Delegate that handles Roll backs</param>
        /// <param name="committed">Deletage that handles commits</param>
        public DapperUnitOfWork(DbTransaction transaction, Action<DapperUnitOfWork> rolledBack, Action<DapperUnitOfWork> committed)
        {
            Transaction = transaction;
            _rolledBack = rolledBack;
            _committed = committed;
        }

        #endregion

        #region IAdoNetUnitOfWork Members

        /// <summary>
        /// Sets and Gets the current transaction in question
        /// </summary>
        public DbTransaction Transaction { get; private set; }

        /// <summary>
        /// Handles the disposal of the UnitOfWork
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Submits the pending changes made during this UnitOfWork
        /// </summary>
        public void SaveChanges()
        {
            if (Transaction == null)
            {
                throw new InvalidOperationException("Cannot call save changes twice.");
            }

            Transaction.Commit();
            _committed(this);
            Transaction = null;
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
                if (Transaction == null)
                {
                    return;
                }

                Transaction.Rollback();
                Transaction.Dispose();
                _rolledBack(this);
                Transaction = null;

                _disposed = true;
            }
        }

        // Use C# destructor syntax for finalization code.
        ~DapperUnitOfWork()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }

        #endregion
    }
}
