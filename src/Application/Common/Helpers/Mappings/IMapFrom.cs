namespace Application.Common.Helpers.Mappings
{
    using AutoMapper;

    public interface IMapFrom<T>
    {
        void Mapping(Profile profile);
    }
}