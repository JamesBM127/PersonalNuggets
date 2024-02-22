namespace JbmAwsBucket.Model
{
    public class DownloadRequest : S3Request
    {
        public string DirectoryPath { get; set; }
    }
}
