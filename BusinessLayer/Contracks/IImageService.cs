using BusinessLayer.Dtos;
using Microsoft.AspNetCore.Http;

namespace BusinessLayer.Contracks
{
    public interface IImageService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="images"></param>
        /// <returns>return images urls</returns>
        Task<List<ImageDto>> UploadImagesAsync(IEnumerable<IFormFile> images);

        Task<bool> DeleteImagesAsync(IEnumerable<ImageDto> imagesDtos);
    }
}
