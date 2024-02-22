namespace JbmAwsBucket.Model
{
    public class UploadRequest : S3Request
    {
        public string FileName { get; set; }
        public FileInfo FileInfo { get; set; }
    }
}
