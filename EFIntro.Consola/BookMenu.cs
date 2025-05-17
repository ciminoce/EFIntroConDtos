using EFIntro.Service.DTOs.Author;
using EFIntro.Service.DTOs.Book;
using EFIntro.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EFIntro.Consola
{
    public class BookMenu
    {
        private readonly IServiceProvider _serviceProvider = null!;

        public BookMenu(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void MostrarMenu()
        {
            
            do
            {
                Console.Clear();
                Console.WriteLine("📕 BOOKS");
                Console.WriteLine("1 - List of Books");
                Console.WriteLine("2 - Add New Book");
                Console.WriteLine("3 - Delete a Book");
                Console.WriteLine("4 - Edit a Book");
                Console.WriteLine("5 - Books Group By Author");
                Console.WriteLine("r - Return");
                Console.Write("Enter an option:");
                var option = Console.ReadLine();
                using (var scope=_serviceProvider.CreateScope())
                {
                    var bookService = scope.ServiceProvider
                        .GetRequiredService<IBookService>();
                    var authorService = scope.ServiceProvider
                        .GetRequiredService<IAuthorService>();
                    switch (option)
                    {
                        case "1":
                            BooksList(bookService);
                            break;
                        case "2":
                            AddBooks(bookService, authorService);
                            break;
                        case "3":
                            DeleteBooks(bookService);
                            break;
                        case "4":
                            EditBooks(bookService, authorService);
                            break;
                        case "5":
                            BooksGroupByAuthor(bookService);
                            break;
                        case "r":
                            return;
                        default:
                            break;
                    }

                }
            } while (true);
        }

        private void EditBooks(IBookService bookService, IAuthorService authorService)
        {
            Console.Clear();
            Console.WriteLine("Editing Books");
            Console.WriteLine("list Of Books to Edit");

            var books = bookService.GetAll("Id");
            foreach (var item in books)
            {
                Console.WriteLine($"{item.Id.ToString().PadLeft(4,' ')}-{item.Title}");
            }

            Console.Write("Enter BookID to edit (0 to Escape):");
            int bookId = int.Parse(Console.ReadLine()!);
            if (bookId < 0)
            {
                Console.WriteLine("Invalid BookID... ");
                Console.ReadLine();
                return;
            }
            if (bookId == 0) return;

            var bookInDb = bookService.GetById(bookId);
            if (bookInDb == null)
            {
                Console.WriteLine("Book does not exist...");
                Console.ReadLine();
                return;
            }
            BookUpdateDto bookUpdateDto = new BookUpdateDto()
            {
                Id = bookInDb.Id,
                Title = bookInDb.Title,
                Pages = bookInDb.Pages,
                PublishDate = bookInDb.PublishDate,
                AuthorId = bookInDb.AuthorId
            };


            Console.WriteLine($"Current Book Title: {bookInDb.Title}");
            Console.Write("Enter New Title (or ENTER to Keep the same):");
            var newTitle = Console.ReadLine();
            if (!string.IsNullOrEmpty(newTitle))
            {
                bookUpdateDto.Title = newTitle;
            }
            Console.WriteLine($"Current Book Pages Count: {bookInDb.Pages}");
            Console.Write("Enter Book Pages Count (or ENTER to Keep the same):");
            var newPages = Console.ReadLine();
            if (!string.IsNullOrEmpty(newPages))
            {
                if (!int.TryParse(newPages, out int bookPages) || bookPages <= 0)
                {
                    Console.WriteLine("You enter an invalid page count");
                    Console.ReadLine();
                    return;
                }
                bookUpdateDto.Pages = bookPages;
            }

            Console.WriteLine($"Current Book Publish Date: {bookInDb.PublishDate}");
            Console.Write("Enter New Book Publish Date (or ENTER to Keep the same):");
            var newDate = Console.ReadLine();
            if (!string.IsNullOrEmpty(newDate))
            {
                if (!DateOnly.TryParse(newDate, out DateOnly publishDate) ||
                    publishDate > DateOnly.FromDateTime(DateTime.Today))
                {
                    Console.WriteLine("Invalid Publish Date...");
                    Console.ReadLine();
                    return;
                }
                bookUpdateDto.PublishDate = publishDate;
            }
            AuthorDto? authorDto = authorService.GetById(bookInDb.AuthorId);
            Console.WriteLine($"Current Book Author:{authorDto!.FirstName} {authorDto.LastName}");
            Console.WriteLine("Available Authors");
            var authors = authorService.GetAll("Id");
            foreach (var author in authors)
            {
                Console.WriteLine($"{author.Id.ToString().PadLeft(4,' ')}-{author.FirstName} {author.LastName}");
            }
            Console.Write("Enter AuthorID (or ENTER to Keep the same or 0 New Author):");
            var newAuthorId = Console.ReadLine();
            if (!string.IsNullOrEmpty(newAuthorId))
            {
                if (!int.TryParse(newAuthorId, out int authorId) || authorId < 0)
                {
                    Console.WriteLine("You enter an invalid AuthorID");
                    Console.ReadLine();
                    return;
                }
                if (authorId > 0)
                {
                    var existingAuthor = authorService.GetById(authorId);
                    if (existingAuthor is null)
                    {
                        Console.WriteLine("AuthorID not found");
                        Console.ReadLine();
                        return;
                    }
                    bookUpdateDto.AuthorId = authorId;

                }
                else
                {
                    //Entering new author
                    Console.WriteLine("Adding a New Author");
                    Console.Write("Enter First Name:");
                    var firstName = Console.ReadLine();
                    Console.Write("Enter Last Name:");
                    var lastName = Console.ReadLine();
                    var existingAuthor = authorService.GetByName(firstName ?? string.Empty,
                        lastName ?? string.Empty);

                    if (existingAuthor is not null)
                    {
                        Console.WriteLine("You have entered an existing author!!!");
                        Console.WriteLine("Assigning his AuthorID");

                        bookUpdateDto.AuthorId = existingAuthor.Id;
                    }
                    else
                    {
                        AuthorCreateDto newAuthor = new AuthorCreateDto
                        {
                            FirstName = firstName ?? string.Empty,
                            LastName = lastName ?? string.Empty,
                        };
                        AuthorDto? authorCreated = null;
                        if (authorService.Create(newAuthor, out authorCreated, out var authorErrors))
                        {
                            Console.WriteLine("New Author Added Successfully");
                            bookUpdateDto.AuthorId = authorCreated!.Id;
                        }
                        else
                        {
                            Console.WriteLine("Errors while trying to add a new author");
                            foreach (var item in authorErrors)
                            {
                                Console.WriteLine(item);
                            }
                        }


                    }
                }

            }

            var originalBook = bookService.GetById(bookId);

            Console.Write($"Are you sure to edit \"{originalBook!.Title}\"? (y/n):");
            var confirm = Console.ReadKey().KeyChar;
            try
            {
                if (confirm.ToString().ToLower() == "y")
                {
                    if (bookService.Update(bookUpdateDto, out var errors))
                    {
                        Console.WriteLine("Book successfully edited");

                    }
                    else
                    {
                        Console.WriteLine("Errors while trying to update a book");
                        errors.ForEach(error => Console.WriteLine(error));
                    }
                }
                else
                {
                    Console.WriteLine("Operation cancelled by user");
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();


            }

        private void DeleteBooks(IBookService bookService)
        {
            Console.Clear();
            Console.WriteLine("Deleting Books");
            Console.WriteLine("List of Books to Delete");
            var books = bookService.GetAll("Id");
            foreach (var book in books)
            {
                Console.WriteLine($"{book.Id.ToString().PadLeft(4,' ')} - {book.Title}");
            }

            Console.Write("Select BookID to Delete (0 to Escape):");
            if (!int.TryParse(Console.ReadLine(), out int bookId) || bookId < 0)
            {
                Console.WriteLine("Invalid BookID...");
                Console.ReadLine();
                return;
            }
            if (bookId == 0) return;
            Console.WriteLine($"Are you sure to delete this book?");
            var response = Console.ReadKey().KeyChar;
            if (response.ToString().ToUpper() == "S")
            {
                if(bookService.Delete(bookId,out var errors))
                {
                    Console.WriteLine("Book Successfully Deleted");

                }
                else
                {
                    Console.WriteLine("Errors while trying to delete a book");
                    errors.ForEach(x => Console.WriteLine(x));
                }
            }
            else
            {
                Console.WriteLine("Operation cancelled by user");
            }
            Console.ReadLine();
        }
        private void AddBooks(IBookService bookService, IAuthorService authorService)
        {
            Console.Clear();
            Console.WriteLine("Adding New Books");
            Console.Write("Enter book's title:");
            var title = Console.ReadLine();
            Console.Write("Enter Publish Date (dd/mm/yyyy):");
            if (!DateOnly.TryParse(Console.ReadLine(), out var publishDate))
            {
                Console.WriteLine("Wrong Date....");
                Console.ReadLine();
                return;
            }
            Console.Write("Enter Page Count:");
            if (!int.TryParse(Console.ReadLine(), out var pages))
            {
                Console.WriteLine("Wrong Page Count...");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("List of Authors to Select");

            var authorsList = authorService!.GetAll("Id");
            foreach (var author in authorsList)
            {
                Console.WriteLine($"{author.Id} - {author.FirstName} {author.LastName}");
            }

            Console.Write("Enter AuthorID (0 New Author):");
            if (!int.TryParse(Console.ReadLine(), out var authorId) || authorId < 0)
            {
                Console.WriteLine("Invalid AuthorID....");
                Console.ReadLine();
                return;
            }
            if (authorId > 0)
            {
                var selectedAuthor = authorService.GetById(authorId);
                if (selectedAuthor is null)
                {
                    Console.WriteLine("Author not found!!!");
                    Console.ReadLine();
                    return;
                }

            }
            else
            {
                //Entering new author
                Console.WriteLine("Adding a New Author");
                Console.Write("Enter First Name:");
                var firstName = Console.ReadLine();
                Console.Write("Enter Last Name:");
                var lastName = Console.ReadLine();

                if (authorService.Exist(firstName!,lastName!))
                {
                    var existingAuthor = authorService.GetByName(firstName ?? string.Empty, lastName ?? string.Empty);

                    Console.WriteLine("You have entered an existing author!!!");
                    Console.WriteLine("Assigning his AuthorID");
                    authorId = existingAuthor!.Id;


                }
                else
                {
                    AuthorCreateDto newAuthor = new AuthorCreateDto
                    {
                        FirstName = firstName ?? string.Empty,
                        LastName = lastName ?? string.Empty,
                    };
                    AuthorDto? authorCreated=null;
                    if(authorService.Create(newAuthor, out authorCreated,out var authorErrors))
                    {
                        Console.WriteLine("New Author Added Successfully");
                        authorId = authorCreated!.Id;
                    }
                    else
                    {
                        Console.WriteLine("Errors while trying to add a new author");
                        foreach (var item in authorErrors)
                        {
                            Console.WriteLine(item);   
                        }
                    }

                }
                
            }
            BookCreateDto bookDto = new BookCreateDto
            {
                Title = title ?? string.Empty,
                Pages = pages,
                PublishDate = publishDate,
                AuthorId = authorId
            };
            if(bookService.Create(bookDto, out var errors))
            {
                Console.WriteLine("Book Successfully Added");
            }
            else
            {
                Console.WriteLine("Errors while trying to add a new book");
                errors.ForEach(error => Console.WriteLine(error));
            }

            Console.ReadLine();
        }
        private void BooksGroupByAuthor(IBookService bookService)
        {
            Console.Clear();
            Console.WriteLine("List of Books");
            var groups = bookService.BooksGroupByAuthor();
            foreach (var group in groups)
            {
                Console.WriteLine($"AuthorId:{group.Author.Id} - Author: {group.Author.FirstName} {group.Author.LastName}");
                Console.WriteLine("    Books");
                if (group.Books is not null)
                {
                    foreach (var book in group.Books)
                    {
                        Console.WriteLine($"        {book.Title}");

                    }

                }
                else
                {
                        Console.WriteLine("         No fucking books!!!");
                }
            }
            Console.ReadLine();
        }

        private void BooksList(IBookService bookService)
        {
            Console.Clear();
            Console.WriteLine("List of Available Books");
            var books = bookService.GetAll();
            foreach (var book in books)
            {
                Console.WriteLine($"{book.Title}");
            }
            Console.ReadLine();
        }



    }
}
