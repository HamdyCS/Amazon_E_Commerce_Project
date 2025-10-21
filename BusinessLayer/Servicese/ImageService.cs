using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Options;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace BusinessLayer.Servicese
{
    public class ImageService : IImageService
    {
        private readonly ILogger<ImageService> _logger;
        private readonly CloudinaryOptions _cloudinaryOptions;
        private readonly List<string> _allowedExtensions = new() { ".jpg", ".png", ".jpeg" };

        public ImageService(ILogger<ImageService> logger, CloudinaryOptions cloudinaryOptions)
        {
            _logger = logger;
            _cloudinaryOptions = cloudinaryOptions;
        }

        private bool _CheckIfFileExtensionValid(string imageExt)
        {
            return _allowedExtensions.Contains(imageExt);
        }
        public async Task<List<ImageDto>> UploadImagesAsync(IEnumerable<IFormFile> images)
        {
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(images, nameof(images));
            try
            {
                //account
                var account = new Account(
                    _cloudinaryOptions.CloudName,
                    _cloudinaryOptions.ApiKey,
                    _cloudinaryOptions.ApiSecret);

                //cloudinary
                Cloudinary cloudinary = new Cloudinary(account);

                //imageUrls
                var imagesDtos = new List<ImageDto>();

                //loop for images
                foreach (var image in images)
                {
                    ParamaterException.CheckIfObjectIfNotNull(image, nameof(image));

                    //check if file extension is not valid
                    if (!_CheckIfFileExtensionValid(Path.GetExtension(image.FileName)))
                    {
                        _logger.LogError($"Invalid file extension for image {image.FileName}. Allowed extensions are: {string.Join(", ", _allowedExtensions)}");
                        throw new Exception($"Invalid file extension for image {image.FileName}. Allowed extensions are: {string.Join(", ", _allowedExtensions)}");
                    }

                    //open stream and auto dispose
                    using (Stream stream = image.OpenReadStream())
                    {
                        // Set upload parameters
                        var uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams()
                        {
                            File = new FileDescription(image.FileName, stream),// File to upload
                            PublicId = Guid.NewGuid().ToString(), // Unique Id for the image
                        };

                        // Upload the image
                        var uploadResult = await cloudinary.UploadAsync(uploadParams);
                        if (uploadResult.StatusCode != HttpStatusCode.OK)
                        {
                            _logger.LogError($"Failed to upload image {image.FileName} to Cloudinary. Status: {uploadResult.StatusCode}");
                            continue;
                        }

                        // Add the URL to the list
                        imagesDtos.Add(new ImageDto
                        {
                            Url = uploadResult.SecureUrl.ToString(),
                            PublicId = uploadResult.PublicId,
                        });
                    }
                }

                return imagesDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while uploading images. {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteImagesAsync(IEnumerable<ImageDto> imagesDtos)
        {
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(imagesDtos, nameof(imagesDtos));

            try
            {
                //account
                var account = new Account(
                    _cloudinaryOptions.CloudName,
                    _cloudinaryOptions.ApiKey,
                    _cloudinaryOptions.ApiSecret);

                //list of public ids
                string[] publicIds = imagesDtos.Select(img => img.PublicId).ToArray();

                //check if public ids is null or empty
                if (publicIds is null || !publicIds.Any())
                {
                    return false;
                }

                //cloudinary
                var cloudinary = new Cloudinary(account);

                // Delete images
                var deleteImagesResult = await cloudinary.DeleteResourcesAsync(publicIds);

                return deleteImagesResult.Deleted.Count > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting image. {ex.Message}");
                throw;
            }
        }

    }
}
