namespace Common.Data.Sql
{
    using System;

    public interface IDapperUnitOfWork : IDisposable
    {
        /// <summary>
        /// Submits the pending changes made during this UnitOfWork
        /// </summary>
        void SaveChanges();
    }
}
