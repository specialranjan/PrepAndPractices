namespace Common.Data.AzureStorage.Table.Entities
{
    using Microsoft.WindowsAzure.Storage.Table;
        
    public class TableEntityExtended : TableEntity
    {
        public int PartitionKeyExtended { get; set; }
    }
}
