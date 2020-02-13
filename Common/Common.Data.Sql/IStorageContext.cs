namespace Common.Data.Sql
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// The StorageContext interface.
    /// </summary>
    public interface IStorageContext
    {
        /// <summary>
        /// Execute Stored Procedure
        /// </summary>
        /// <param name="procedureName">procedure Name</param>
        /// <param name="parameters">parameters dictionary</param>
        /// <returns>Task of string</returns>
        Task<string> ExecuteStoredProcedureAsync(string procedureName, Dictionary<string, object> parameters);
    }
}
