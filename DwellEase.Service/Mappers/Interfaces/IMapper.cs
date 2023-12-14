namespace DwellEase.Service.Mappers.Interfaces;

public interface IMapper<T, K>
{
    T MapTo(K source);
}