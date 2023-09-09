namespace DwellEase.Domain.Models;

public class Image
{
    public byte[] Data { get; set; } = null!;
    public string ContentType { get; set; } = null!;
}