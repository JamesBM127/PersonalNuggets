using JbmAwsBucket.Model;

namespace JbmAwsBucket.Interface
{
    public interface IS3Manager
    {
        public Task UploadAsync(UploadRequest uploadRequest);
        public Task UploadDirectoryAsync(UploadRequest uploadRequest, string folderPath);
        public Task DownloadAsync(DownloadRequest downloadRequest);
        public Task DownloadAsync2(DownloadRequest downloadRequest);
    }
}
