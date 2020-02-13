namespace Common.Data.AzureStorage.FileShare
{
	using System.Collections.Generic;
	using Microsoft.WindowsAzure.Storage.File;

	public interface IFileShareRepository
	{
		IEnumerable<IListFileItem> GetFilesFromFileShare(string fileShareName, string folderName);
	}
}
