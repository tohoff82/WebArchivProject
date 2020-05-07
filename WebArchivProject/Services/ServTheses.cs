﻿using AutoMapper;

using Microsoft.Extensions.Caching.Memory;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebArchivProject.Contracts;
using WebArchivProject.Extensions;
using WebArchivProject.Models;
using WebArchivProject.Models.ArchivDb;
using WebArchivProject.Models.DTO;
using WebArchivProject.Models.VO;

namespace WebArchivProject.Services
{
    class ServTheses : IServTheses
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IRepoTheses _repoTheses;
        private readonly IRepoAuthors _repoAuthors;
        private readonly IServUserSession _userSession;

        private string KeyId => string
            .Format("Theses_{0}", _userSession.User.Id);

        private SessionUser User
            => _userSession.User;

        public ServTheses(
            IMapper mapper,
            IMemoryCache cache,
            IRepoTheses repoTheses,
            IRepoAuthors repoAuthors,
            IServUserSession userSession)
        {
            _mapper = mapper;
            _cache = cache;
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

            await UpdateThesesCash();
        }

        public async Task DeleteFromDbAsync(int thesisId)
        {
            var thesis = await _repoTheses.GetThesisByIdAsync(thesisId);
            var authors = await _repoAuthors.GetAuthorsByExtIdAsync(thesis.AuthorExternalId);

            await _repoTheses.DeleteThesisAsync(thesis);
            await _repoAuthors.DeleteAuthorsRangeAsync(authors);

            await UpdateThesesCash();
        }

        public Paginator<DtoSearchresultThesis> GetPaginationResult(int pageNumber, int pageSize)
        {
            if (GetThesesCash() == null) UpdateThesesCash().GetAwaiter().GetResult();

            return GetThesesPaginator(pageNumber, pageSize);
        }

        private Paginator<DtoSearchresultThesis> GetThesesPaginator(int pageNumber, int pageSize)
            => Paginator<DtoSearchresultThesis>.ToList(GetThesesCash(), pageNumber, pageSize);

        private async Task UpdateThesesCash()
        {
            var theses = new List<DtoSearchresultThesis>();
            foreach (var item in await _repoTheses.ToListAsync())
            {
                var authors = await _repoAuthors
                    .GetAuthorsByExtIdAsync(
                        item.AuthorExternalId);

                var thesis = _mapper.Map<DtoSearchresultThesis>(item);
                thesis.Authors = authors.Select(a => a.NameUa).ToList();
                theses.Add(thesis);
            }

            _cache.Remove(KeyId);

            _cache.Set(KeyId, theses, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds
                (
                    value: _userSession.User.Expirate - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                )
            });
        }

        private List<DtoSearchresultThesis> GetThesesCash()
        {
            object obj = _cache.Get(KeyId);
            return obj as List<DtoSearchresultThesis>;
        }
    }
}
