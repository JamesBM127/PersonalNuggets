using Amazon.S3;
using JbmAwsBucket.Interface;
using JbmAwsBucket.Model;

namespace JbmAwsBucket
{
    public class S3Manager : IS3Manager
    {
        private readonly IS3Config _s3Config;

        public S3Manager(IS3Config s3Config)
        {
            _s3Config = s3Config;
        }

        public async Task UploadAsync(UploadRequest uploadRequest)
        {
            try
            {
                using (AmazonS3Client client = S3Settings.GetAmazonS3Client(_s3Config))
                {
                    //await S3Settings.UploadAsync(client, uploadRequest.FileInfo, _s3Config.BucketName);
                    await S3Settings.UploadAsync2(client, uploadRequest.FileInfo, _s3Config.BucketName);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task UploadDirectoryAsync(UploadRequest uploadRequest, string folderPath)
        {
            try
            {
                using (AmazonS3Client client = S3Settings.GetAmazonS3Client(_s3Config))
                {
                    await S3Settings.UploadDirectoryAsync(client, folderPath, _s3Config.BucketName);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task DownloadAsync(DownloadRequest downloadRequest)
        {
            try
            {
                using (AmazonS3Client client = S3Settings.GetAmazonS3Client(_s3Config))
                {
                    await S3Settings.DownloadAsync(client, downloadRequest);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task DownloadAsync2(DownloadRequest downloadRequest)
        {
            try
            {
                using (AmazonS3Client client = S3Settings.GetAmazonS3Client(_s3Config))
                {
                    await S3Settings.DownloadAsync2(client, downloadRequest);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
