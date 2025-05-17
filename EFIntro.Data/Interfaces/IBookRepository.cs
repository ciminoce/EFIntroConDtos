﻿using EFIntro.Entities;

namespace EFIntro.Data.Interfaces
{
    public interface IBookRepository
    {
        void Add(Book book);
        void Delete(int bookId);
        bool Exist(string bookTitle, int bookAuthorId, int? excludeId = null);
        List<Book> GetAll(string sortedBy = "Title");
        Book? GetById(int bookId, bool include = false, bool tracked = false);
        void Update(Book book);
        void SaveChanges();
    }
}