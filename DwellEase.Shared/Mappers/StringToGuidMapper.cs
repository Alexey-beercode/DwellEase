namespace DwellEase.Shared.Mappers;

public class StringToGuidMapper
{
    public Guid MapTo(string id)
    {
        if (!Guid.TryParse(id,out var guid))
        {
            throw new("Invalid id");
        }

        return guid;
    }
}