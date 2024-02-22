namespace JbmAwsBucket.Interface
{
    public interface IS3Config
    {
        public string AwsAccessKeyId { get; set; }
        public string AwsSecretAccessKey { get; set; }
        public string BucketName { get; set; }
        public string BucketUrl { get; set; }
        public string RegionEndpoint { get; set; }
    }
}
