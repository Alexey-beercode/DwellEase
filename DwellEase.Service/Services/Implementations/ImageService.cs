using System.Net;
using DwellEase.Domain.Models;
using DwellEase.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DwellEase.Service.Services.Implementations;

public class ImageService : IImageService
{
    public BaseResponse<List<Image>> UploadImage(List<IFormFile> files)
    {
        foreach (var file in files)
        {
            if (file == null || file.Length == 0) return new BaseResponse<List<Image>>(){StatusCode = HttpStatusCode.BadRequest,Description = $"Файл пуст:{file.FileName}"};
        }
        
        using (var memoryStream = new MemoryStream())
        {
            var images = new List<Image>();
            foreach (var file in files)
            {
                file.CopyTo(memoryStream);
                var image = new Image
                {
                    Data = memoryStream.ToArray(),
                    ContentType = file.ContentType
                }; 
                images.Add(image);
            }
            
            return new BaseResponse<List<Image>>(){StatusCode = HttpStatusCode.OK,Data = images};
        }
    }
    
}