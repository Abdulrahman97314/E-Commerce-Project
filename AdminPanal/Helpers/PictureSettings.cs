namespace AdminPanal.Helpers
{
    public class PictureSettings
    {
        public static string UploadFile(IFormFile formFile, string folderName)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", folderName);

            string fileName = $"{Guid.NewGuid()}_{formFile.FileName}";

            string filePath = Path.Combine(folderPath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                formFile.CopyTo(fileStream);
            }

            return Path.Combine("images\\products", fileName);
        }
        public static bool DeleteFile(string folderName, string fileName)
        {
            // Combine the folder path and the filename to get the full path
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\images", folderName);
            string filePath = Path.Combine(folderPath, fileName);

            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting file {filePath}: {ex.Message}");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
