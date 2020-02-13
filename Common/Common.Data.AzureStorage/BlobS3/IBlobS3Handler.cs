namespace Common.Data.AzureStorage.BlobS3
{
    using System.Threading.Tasks;

    public interface IBlobS3Handler
    {
        Task<BlobS3HandlerResult> CopyFromBlobToS3Async();
    }
}
