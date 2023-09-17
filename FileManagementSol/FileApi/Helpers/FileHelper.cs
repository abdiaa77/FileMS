namespace FileApi.Helpers
{
    public class FileHelper
    {

        public bool ValidateFileExtension(IFormFile file)
        {
            string[] permittedExtensions = { ".png", ".jpg", ".jpeg" };

            if (file == null)
                return false;

            var ext = Path.GetExtension(file.FileName);
            if (String.IsNullOrEmpty(ext) || permittedExtensions.Contains(ext))
            {
                return true;
            }
            return false;
        }



        public string UploadFile(IFormFile? file, string imgPath)
        {
            var rootPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);


            var fullPath = Path.Combine(rootPath, imgPath);

            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }


            String uniqueFileName = Guid.NewGuid().ToString() + "_" + file!.FileName;
            string FilePath = Path.Combine(fullPath, uniqueFileName);

            using (var stream = new FileStream(FilePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return uniqueFileName;
        }




    }
}
