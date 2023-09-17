using FileWeb.DTO.Image;
using FileWeb.DTO.ViewModels;

namespace FileWeb.Services.Interfaces
{
    public interface IFileService
    {
        /// <summary>
        /// Gets All Images
        /// </summary>
        /// <returns>All Image as list of ImageRes objects </returns>
        Task<IEnumerable<ImageRes>> GetAllImages();


        /// <summary>
        /// Gets Image by Id
        /// </summary>
        /// <param name="id">the Image Id to filter</param>
        /// <returns>an Image as ImageRes object according to the supplied image id </returns>
        Task<ImageRes> GetImageById(int? id);


        /// <summary>
        /// Upload an Image
        /// </summary>
        /// <param name="file">the image to be uploaded</param>
        /// <returns>a dynamic object with succcess, statusCode & message</returns>
        Task<ReponseVM> UploadImage(IFormFile? file);
    }
}
