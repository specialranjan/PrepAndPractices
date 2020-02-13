namespace Common.Data.AzureStorage.BlobS3
{
    using Amazon.S3;
    using Common.Data.AzureStorage.Blob;

    /// <summary>
    /// Holds the details to copy BLOB to S3.
    /// </summary>
    public class BlobS3Request
    {
        /// <summary>
        /// Gets or sets the BLOB repository.
        /// </summary>
        /// <value>
        /// The BLOB repository.
        /// </value>
        public IBlobRepository BlobRepository { get; set; }

        /// <summary>
        /// Gets or sets the source BLOB container.
        /// </summary>
        /// <value>
        /// The source BLOB container.
        /// </value>
        public string SourceBlobContainer { get; set; }

        /// <summary>
        /// Gets or sets the source BLOB.
        /// </summary>
        /// <value>
        /// The source BLOB.
        /// </value>
        public string SourceBlob { get; set; }

        /// <summary>
        /// Gets or sets the S3 client.
        /// </summary>
        /// <value>
        /// The S3 client.
        /// </value>
        public AmazonS3Client S3Client { get; set; }

        /// <summary>
        /// Gets or sets the target S3 bucket.
        /// </summary>
        /// <value>
        /// The target S3 bucket.
        /// </value>
        public string TargetS3Bucket { get; set; }

        /// <summary>
        /// Gets or sets the target S3 file.
        /// </summary>
        /// <value>
        /// The target S3 file.
        /// </value>
        public string TargetS3File { get; set; }
    }
}
