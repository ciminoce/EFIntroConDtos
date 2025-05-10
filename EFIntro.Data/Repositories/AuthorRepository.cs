﻿using EFIntro.Data.Interfaces;
using EFIntro.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFIntro.Data.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryContext _context = null!;

        public AuthorRepository(LibraryContext context)
        {
            _context = context;
        }
        public List<Author> GetAll(string sortedBy = "LastName")
        {
            IQueryable<Author> query = _context.Authors.AsNoTracking();


            //MODERM FORM--> MAS BANANA
            return sortedBy switch
            {
                "LastName" => query.OrderBy(a => a.LastName)
                                        .ThenBy(a => a.FirstName)
                                        .ToList(),
                "FirstName" => query.OrderBy(a => a.FirstName)
                                    .ThenBy(a => a.LastName)
                                    .ToList(),
                _ => query.OrderBy(a => a.Id).ToList(),
            };
        }

        public Author? GetById(int authorId)
        {
            return _context.Authors.AsNoTracking()
                    .FirstOrDefault(a => a.Id == authorId);
        }
        public bool Exist(string firstName, string lastName, int? excludeId = null)
        {
            return excludeId.HasValue
                ? _context.Authors.Any(a => a.FirstName == firstName &&
                    a.LastName == lastName && a.Id != excludeId)
                : _context.Authors.Any(a => a.FirstName == firstName &&
                    a.LastName == lastName);


        }

        public void Add(Author author)
        {
            _context.Authors.Add(author);

        }

        public void Delete(int authorId)
        {
            //var authorInDb = GetById(authorId);
            //var authorInDb=_context.Authors.Find(authorId);
            //if (authorInDb != null)
            //{
            //    _context.Authors.Remove(authorInDb);

            //}
            var authorInDb = GetById(authorId);
            if (authorInDb != null)
            {
                _context.Entry(authorInDb).State = EntityState.Deleted;
            }
        }
        public void Update(Author author)
        {
            var authorInDb = GetById(author.Id);
            if (authorInDb != null)
            {
                authorInDb.FirstName = author.FirstName;
                authorInDb.LastName = author.LastName;
                _context.Entry(authorInDb).State = EntityState.Modified;
            }
        }

        public bool HasDependencies(int authorId)
        {
            return _context.Books.Any(b => b.AuthorId == authorId);

        }

        public void LoadBooks(Author author)
        {
            _context.Entry(author).Collection(a => a.Books!).Load();

        }

        public List<Author> GetAllWithBooks()
        {
            return _context.Authors.Include(a => a.Books).ToList();
        }


        public Author? GetByName(string firstName, string lastName)
        {
            return _context.Authors.FirstOrDefault(a => a.LastName == lastName
                    && a.FirstName == firstName);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
