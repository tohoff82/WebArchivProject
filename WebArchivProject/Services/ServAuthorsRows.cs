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

        private int KeyId => _sessionUser.User.Id;

        public SortedDictionary<byte, DtoAuthor> AuthorsRows
            => GetAuthorsRows();

        public ServAuthorsRows(
            IMemoryCache cache,
            IServUserSession sessionUser)
        {
            _cache = cache;
            _sessionUser = sessionUser;
        }

        public void InitAuthorsRowsCash()
        {
            var rows = new SortedDictionary<byte, DtoAuthor>()
            {
                [1] = EmptyRow
            };

            UpdateRowCash(rows);
        }

        public void HandleAddRow(List<DtoAuthor> authors)
        {
            var rows = new SortedDictionary<byte, DtoAuthor>();

            FillAuthorsRows(rows, authors);
            rows.Add(Convert.ToByte(authors.Count + 1), EmptyRow);

            UpdateRowCash(rows);
        }

        public void HandleUpdateRow(List<DtoAuthor> authors)
        {
            var rows = new SortedDictionary<byte, DtoAuthor>();

            if (authors.Count > 0)
            {
                FillAuthorsRows(rows, authors);
            }
            else rows.Add(1, EmptyRow);
            UpdateRowCash(rows);
        }

        private void FillAuthorsRows(SortedDictionary<byte, DtoAuthor> rows, List<DtoAuthor> authors)
        {
            for (int i = 1; i <= authors.Count; i++)
            {
                rows.Add(Convert.ToByte(i), authors[i - 1]);
            }
        }

        private SortedDictionary<byte, DtoAuthor> GetAuthorsRows()
        {
            object obj = _cache.Get(KeyId);
            return obj as SortedDictionary<byte, DtoAuthor>;
        }

        private void UpdateRowCash(SortedDictionary<byte, DtoAuthor> rows)
        {
            _cache.Remove(KeyId);

            _cache.Set(KeyId, rows, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(7)
            });
        }

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
