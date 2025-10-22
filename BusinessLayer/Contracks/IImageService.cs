using BusinessLayer.Dtos;
using Microsoft.AspNetCore.Http;

namespace BusinessLayer.Contracks
{
    public interface IImageService
    {
        Task<List<ImageDto>> UploadImagesAsync(IEnumerable<IFormFile> images);

        Task<bool> DeleteImagesAsync(IEnumerable<ImageDto> imagesDtos);

        Task<bool> DeleteImageAsync(ImageDto imageDto);

        Task<ImageDto> UploadImageAsync(IFormFile image);
    }
}
