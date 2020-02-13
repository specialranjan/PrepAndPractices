using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data.AzureStorage.Blob
{
    using System.Threading.Tasks;

    /// <summary>
    /// The BLOB lease manager.
    /// </summary>
    public class BlobLeaseManager
    {
        /// <summary>
        /// The lease container name.
        /// </summary>
        private const string LeaseContainerName = "leases-blobleasemanager";
        private readonly string leaseIdentifier;
        private readonly IBlobRepository blobRepository;

        public BlobLeaseManager(IBlobRepository blobRepository, string leaseId)
        {
            //Requires.NotNull(blobRepository, nameof(blobRepository));
            //Requires.NotNullOrWhiteSpace(leaseId, nameof(leaseId));

            this.blobRepository = blobRepository;
            this.leaseIdentifier = leaseId;

            this.blobRepository.CreateIfNotExistsAsync(LeaseContainerName).Wait();
        }
        public BlobLeaseClient AcquireLease() => this.AcquireLeaseAsync().Result;
        public async Task<BlobLeaseClient> AcquireLeaseAsync()
        {
            var blobLeaseInformation =
                await this.blobRepository.AcquireLeaseAsync(LeaseContainerName, this.leaseIdentifier).ConfigureAwait(false);

            return new BlobLeaseClient(blobLeaseInformation.LeaseBlob, blobLeaseInformation.LeaseIdentifier);
        }
    }
}
