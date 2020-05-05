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

            CreateMap<DtoAuthor, Author>();

            CreateMap<DtoStartItem, DtoBook>();
            CreateMap<DtoFormBook, DtoBook>();
            CreateMap<DtoBook, Book>();

            CreateMap<DtoStartItem, DtoPost>();
            CreateMap<DtoFormPost, DtoPost>()
                .ForMember(x => x.PagesInterval, x => x.MapFrom(s => string
                .Format("{0} — {1}", s.PagesIntervalStart, s.PagesIntervalFinish)));
            CreateMap<DtoPost, Post>();

            CreateMap<DtoStartItem, DtoThesis>();
            CreateMap<DtoFormThesis, DtoThesis>()
                .ForMember(x => x.DatesInterval, x => x.MapFrom(s => string
                .Format("{0} — {1}", s.DatesIntervalStart, s.DatesIntervalFinish)))
                .ForMember(x => x.PagesInterval, x => x.MapFrom(s => string
                .Format("{0} — {1}", s.PagesIntervalStart, s.PagesIntervalFinish)));
            CreateMap<DtoThesis, Thesis>();
        }
    }
}
