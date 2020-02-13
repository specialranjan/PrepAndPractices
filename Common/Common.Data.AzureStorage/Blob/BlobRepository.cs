namespace Common.Data.AzureStorage.Blob
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.RetryPolicies;

    using Polly;

    public class BlobRepository : IBlobRepository
    {
        private readonly CloudBlobClient _cloudBlobClient;
        public BlobRepository(string storageConnectionString)
        {
            //Validate.String(storageConnectionString, nameof(storageConnectionString));

            var cloudStorageAccount = CloudStorageAccount.Parse(storageConnectionString);
            var requestOptions = new BlobRequestOptions
            {
                RetryPolicy = new ExponentialRetry(TimeSpan.FromMilliseconds(200), 3),
                StoreBlobContentMD5 = true
            };

            this._cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            this._cloudBlobClient.DefaultRequestOptions = requestOptions;
        }
        public async Task<CloudBlobContainer> CreateIfNotExistsAsync(string blobContainerName)
        {
            //Validate.BlobContainerName(blobContainerName, nameof(blobContainerName));

            var cloudBlobContainer = this._cloudBlobClient.GetContainerReference(blobContainerName);
            await cloudBlobContainer.CreateIfNotExistsAsync().ConfigureAwait(false);
            var permissions = cloudBlobContainer.GetPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Off;
            cloudBlobContainer.SetPermissions(permissions);

            return cloudBlobContainer;
        }
        public async Task<CloudBlobContainer> CreateIfNotExistsAsync(string blobContainerName, BlobContainerPublicAccessType publicAccessType)
        {
            //Validate.BlobContainerName(blobContainerName, nameof(blobContainerName));

            var cloudBlobContainer = this._cloudBlobClient.GetContainerReference(blobContainerName);
            await cloudBlobContainer.CreateIfNotExistsAsync().ConfigureAwait(false);
            var permissions = cloudBlobContainer.GetPermissions();
            permissions.PublicAccess = publicAccessType;
            cloudBlobContainer.SetPermissions(permissions);

            return cloudBlobContainer;
        }
        public async Task DeleteBlobContainerAsync(string blobContainerName)
        {
            //Validate.BlobContainerName(blobContainerName, nameof(blobContainerName));

            // Get reference of cloud blob container, create one if it's not exists
            var cloudBlobContainer = this._cloudBlobClient.GetContainerReference(blobContainerName);
            await cloudBlobContainer.DeleteIfExistsAsync().ConfigureAwait(false);
        }
        public async Task<string> CreateBlockBlobAsync(string blobId, string contentType, Stream data, string blobContainerName)
        {
            //Validate.BlobName(blobId, nameof(blobId));
            //Validate.String(contentType, nameof(contentType));
            //Validate.Null(data, nameof(data));

            // Get reference of cloud blob block
            var cloudBlockBlob = this.GetBlockBlobReference(blobId, blobContainerName);
            cloudBlockBlob.Properties.ContentType = contentType;
            await cloudBlockBlob.UploadFromStreamAsync(data).ConfigureAwait(false);

            return cloudBlockBlob.Uri.ToString();
        }
        public async Task<string> CreateBlockBlobAsync(string blobId, string contentType, byte[] data, string blobContainerName)
        {
            //Validate.BlobName(blobId, nameof(blobId));
            //Validate.String(contentType, nameof(contentType));
            //Validate.Null(data, nameof(data));

            // Get reference of cloud blob block
            var cloudBlockBlob = this.GetBlockBlobReference(blobId, blobContainerName);
            cloudBlockBlob.Properties.ContentType = contentType;
            await cloudBlockBlob.UploadFromByteArrayAsync(data, 0, data.Length).ConfigureAwait(false);

            return cloudBlockBlob.Uri.ToString();
        }
        public async Task<string> CreateBlockBlobAsync(string blobId, string contentType, string data, string blobContainerName)
        {
            //Validate.BlobName(blobId, nameof(blobId));
            //Validate.String(contentType, nameof(contentType));
            //Validate.String(data, nameof(data));

            // Get reference of cloud blob block
            var cloudBlockBlob = this.GetBlockBlobReference(blobId, blobContainerName);
            cloudBlockBlob.Properties.ContentType = contentType;
            await cloudBlockBlob.UploadTextAsync(data).ConfigureAwait(false);

            return cloudBlockBlob.Uri.ToString();
        }
        public async Task<string> CreateBlockBlobAsync(string blobId, string filePath, string blobContainerName)
        {
            //Validate.BlobName(blobId, nameof(blobId));
            //Validate.String(filePath, "contentType");

            // Get reference of cloud blob block
            var cloudBlockBlob = this.GetBlockBlobReference(blobId, blobContainerName);
            await cloudBlockBlob.UploadFromFileAsync(filePath).ConfigureAwait(false);

            return cloudBlockBlob.Uri.ToString();
        }
        public CloudBlockBlob GetBlockBlobReference(string blobId, string blobContainerName)
        {
            //Validate.BlobName(blobId, nameof(blobId));

            // Get reference of cloud blob container
            var cloudBlobContainer = this._cloudBlobClient.GetContainerReference(blobContainerName);
            return cloudBlobContainer.GetBlockBlobReference(blobId);
        }
        public CloudBlobContainer GetBlobContainerReference(string blobContainerName)
        {
            //Validate.BlobContainerName(blobContainerName, nameof(blobContainerName));

            return this._cloudBlobClient.GetContainerReference(blobContainerName);
        }
        public async Task<Stream> GetBlockBlobDataAsStreamAsync(string blobId, string blobContainerName)
        {
            //Validate.BlobName(blobId, nameof(blobId));

            // Get reference of cloud blob block
            var blob = this.GetBlockBlobReference(blobId, blobContainerName);
            var stream = new MemoryStream();
            await blob.DownloadToStreamAsync(stream).ConfigureAwait(false);
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }
        public async Task<string> GetBlockBlobDataAsStringAsync(string blobId, string blobContainerName)
        {
            //Validate.BlobName(blobId, nameof(blobId));

            // Get reference of cloud blob block
            var blob = this.GetBlockBlobReference(blobId, blobContainerName);
            return await blob.DownloadTextAsync().ConfigureAwait(false);
        }
        public IEnumerable<IListBlobItem> ListBlobsInContainer(string blobContainerName)
        {
            //Validate.BlobContainerName(blobContainerName, nameof(blobContainerName));

            // Get reference of cloud blob container, create one if it's not exists
            var cloudBlobContainer = this._cloudBlobClient.GetContainerReference(blobContainerName);
            return cloudBlobContainer.ListBlobs(useFlatBlobListing: true);
        }
        public async Task DeleteBlobAsync(string blobId, string blobContainerName)
        {
            // Get reference of cloud blob block
            var blob = this.GetBlockBlobReference(blobId, blobContainerName);
            await blob.DeleteIfExistsAsync().ConfigureAwait(false);
        }
        public async Task PutBlockBlobAsync(string blobId, string blockId, Stream data, string blobContainerName)
        {
            //Validate.BlobName(blobId, nameof(blobId));
            //Validate.String(blockId, nameof(blockId));
            //Validate.Null(data, nameof(data));
            //Validate.Null(blobContainerName, nameof(blobContainerName));

            // Get reference of cloud blob block
            var cloudBlockBlob = this.GetBlockBlobReference(blobId, blobContainerName);
            await cloudBlockBlob.PutBlockAsync(blockId, data, null).ConfigureAwait(false);
        }
        public async Task PutBlockListAsync(string blobId, string[] blockIds, string blobContainerName, string contentMD5 = null)
        {
            //Validate.BlobName(blobId, nameof(blobId));
            //Validate.Null(blobContainerName, nameof(blobContainerName));

            // Get reference of cloud blob block
            var cloudBlockBlob = this.GetBlockBlobReference(blobId, blobContainerName);

            if (contentMD5 != null)
            {
                cloudBlockBlob.Properties.ContentMD5 = contentMD5;
            }

            await cloudBlockBlob.PutBlockListAsync(blockIds).ConfigureAwait(false);
        }
        public string GetBlobSasUri(string blobId, double expiryTimeInHours, SharedAccessBlobPermissions sharedAccessBlobPermissions, string blobContainerName)
        {
            //Validate.BlobContainerName(blobContainerName, nameof(blobContainerName));

            // Get reference of cloud blob block
            var blob = this.GetBlockBlobReference(blobId, blobContainerName);

            var sasConstraints =
                new SharedAccessBlobPolicy
                {
                    SharedAccessExpiryTime =
                            DateTimeOffset.UtcNow.AddHours(expiryTimeInHours),
                    Permissions = sharedAccessBlobPermissions
                };

            var sasBlobToken = blob.GetSharedAccessSignature(sasConstraints, null, null, SharedAccessProtocol.HttpsOnly, null);
            return blob.Uri + sasBlobToken;
        }
        public string GetBlobSasUri(string blobId, double expiryTimeInHours, SharedAccessBlobPermissions sharedAccessBlobPermissions, string blobContainerName, SharedAccessBlobHeaders headers)
        {
            //Validate.BlobContainerName(blobContainerName, nameof(blobContainerName));

            var blob = this.GetBlockBlobReference(blobId, blobContainerName);

            var sasConstraints =
                new SharedAccessBlobPolicy
                {
                    SharedAccessExpiryTime =
                        DateTimeOffset.UtcNow.AddHours(expiryTimeInHours),
                    Permissions = sharedAccessBlobPermissions
                };

            var sasBlobToken = blob.GetSharedAccessSignature(sasConstraints, headers, null, SharedAccessProtocol.HttpsOnly, null);
            return blob.Uri + sasBlobToken;
        }
        public async Task<Stream> GetBlockBlobUsingSasTokenAsync(string sasUri)
        {
            //Validate.String(sasUri, nameof(sasUri));

            var blob = GetBlockBlobReference(sasUri);
            var blobStream = new MemoryStream();
            await blob.DownloadToStreamAsync(blobStream).ConfigureAwait(false);
            blobStream.Seek(0x0, SeekOrigin.Begin);
            return blobStream;
        }
        public async Task<BlobLeaseInformation> AcquireLeaseAsync(string containerName, string leaseId)
        {
            //Validate.BlobContainerName(containerName, nameof(containerName));
            //Validate.BlobName(leaseId, nameof(leaseId));

            var blobLeaseInformation = new BlobLeaseInformation();
            blobLeaseInformation.LeaseBlob = this.GetBlockBlobReference(leaseId, containerName);

            if (!blobLeaseInformation.LeaseBlob.Exists())
            {
                await blobLeaseInformation.LeaseBlob.UploadTextAsync(leaseId).ConfigureAwait(false);
            }

            var result = await Policy
                        .Handle<StorageException>(ex => ex.RequestInformation.HttpStatusCode == 409)
                        .WaitAndRetryForeverAsync(count => TimeSpan.FromMilliseconds(200))
                        .ExecuteAndCaptureAsync(async () =>
                            await blobLeaseInformation.LeaseBlob.AcquireLeaseAsync(TimeSpan.FromSeconds(30), Guid.NewGuid().ToString()).ConfigureAwait(false)).ConfigureAwait(false);

            blobLeaseInformation.LeaseIdentifier = result.Result;

            return blobLeaseInformation;
        }
        public async Task ReleaseLeaseAsync(string containerName, string leaseId)
        {
            //Validate.BlobContainerName(containerName, nameof(containerName));
            //Validate.BlobName(leaseId, nameof(leaseId));

            var blobInstance = this.GetBlockBlobReference(leaseId, containerName);

            await blobInstance.ReleaseLeaseAsync(AccessCondition.GenerateLeaseCondition(leaseId)).ConfigureAwait(false);
        }
        private static CloudBlockBlob GetBlockBlobReference(string blobSasUriString)
        {
            //var blobSasUri = Validate.ValidateUri(blobSasUriString, nameof(blobSasUriString));
            var blobSasUri = new Uri(blobSasUriString);

            // Get reference of cloud blob container
            return new CloudBlockBlob(blobSasUri);
        }
    }
}
