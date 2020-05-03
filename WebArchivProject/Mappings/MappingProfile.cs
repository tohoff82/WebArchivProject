using AutoMapper;

using WebArchivProject.Models;
using WebArchivProject.Models.ArchivDb;
using WebArchivProject.Models.DTO;

namespace WebArchivProject.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, SessionUser>();
            CreateMap<DtoFormRegisterUser, AppUser>();
        }
    }
}
