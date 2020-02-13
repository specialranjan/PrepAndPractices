namespace Common.Data.AzureStorage.FileShare
{
    using System;
    using System.Collections.Generic;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.File;
    using Microsoft.WindowsAzure.Storage.RetryPolicies;

    public class FileShareRepository : IFileShareRepository
    {
        /// <summary>
        /// The _cloud file client.
        /// </summary>
        private readonly CloudFileClient _cloudFileClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileShareRepository"/> class.
        /// </summary>
        /// <param name="storageConnectionString">Storage Connection string</param>
        public FileShareRepository(string storageConnectionString)
        {
            //Validate.String(storageConnectionString, nameof(storageConnectionString));

            var cloudStorageAccount = CloudStorageAccount.Parse(storageConnectionString);
            var requestOptions = new FileRequestOptions
            {
                RetryPolicy = new ExponentialRetry(TimeSpan.FromMilliseconds(200), 3)
            };

            this._cloudFileClient = cloudStorageAccount.CreateCloudFileClient();
            this._cloudFileClient.DefaultRequestOptions = requestOptions;
        }

        /// <summary>
        /// Get all the files from file share
        /// </summary>
        /// <param name="fileShareName">File share name</param>
        /// <param name="folderName">Folder name</param>
        /// <returns>Ienumerable list of files</returns>
        public IEnumerable<IListFileItem> GetFilesFromFileShare(string fileShareName, string folderName)
        {
            var fileShare = _cloudFileClient.GetShareReference(fileShareName);
            if (fileShare.Exists())
            {
                var rootDirectory = fileShare.GetRootDirectoryReference();
                if (rootDirectory.Exists())
                {
                    var customDirectory = rootDirectory.GetDirectoryReference(folderName);
                    if (customDirectory.Exists())
                    {
                        var files = customDirectory.ListFilesAndDirectories();
                        return files;
                    }
                }
            }
            return null;
        }
    }
}
