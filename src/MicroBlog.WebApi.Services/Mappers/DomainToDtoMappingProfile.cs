namespace MicroBlog.WebApi.Services.Mappers;

public class DomainToDtoMappingProfile : Profile
{
    public DomainToDtoMappingProfile()
    {
        CreateMap<Tag, TagDto>();
    }
}