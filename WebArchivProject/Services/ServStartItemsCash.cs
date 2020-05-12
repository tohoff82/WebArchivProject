using Microsoft.Extensions.Caching.Memory;

using System;
using System.Collections.Generic;

using WebArchivProject.Contracts;
using WebArchivProject.Models.DTO;

namespace WebArchivProject.Services
{
    class ServStartItemsCash : IServStartItemsCash
    {
        private readonly IMemoryCache _cache;
        private readonly IServUserSession _userSession;

        private string KeyId => string
            .Format("StartItems_{0}", _userSession.User.Id);

        public ServStartItemsCash(
            IMemoryCache cache,
            IServUserSession userSession)
        {
            _cache = cache;
            _userSession = userSession;
        }

        public DtoStartItem StartItem => GetStartItem();

        /// <summary>
        /// Инициализируем кеш
        /// </summary>
        public void InitStartItemCash()
        {
            UpdateStartItem(EmptyStartItem);
        }

        /// <summary>
        /// Обновление кеша
        /// </summary>
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

        /// <summary>
        /// Получение объекта из кеша
        /// </summary>
        private DtoStartItem GetStartItem()
        {
            object obj = _cache.Get(KeyId);
            return obj as DtoStartItem;
        }

        /// <summary>
        /// Создание пустого объекта кеша
        /// </summary>
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
