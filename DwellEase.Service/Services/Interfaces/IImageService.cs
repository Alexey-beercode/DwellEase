using DwellEase.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace DwellEase.Service.Services.Interfaces;

public interface IImageService
{
    BaseResponse<List<Image>> UploadImage(List<IFormFile> files);
}
