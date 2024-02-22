namespace JbmAwsBucket
{
    public abstract class S3IOSettings
    {
        public static async Task<Stream> ConvertFileInfoToStreamAsync(FileInfo fileInfo)
        {
            using (FileStream fileStream = fileInfo.OpenRead())
            {
                byte[] temporaryBytes = new byte[fileStream.Length];

                Stream stream = new MemoryStream(temporaryBytes);
                await fileStream.CopyToAsync(stream);
                await stream.FlushAsync();
                stream.Position = 0;

                return stream;
            }
        }

        public static async Task CopyFileThatIsUsedByAnotherProcessAsync(string sourcePath, string destinationPath)
        {
            using (FileStream sourceStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (FileStream destinationStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    await sourceStream.CopyToAsync(destinationStream);
                }
            }
        }

        public static bool CheckAndCreateDestinationDirectory(string destinationFilePath)
        {
            bool directoryExists = Directory.Exists(destinationFilePath);

            if (!directoryExists)
            {
                Directory.CreateDirectory(destinationFilePath);
                directoryExists = Directory.Exists(destinationFilePath);
            }

            return directoryExists;
        }

        public static void DeleteTempDatabase(string dbPath)
        {
            try
            {
                if (!File.Exists(dbPath))
                    throw new FileNotFoundException("AVISAR O JAMESON- Cópia temporária do banco não criada");

                File.Delete(dbPath);
            }
            catch (Exception ex)
            {
                throw new Exception("AVISAR O JAMESON- Erro ao excluir cópia do banco:" + ex.Message);
            }
        }
    }
}
