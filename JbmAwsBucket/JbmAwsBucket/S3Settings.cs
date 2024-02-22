using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using JbmAwsBucket.Interface;
using JbmAwsBucket.Model;

namespace JbmAwsBucket
{
    public abstract class S3Settings
    {
        /// <summary>
        /// Upload a file
        /// </summary>
        /// <param name="client">AWS Client</param>
        /// <param name="fileInfo">Information of the file that gonna be uploaded</param>
        /// <param name="bucketName">The target bucket name</param>
        public static async Task UploadAsync(AmazonS3Client client, FileInfo fileInfo, string bucketName)
        {
            TransferUtility transferUtility = new TransferUtility(client);
            Stream stream = await S3IOSettings.ConvertFileInfoToStreamAsync(fileInfo);
            await transferUtility.UploadAsync(stream, bucketName, fileInfo.Name);
        }

        /// <summary>
        /// Copy the file, and upload the copy to S3Bucket.
        /// Is used to upload some file that can't be upload because is used by some process. 
        /// The method create a copy of the file and upload the copy, then this copy is deleted.
        /// </summary>
        /// <param name="client">AWS Client</param>
        /// <param name="fileInfo">Information of the file that gonna be uploaded</param>
        /// <param name="bucketName">The target bucket name</param>
        public static async Task UploadAsync2(AmazonS3Client client, FileInfo fileInfo, string bucketName)
        {
            TransferUtility transferUtility = new TransferUtility(client);

            string sourcePath = Path.Combine(fileInfo.Directory.FullName, fileInfo.Name);
            string tempFile = "temp" + fileInfo.Name;
            string destinationPath = Path.Combine(fileInfo.Directory.FullName, tempFile);

            await S3IOSettings.CopyFileThatIsUsedByAnotherProcessAsync(sourcePath, destinationPath);

            FileInfo tempFileInfo = new FileInfo(destinationPath);

            Stream stream = await S3IOSettings.ConvertFileInfoToStreamAsync(tempFileInfo);
            await transferUtility.UploadAsync(stream, bucketName, fileInfo.Name);

            S3IOSettings.DeleteTempDatabase(destinationPath);
        }

        /// <summary>
        /// Upload all files from local directory.
        /// </summary>
        /// <param name="client">AWS Client</param>
        /// <param name="folderPath">The path where's the local directory located</param>
        /// <param name="bucketName">The target bucket name</param>
        public static async Task UploadDirectoryAsync(AmazonS3Client client, string folderPath, string bucketName)
        {
            TransferUtility transferUtility = new TransferUtility(client);
            await transferUtility.UploadDirectoryAsync(folderPath, bucketName);
        }

        /// <summary>
        /// This Download method are using TransferUtility.
        /// </summary>
        /// <param name="client">AWS Client</param>
        /// <param name="downloadRequest">The Download data request</param>
        public static async Task DownloadAsync(AmazonS3Client client, DownloadRequest downloadRequest)
        {
            TransferUtility transferUtility = new TransferUtility(client);

            string destinationFilePath = downloadRequest.DirectoryPath;
            bool directoryExists = S3IOSettings.CheckAndCreateDestinationDirectory(destinationFilePath);

            if (directoryExists)
            {
                destinationFilePath = Path.Combine(destinationFilePath, downloadRequest.Key);

                //await a(client, downloadRequest, destinationFilePath);

                await transferUtility.DownloadAsync(destinationFilePath, downloadRequest.BucketName, downloadRequest.Key);
            }
        }

        /// <summary>
        /// This Download method are using GetObjectRequest.
        /// </summary>
        /// <param name="client">AWS Client</param>
        /// <param name="downloadRequest">The Download data request</param>
        public static async Task DownloadAsync2(AmazonS3Client client, DownloadRequest downloadRequest)
        {
            GetObjectRequest objectRequest = new()
            {
                BucketName = downloadRequest.BucketName,
                Key = downloadRequest.Key
            };

            string destinationFilePath = downloadRequest.DirectoryPath;
            bool directoryExists = S3IOSettings.CheckAndCreateDestinationDirectory(destinationFilePath);

            if (directoryExists)
            {
                using (GetObjectResponse response = await client.GetObjectAsync(objectRequest))
                {
                    using (Stream responseStream = response.ResponseStream)
                    {
                        destinationFilePath = Path.Combine(destinationFilePath, objectRequest.Key);

                        using (FileStream fileStream = File.Create(destinationFilePath))
                        {
                            await responseStream.CopyToAsync(fileStream);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Retrieve the Amazon S3 Client data from somewhere.
        /// </summary>
        /// <param name="s3Config">The AWS secret data</param>
        /// <returns>Logged Amazon S3Client</returns>
        public static AmazonS3Client GetAmazonS3Client(IS3Config s3Config)
        {
            return new AmazonS3Client(s3Config.AwsAccessKeyId, s3Config.AwsSecretAccessKey, RegionEndpoint.GetBySystemName(s3Config.RegionEndpoint));
        }
    }
}
