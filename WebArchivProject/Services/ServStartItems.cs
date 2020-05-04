using Microsoft.Extensions.Caching.Memory;

using System;
using System.Collections.Generic;

using WebArchivProject.Contracts;
using WebArchivProject.Models.DTO;

namespace WebArchivProject.Services
{
    class ServStartItems : IServStartItems
    {
        private readonly IMemoryCache _cache;
        private readonly IServUserSession _userSession;

        private string KeyId => string
            .Format("StartItems_{0}", _userSession.User.Id);

        public ServStartItems(
            IMemoryCache cache,
            IServUserSession userSession)
        {
            _cache = cache;
            _userSession = userSession;
        }

        public DtoStartItem StartItem => GetStartItem();

        public void InitStartItemCash()
        {
            UpdateStartItem(EmptyStartItem);
        }

        public void UpdateStartItem(DtoStartItem dtoStartItem)
        {
            _cache.Remove(KeyId);

            _cache.Set(KeyId, dtoStartItem, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds
                (
                    value: _userSession.User.Expirate - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                )
            });
        }

        private DtoStartItem GetStartItem()
        {
            object obj = _cache.Get(KeyId);
            return obj as DtoStartItem;
        }

        private DtoStartItem EmptyStartItem
            => new DtoStartItem
            {
                Name = string.Empty,
                Year = string.Empty,
                ItemType = string.Empty,
                Authors = new List<DtoAuthor>
                {
                    new DtoAuthor
                    {
                        NameUa = string.Empty,
                        NameRu = string.Empty,
                        NameEn = string.Empty,
                        IsEmptyObj = true
                    }
                }
            };
    }
}
