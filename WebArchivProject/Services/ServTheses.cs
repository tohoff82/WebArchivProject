using AutoMapper;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WebArchivProject.Contracts;
using WebArchivProject.Extensions;
using WebArchivProject.Models;
using WebArchivProject.Models.ArchivDb;
using WebArchivProject.Models.DTO;

namespace WebArchivProject.Services
{
    class ServTheses : IServTheses
    {
        private readonly IMapper _mapper;
        private readonly IRepoTheses _repoTheses;
        private readonly IRepoAuthors _repoAuthors;
        private readonly IServUserSession _userSession;

        private SessionUser User
            => _userSession.User;

        public ServTheses(
            IMapper mapper,
            IRepoTheses repoTheses,
            IRepoAuthors repoAuthors,
            IServUserSession userSession)
        {
            _mapper = mapper;
            _repoTheses = repoTheses;
            _repoAuthors = repoAuthors;
            _userSession = userSession;
        }

        public async Task AddToDbAsync(DtoThesis dtoThesis)
        {
            string guid = Guid.NewGuid().ToString();

            var thesis = _mapper.Map<Thesis>(dtoThesis);
            thesis.AuthorExternalId = guid;
            thesis.OwnerId = User.Id;
            await _repoTheses.AddThesisAsync(thesis);

            var authors = _mapper.Map<List<Author>>(dtoThesis.Authors);
            await _repoAuthors.AddAuthorsRangeAsync(authors.With(guid));
        }
    }
}
