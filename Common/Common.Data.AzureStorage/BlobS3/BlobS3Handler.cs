namespace Common.Data.AzureStorage.BlobS3
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security.Cryptography;
    using System.Threading.Tasks;
    using Amazon.S3;
    using Amazon.S3.Model;
    using Common.Data.AzureStorage.Blob;
    using Microsoft.WindowsAzure.Storage.Blob;

    /// <summary>
    /// The BLOB S3 handler.
    /// </summary>
    public class BlobS3Handler : IBlobS3Handler
    {
        /// <summary>
        /// The container not exists.
        /// </summary>
        private const string ContainerNotExists = "BLOB Container doesn't exists.";

        /// <summary>
        /// The BLOB not exists.
        /// </summary>
        private const string BlobNotExists = "BLOB doesn't exists.";

        /// <summary>
        /// Part size to read from BLOB and upload to S3.
        /// </summary>
        private const long PartSize = 104857600; // 100 MB.

        /// <summary>
        /// The BLOB S3 request.
        /// </summary>
        private readonly BlobS3Request blobS3Request;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobS3Handler"/> class.
        /// </summary>
        /// <param name="blobS3Request">The BLOB S3 request.</param>
        public BlobS3Handler(BlobS3Request blobS3Request)
        {
            this.blobS3Request = blobS3Request;
        }

        /// <summary>
        /// Gets the BLOB repository.
        /// </summary>
        /// <value>
        /// The BLOB repository.
        /// </value>
        private IBlobRepository BlobRepository => this.blobS3Request.BlobRepository;

        /// <summary>
        /// Gets the source BLOB container.
        /// </summary>
        /// <value>
        /// The source BLOB container.
        /// </value>
        private string SourceBlobContainer => this.blobS3Request.SourceBlobContainer;

        /// <summary>
        /// Gets the source BLOB.
        /// </summary>
        /// <value>
        /// The source BLOB.
        /// </value>
        private string SourceBlob => this.blobS3Request.SourceBlob;

        /// <summary>
        /// Gets the Amazon S3 client.
        /// </summary>
        /// <value>
        /// The Amazon S3 client.
        /// </value>
        private AmazonS3Client S3Client => this.blobS3Request.S3Client;

        /// <summary>
        /// Gets the target S3 bucket.
        /// </summary>
        /// <value>
        /// The target S3 bucket.
        /// </value>
        private string TargetS3Bucket => this.blobS3Request.TargetS3Bucket;

        /// <summary>
        /// Gets the target S3 file.
        /// </summary>
        /// <value>
        /// The target S3 file.
        /// </value>
        private string TargetS3File => this.blobS3Request.TargetS3File;

        /// <summary>
        /// Copies from BLOB to S3.
        /// </summary>
        /// <returns>
        /// Returns the result of copy from BLOB to S3.
        /// </returns>
        public async Task<BlobS3HandlerResult> CopyFromBlobToS3Async()
        {
            var result = await this.ProcessBlobAndCopyToS3Async().ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Processes the BLOB an copy to s3.
        /// </summary>
        /// <returns>Returns the result of copy from BLOB to S3.</returns>
        private async Task<BlobS3HandlerResult> ProcessBlobAndCopyToS3Async()
        {
            var result = new BlobS3HandlerResult();

            var validationResult = await this.ValidateRequestAsync().ConfigureAwait(false);

            if (!validationResult.Item1)
            {
                return validationResult.Item2;
            }

            var blobToCopy = validationResult.Item3;
            await blobToCopy.FetchAttributesAsync().ConfigureAwait(false);

            var remainingBytes = blobToCopy.Properties.Length;
            long readPosition = 0; // To be used offset / position from where to start reading from BLOB.

            var initiateMultipartUploadRequest = new InitiateMultipartUploadRequest
            {
                BucketName = this.TargetS3Bucket,
                Key = this.TargetS3File
            };

            // Will use UploadId from this response.
            var initiateMultipartUploadResponse = this.S3Client.InitiateMultipartUpload(initiateMultipartUploadRequest);
            var uploadPartResponses = new List<UploadPartResponse>();

            try
            {
                var partCounter = 0; // To increment on each read of parts and use it as part number.
                var sha256 = new SHA256Managed();

                while (remainingBytes > 0)
                {
                    // Determine the size when final block reached as it might be less than Part size.
                    // Will be PartSize except final block.
                    var bytesToCopy = Math.Min(PartSize, remainingBytes);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        // To download part from BLOB.
                        await blobToCopy.DownloadRangeToStreamAsync(memoryStream, readPosition, bytesToCopy).ConfigureAwait(false);
                        memoryStream.Position = 0;
                        partCounter++;

                        var uploadRequest = new UploadPartRequest
                        {
                            BucketName = this.TargetS3Bucket,
                            Key = this.TargetS3File,
                            UploadId = initiateMultipartUploadResponse.UploadId,
                            PartNumber = partCounter,
                            PartSize = bytesToCopy,
                            InputStream = memoryStream
                        };

                        var uploadPartResponse = this.S3Client.UploadPart(uploadRequest);
                        uploadPartResponses.Add(uploadPartResponse);

                        remainingBytes -= bytesToCopy;
                        readPosition += bytesToCopy;

                        // $"Uploaded part with part number {partCounter}, size {bytesToCopy}bytes and remaining {remainingBytes}bytes to read.")

                        // Calculate the checksum value.
                        if (remainingBytes <= 0)
                        {
                            sha256.TransformFinalBlock(memoryStream.ToArray(), 0, (int)bytesToCopy);
                        }
                        else
                        {
                            var bytesToSend = memoryStream.ToArray();
                            sha256.TransformBlock(bytesToSend, 0, (int)bytesToCopy, bytesToSend, 0);
                        }
                    }
                }

                result.Sha256CheckSum = BitConverter.ToString(sha256.Hash).Replace("-", string.Empty);

                var completeMultipartUploadRequest = new CompleteMultipartUploadRequest
                {
                    BucketName = this.TargetS3Bucket,
                    Key = this.TargetS3File,
                    UploadId = initiateMultipartUploadResponse.UploadId
                };

                completeMultipartUploadRequest.AddPartETags(uploadPartResponses);

                var completeMultipartUploadResponse = await this.S3Client.CompleteMultipartUploadAsync(completeMultipartUploadRequest).ConfigureAwait(false);

                result.HasSucceeded = true;
                result.S3Path = completeMultipartUploadResponse.Location;
            }
            catch (Exception exception)
            {
                result.HasSucceeded = false;
                result.Message = exception.Message;

                var abortMultipartUploadRequest = new AbortMultipartUploadRequest
                {
                    BucketName = this.TargetS3Bucket,
                    Key = this.TargetS3File,
                    UploadId = initiateMultipartUploadResponse.UploadId
                };

                await this.S3Client.AbortMultipartUploadAsync(abortMultipartUploadRequest).ConfigureAwait(false);
            }

            return result;
        }

        /// <summary>
        /// Validates the request.
        /// </summary>
        /// <returns>The validation result.</returns>
        private async Task<Tuple<bool, BlobS3HandlerResult, CloudBlockBlob>> ValidateRequestAsync()
        {
            BlobS3HandlerResult result = null;

            if (!await this.BlobRepository.GetBlobContainerReference(this.SourceBlobContainer).ExistsAsync().ConfigureAwait(false))
            {
                result = new BlobS3HandlerResult
                {
                    HasSucceeded = false,
                    Message = ContainerNotExists
                };

                return new Tuple<bool, BlobS3HandlerResult, CloudBlockBlob>(false, result, null);
            }

            var blobToCopy = this.BlobRepository.GetBlockBlobReference(this.SourceBlob, this.SourceBlobContainer);

            if (await blobToCopy.ExistsAsync().ConfigureAwait(false))
            {
                return new Tuple<bool, BlobS3HandlerResult, CloudBlockBlob>(true, null, blobToCopy);
            }

            result = new BlobS3HandlerResult
            {
                HasSucceeded = false,
                Message = BlobNotExists
            };

            return new Tuple<bool, BlobS3HandlerResult, CloudBlockBlob>(false, result, null);
        }
    }
}
