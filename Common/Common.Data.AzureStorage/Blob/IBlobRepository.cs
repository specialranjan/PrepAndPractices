using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data.AzureStorage.Blob
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Threading.Tasks;

	using Microsoft.WindowsAzure.Storage.Blob;

	public interface IBlobRepository
	{
		Task<string> CreateBlockBlobAsync(string blobId, string contentType, Stream data, string blobContainerName);
		Task<string> CreateBlockBlobAsync(string blobId, string contentType, byte[] data, string blobContainerName);
		Task<string> CreateBlockBlobAsync(string blobId, string contentType, string data, string blobContainerName);
		Task<string> CreateBlockBlobAsync(string blobId, string filePath, string blobContainerName);
		Task<CloudBlobContainer> CreateIfNotExistsAsync(string blobContainerName);
		Task<CloudBlobContainer> CreateIfNotExistsAsync(string blobContainerName,BlobContainerPublicAccessType publicAccessType);
		Task DeleteBlobAsync(string blobId, string blobContainerName);
		Task DeleteBlobContainerAsync(string blobContainerName);
		string GetBlobSasUri(string blobId,double expiryTimeInHours,SharedAccessBlobPermissions sharedAccessBlobPermissions,string blobContainerName);
		string GetBlobSasUri(string blobId,double expiryTimeInHours,SharedAccessBlobPermissions sharedAccessBlobPermissions,string blobContainerName,SharedAccessBlobHeaders headers);
		Task<Stream> GetBlockBlobDataAsStreamAsync(string blobId, string blobContainerName);
		Task<string> GetBlockBlobDataAsStringAsync(string blobId, string blobContainerName);
		CloudBlockBlob GetBlockBlobReference(string blobId, string blobContainerName);
		CloudBlobContainer GetBlobContainerReference(string blobContainerName);
		IEnumerable<IListBlobItem> ListBlobsInContainer(string blobContainerName);
		Task PutBlockBlobAsync(string blobId, string blockId, Stream data, string blobContainerName);
		Task PutBlockListAsync(string blobId, string[] blockIds, string blobContainerName, string contentMD5 = null);
		Task<Stream> GetBlockBlobUsingSasTokenAsync(string sasUri);
		Task<BlobLeaseInformation> AcquireLeaseAsync(string containerName, string leaseId);
		Task ReleaseLeaseAsync(string containerName, string leaseId);
	}
}
