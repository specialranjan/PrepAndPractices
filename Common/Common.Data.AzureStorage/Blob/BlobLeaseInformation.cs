namespace Common.Data.AzureStorage.Blob
{
    using Microsoft.WindowsAzure.Storage.Blob;

    public class BlobLeaseInformation
    {
        public CloudBlockBlob LeaseBlob { get; set; }
        public string LeaseIdentifier { get; set; }
    }
}
