using System.Net.Mime;

namespace DwellEase.Domain.Models;

public class Image
{
    public byte[] Data { get; set; }
    public string ContentType { get; set; }
}