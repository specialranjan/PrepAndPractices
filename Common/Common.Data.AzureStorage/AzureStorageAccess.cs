namespace Common.Data.AzureStorage
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.Queue;
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Contains the methods and properties to manage Azure storage
    /// </summary>
    public class AzureStorageAccess : IAzureStorage
    {
        /// <summary>
        /// 
        /// </summary>
        private CloudTableClient _tableClient;

        /// <summary>
        /// Reference to blob client
        /// </summary>
        private CloudBlobClient _blobClient;

        /// <summary>
        /// Reference to queue client
        /// </summary>
        private CloudQueueClient _queueClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureStorageAccess" /> class.
        /// </summary>
        /// <param name="connectionString">connection string</param>
        public AzureStorageAccess(string connectionString)
        {
            this.ConnectionString = connectionString;
            this.StorageAccount = CloudStorageAccount.Parse(ConnectionString);
        }

        /// <summary>
        /// Gets the Connection string
        /// </summary>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// Gets the Storage Account
        /// </summary>
        public CloudStorageAccount StorageAccount { get; private set; }

        #region -- Azure Tables

        /// <summary>
        /// Gets the Table client
        /// </summary>
        private CloudTableClient TableClient
        {
            get
            {
                if (_tableClient == null)
                {
                    _tableClient = StorageAccount.CreateCloudTableClient();
                }

                return _tableClient;
            }
        }

        /// <summary>
        /// Gets the Azure storage table name
        /// </summary>
        /// <param name="tableName">name of the storage table</param>
        /// <returns>returns the Cloud table.</returns>
        public CloudTable GetTable(string tableName)
        {
            var table = TableClient.GetTableReference(tableName);
            try
            {
                table.CreateIfNotExists();
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error while creatting the analytics history table: {0}", ex);
                return null;
            }

            return table;
        }
        #endregion

        #region -- Azure Blobs

        /// <summary>
        /// Gets the blob client
        /// </summary>
        private CloudBlobClient BlobClient
        {
            get
            {
                if (_blobClient == null)
                {
                    _blobClient = StorageAccount.CreateCloudBlobClient();
                }

                return _blobClient;
            }
        }

        /// <summary>
        /// Gets the Cloud block blob
        /// </summary>
        /// <param name="uriString">a valid uri for the blob</param>
        /// <returns>returns a reference to Cloud block blob</returns>
        public CloudBlockBlob GetBlockBlob(string uriString)
        {
            var blobUri = new Uri(uriString);
            return new CloudBlockBlob(blobUri);
        }

        /// <summary>
        /// Gets the Cloud block blob
        /// </summary>
        /// <param name="containerName">name of the container</param>
        /// <param name="blobWithFolders">name of the folders</param>
        /// <returns>returns a reference to Cloud block blob</returns>
        public CloudBlockBlob GetBlockBlob(string containerName, string blobWithFolders)
        {
            var container = this.BlobClient.GetContainerReference(containerName);
            container.CreateIfNotExists();
            var blob = container.GetBlockBlobReference(blobWithFolders);
            return blob;
        }

        /// <summary>
        /// Gets the Cloud blob
        /// </summary>
        /// <param name="storageUri">valid uri</param>
        /// <param name="containerName">name of the container</param>
        /// <param name="blobWithFolders">name of the folder</param>
        /// <returns>returns a reference to Cloud blob</returns>
        public CloudBlob GetBlockBlob(StorageUri storageUri, string containerName, string blobWithFolders)
        {
            if (storageUri == null)
            {
                throw new ArgumentNullException(nameof(storageUri));
            }

            containerName = $"{storageUri.GetUri(StorageLocation.Primary).AbsoluteUri}/{containerName}";
            return this.GetBlockBlob(containerName, blobWithFolders);
        }

        /// <summary>
        /// Method to upload the byte array to the blob storage
        /// </summary>
        /// <param name="fileName">Name of the file to be uploaded</param>
        /// <param name="data">Content of the file to be uploaded</param>
        /// <returns>Return nothing</returns>
        public async Task UploadFromByteArrayAsync(string fileName, byte[] data)
        {
            const string ContainerName = "archivestore";
            var container = this.BlobClient.GetContainerReference(ContainerName);
            container.CreateIfNotExists();
            var blockBlob = container.GetBlockBlobReference(fileName);
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            await blockBlob.UploadFromByteArrayAsync(data, 0, data.Length).ConfigureAwait(false);
        }

        #endregion

        #region -- Azure Queue

        /// <summary>
        /// Gets the Cloud Queue client
        /// </summary>
        private CloudQueueClient QueueClient
        {
            get
            {
                if (this._queueClient == null)
                {
                    this._queueClient = this.StorageAccount.CreateCloudQueueClient();
                }

                return this._queueClient;
            }
        }

        /// <summary>
        /// Gets the Cloud Queue
        /// </summary>
        /// <param name="queueName">name of the queue</param>
        /// <returns>returns a reference of Cloud queue</returns>
        public CloudQueue GetQueue(string queueName)
        {
            var queue = this.QueueClient.GetQueueReference(queueName);
            try
            {
                queue.CreateIfNotExists();
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error while creatting the pushDestination Queue: {0}", ex);
                return null;
            }

            return queue;
        }

        /// <summary>
        /// uploads the bytes (content) to the blob with the name as file name
        /// </summary>
        /// <param name="fileName">name of the blob</param>
        /// <param name="data">array of bytes</param>
        /// <returns>The task.</returns>
        public async Task UploadByteArrayAsync(string fileName, byte[] data)
        {
            await this.UploadFromByteArrayAsync(fileName, data).ConfigureAwait(false);
        }

        #endregion
    }
}
