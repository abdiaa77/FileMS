using Dapper;
using FileWeb.DTO.Image;
using FileWeb.DTO.ViewModels;
using FileWeb.Services.Helpers;
using FileWeb.Services.Interfaces;

namespace FileWeb.Services
{
    public class FileService : IFileService
    {
        private readonly DbConnFactoryHelper _dbConnFactoryHelper;
        public FileService(DbConnFactoryHelper dbConnFactoryHelper)
        {
            _dbConnFactoryHelper = dbConnFactoryHelper;
        }
        
        public async Task<IEnumerable<ImageRes>> GetAllImages()
        {
            var query = """
                            SELECT id, imageName, uploadedAt FROM tbl_images;
                        """;

            var conn = _dbConnFactoryHelper.Create();

            var result = await conn.QueryAsync<ImageRes>(query);

            return result;
        }



        public async Task<ImageRes> GetImageById(int? id)
        {
            var query = """
                            SELECT id, imageName, uploadedAt FROM tbl_images WHERE id=@id;
                        """;

            var conn = _dbConnFactoryHelper.Create();

            var result = await conn.QueryFirstOrDefaultAsync<ImageRes>(query, new { id });

            return result;
        }



        public async Task<ReponseVM> UploadImage(IFormFile? file)
        {
            var imageName = string.Empty;

            if (file != null && ValidateFileExtension(file))
                imageName = UploadFile(file, "fileManagement");
                

            var query = """
                            INSERT INTO tbl_images (imageName) VALUES (@imageName);
                        """;

            var conn = _dbConnFactoryHelper.Create();

          
            var result = await conn.ExecuteAsync(query, new { imageName });
            if(result > 0)
                return new ReponseVM(true,200, "success", "Image uploaded successfully");

            return new ReponseVM(false, 500, "error", "Image upload failed");
        }




        #region File Upload

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

        #endregion




    }
}
