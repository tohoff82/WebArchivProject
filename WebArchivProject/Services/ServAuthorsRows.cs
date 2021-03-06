﻿using Microsoft.Extensions.Caching.Memory;

using System;
using System.Collections.Generic;

using WebArchivProject.Contracts;
using WebArchivProject.Models.DTO;

namespace WebArchivProject.Services
{
    class ServAuthorsRows : IServAuthorsRows
    {
        private readonly IMemoryCache _cache;
        private readonly IServUserSession _sessionUser;

        private string KeyId => string
            .Format("Rows_{0}", _sessionUser.User.Id);

        private SortedDictionary<byte, DtoAuthor> NewRows
            => new SortedDictionary<byte, DtoAuthor>();

        public SortedDictionary<byte, DtoAuthor> AuthorsRows
            => GetAuthorsRows();

        public ServAuthorsRows(
            IMemoryCache cache,
            IServUserSession sessionUser)
        {
            _cache = cache;
            _sessionUser = sessionUser;
        }

        /// <summary>
        /// Инициализация кеша
        /// </summary>
        public void InitAuthorsRowsCash()
        {
            var rows = NewRows;
            rows.Add(1, EmptyRow);
            UpdateRowCash(rows);
        }

        /// <summary>
        /// Обработчик добавления строки автора
        /// </summary>
        public void HandleAddRow(List<DtoAuthor> authors)
        {
            var rows = NewRows;

            FillAuthorsRows(rows, authors);
            rows.Add(Convert.ToByte(authors.Count + 1), EmptyRow);

            UpdateRowCash(rows);
        }

        /// <summary>
        /// Обработчик обнавления строки автора
        /// </summary>
        public void HandleUpdateRow(List<DtoAuthor> authors)
        {
            var rows = NewRows;

            if (authors.Count > 0)
            {
                FillAuthorsRows(rows, authors);
            }
            else rows.Add(1, EmptyRow);

            UpdateRowCash(rows);
        }

        /// <summary>
        /// Заполнение строк авторов данными
        /// </summary>
        private void FillAuthorsRows(SortedDictionary<byte, DtoAuthor> rows, List<DtoAuthor> authors)
        {
            for (int i = 1; i <= authors.Count; i++)
            {
                rows.Add(Convert.ToByte(i), authors[i - 1]);
            }
        }

        /// <summary>
        /// Получение объектов строк из кеша
        /// </summary>
        private SortedDictionary<byte, DtoAuthor> GetAuthorsRows()
        {
            object obj = _cache.Get(KeyId);
            return obj as SortedDictionary<byte, DtoAuthor>;
        }

        /// <summary>
        /// Обновление кеша объектов строк
        /// </summary>
        private void UpdateRowCash(SortedDictionary<byte, DtoAuthor> rows)
        {
            _cache.Remove(KeyId);

            _cache.Set(KeyId, rows, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds
                (
                    value: _sessionUser.User.Expirate - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                )
            });
        }

        /// <summary>
        /// Получение пустого объекта строки
        /// </summary>
        private DtoAuthor EmptyRow
            => new DtoAuthor
            {
                NameUa = string.Empty,
                NameRu = string.Empty,
                NameEn = string.Empty,
                IsEmptyObj = true
            };
    }
}
