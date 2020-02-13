namespace Common.Data.AzureStorage.BlobS3
{
    public class BlobS3HandlerResult
    {
        public bool HasSucceeded { get; set; }
        public string S3Path { get; set; }
        public string Message { get; set; }
        public string Sha256CheckSum { get; set; }
    }
}
