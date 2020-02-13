using System;
using System.Data.Common;

namespace Common.Data.Sql
{
    public interface IDapperContext : IDisposable
    {
        /// <summary>
        /// The StartUnitOfWork
        /// </summary>
        /// <returns></returns>
        IDapperUnitOfWork StartUnitOfWork();

        /// <summary>
        /// Creates a command object and sets the CommandTimeout and the transaction.
        /// </summary>
        /// <returns>A command object</returns>
        DbCommand InitializeCommand();

        /// <summary>
        /// Gets the DbConnection
        /// </summary>
        DbConnection DbConnection { get; }
    }
}
