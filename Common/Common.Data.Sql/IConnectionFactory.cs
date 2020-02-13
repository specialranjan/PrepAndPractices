namespace Common.Data.Sql
{
    using System.Data.Common;

    /// <summary>
    /// Serves as database connection builder
    /// </summary>
    public interface IConnectionFactory
    {
        /// <summary>
        /// Initializes the database connection
        /// </summary>
        /// <returns></returns>
        DbConnection Initialize();
    }
}
