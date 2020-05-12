using AutoMapper;

using WebArchivProject.Extensions;
using WebArchivProject.Models;
using WebArchivProject.Models.ArchivDb;
using WebArchivProject.Models.DTO;

namespace WebArchivProject.Mappings
{
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Настройка маппинга объектов
        /// </summary>
        public MappingProfile()
        {
            CreateMap<AppUser, SessionUser>();
            CreateMap<AppUser, DtoAppUserView>();
            CreateMap<DtoFormRegisterUser, AppUser>();
            CreateMap<DtoFormRegisterUser, DtoFormLoginUser>();

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

            CreateMap<Book, DtoSearchresultBook>()
                .ForMember(x => x.IssuerLine, x => x.MapFrom(s
                    => s.City.ToIssuerLine(s.Issuer)));
            CreateMap<Post, DtoSearchresultPost>()
                .ForMember(x => x.PagesCount, x => x.MapFrom(s => s.PagesInterval.ToCount()));
            CreateMap<Thesis, DtoSearchresultThesis>()
                .ForMember(x => x.PagesCount, x => x.MapFrom(s => s.PagesInterval.ToCount()))
                .ForMember(x => x.Location, x => x.MapFrom(s => s.Country.ToLocate(s.City)));
        }
    }
}
