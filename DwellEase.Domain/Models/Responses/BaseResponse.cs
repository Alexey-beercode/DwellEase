using System.Net;

namespace DwellEase.Domain.Models.Responses;

public class BaseResponse<T> 
{
    public string? Description { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public T? Data { get; set; }
    
}