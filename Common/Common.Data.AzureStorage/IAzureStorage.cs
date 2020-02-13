using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;

namespace Common.Data.AzureStorage
{
    public interface IAzureStorage
    {
        CloudTable GetTable(string tableName);

        CloudBlob GetBlockBlob(StorageUri storageUri, string containerName, string blobWithFolders);

        CloudBlockBlob GetBlockBlob(string containerName, string blobWithFolders);

        CloudBlockBlob GetBlockBlob(string uriString);

        CloudQueue GetQueue(string queueName);

        string ConnectionString { get; }

        CloudStorageAccount StorageAccount { get; }

        /// <summary>
        /// Method to upload the byte array to the blob storage
        /// </summary>
        /// <param name="fileName">Name of the file to be uploaded</param>
        /// <param name="data">Content of the file to be uploaded</param>
        /// <returns>Return nothing</returns>
        Task UploadByteArrayAsync(string fileName, byte[] data);
    }
}
