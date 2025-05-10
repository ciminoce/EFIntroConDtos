using EFIntro.Service.DTOs.Author;
using EFIntro.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EFIntro.Consola
{
    public class AuthorMenu
    {
        private readonly IServiceProvider _serviceProvider = null!;

        public AuthorMenu(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void MostrarMenu()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("✍️ AUTHORS");
                Console.WriteLine("1 - List of Authors");
                Console.WriteLine("2 - Add New Author");
                Console.WriteLine("3 - Delete an Author");
                Console.WriteLine("4 - Edit an Author");
                Console.WriteLine("5 - List of Authors With Books");
                Console.WriteLine("6 - Authors With Books (Summary or Details)");
                Console.WriteLine("r - Return");
                Console.Write("Enter an option:");
                var option = Console.ReadLine();

                using (var scope=_serviceProvider.CreateScope())
                {
                    var authorService=scope.ServiceProvider
                        .GetRequiredService<IAuthorService>();
                    switch (option)
                    {
                        case "1":
                            AuthorsList(authorService);
                            break;
                        case "2":
                            AddAuthors(authorService);
                            break;
                        case "3":
                            DeleteAuthors(authorService);
                            break;
                        case "4":
                            EditAuthors(authorService);
                            break;
                        case "5":
                            ListOfAuthorsWithBooks(authorService);
                            break;
                        case "6":
                            AuthorsWithBooksSummaryOrDetails(authorService);
                            break;
                        case "r":
                            return;
                        default:
                            break;
                    }

                }

            } while (true);

        }

        private void EditAuthors(IAuthorService authorService)
        {
            do
            {
                Console.Clear();
                Console.WriteLine("Edit An Author");
                var authors = authorService.GetAll("Id");
                foreach (var author in authors)
                {
                    Console.WriteLine($"{author.Id} - {author.FirstName} {author.LastName}");
                }

                Console.Write("Enter an AuthorID to edit (0 to Escape):");
                int authorId;
                if (!int.TryParse(Console.ReadLine(), out authorId) || authorId < 0)
                {
                    Console.WriteLine("Invalid AuthorId!!!");
                    Console.ReadLine();
                    return;
                }
                if (authorId == 0) return;

                var authorInDb = authorService.GetById(authorId);
                if (authorInDb != null)
                {
                    Console.WriteLine($"Current Author First Name: {authorInDb.FirstName}");
                    Console.Write("Enter New First Name (or ENTER to Keep the same)");
                    var newFirstName = Console.ReadLine();
                    if (string.IsNullOrEmpty(newFirstName))
                    {
                        newFirstName = authorInDb.FirstName;
                    }
                    Console.WriteLine($"Current Author Last Name: {authorInDb.LastName}");
                    Console.Write("Enter New Last Name (or ENTER to Keep the same)");
                    var newLastName = Console.ReadLine();
                    if (string.IsNullOrEmpty(newLastName))
                    {
                        newLastName = authorInDb.LastName;
                    }

                    var originalAuthor = authorService.GetById(authorId);

                    Console.Write($"Are you sure to edit \"{originalAuthor!.FirstName} {originalAuthor.LastName}\"? (y/n):");
                    var confirm = Console.ReadLine();
                    if (confirm?.ToLower() == "y")
                    {
                        AuthorUpdateDto authorUpdateDto = new AuthorUpdateDto()
                        {
                            Id = authorInDb.Id,
                            FirstName = newFirstName ?? string.Empty,
                            LastName = newLastName ?? string.Empty,
                        };
                        if (authorService.Update(authorUpdateDto, out var errors))
                        {
                            Console.WriteLine("Author successfully updated");
                        }
                        else
                        {
                            Console.WriteLine("Errors while trying to update an author!!");
                            foreach (var message in errors)
                            {
                                Console.WriteLine(message);
                            }
                        }

                    }
                    else
                    {
                        Console.WriteLine("Operation cancelled by user");
                    }
                }
                else
                {
                    Console.WriteLine("Author does not exist");
                }
                Console.ReadLine();

            } while (true);
        }

        private void DeleteAuthors(IAuthorService authorService)
        {
            Console.Clear();
            Console.WriteLine("Delete An Author");
            var authors = authorService.GetAll("Id");
            foreach (var author in authors)
            {
                Console.WriteLine($"{author.Id} - {author.FirstName} {author.LastName}");
            }

            Console.Write("Enter an AuthorID to delete (0 to Escape):");
            int authorId;
            if (!int.TryParse(Console.ReadLine(), out authorId) || authorId < 0)
            {
                Console.WriteLine("Invalid AuthorId!!!");
                Console.ReadLine();
                return;
            }
            if (authorId == 0) return;
            var authorInDb= authorService.GetById(authorId);
            if (authorInDb is null)
            {
                Console.WriteLine("ID no found!!!");
                Console.ReadLine();
                return;
            }
            Console.Write($"Are you sure to delete \"{authorInDb.FirstName} {authorInDb.LastName}\"? (y/n):");
            var confirm = Console.ReadLine();
            if (confirm?.ToLower() == "y")
            {
                if(authorService.Delete(authorId, out var errors))
                {
                    Console.WriteLine("Author Successfully Removed");
                }
                else
                {
                    Console.WriteLine("Error while trying to delete an author");
                    foreach (var message in errors)
                    {
                        Console.WriteLine(message);
                    }
                }
            }
            else
            {
                Console.WriteLine("Operation cancelled by user");
            }

            Console.ReadLine();

        }

        private void AddAuthors(IAuthorService authorService)
        {
            Console.Clear();
            Console.WriteLine("Adding a New Author");
            Console.Write("Enter First Name:");
            var firstName = Console.ReadLine();
            Console.Write("Enter Last Name:");
            var lastName = Console.ReadLine();

            var authorDto = new AuthorCreateDto
            {
                FirstName = firstName ?? string.Empty,
                LastName = lastName ?? string.Empty
            };


            if (authorService.Create(authorDto,out var errors))
            {
                Console.WriteLine("Author Succesfully Added");

            }
            else
            {
                foreach (var message in errors)
                {
                    Console.WriteLine(message.ToString());
                }
            }
            Console.ReadLine();
        }

        private void AuthorsWithBooksSummaryOrDetails(IAuthorService authorService)
        {
            Console.Clear();
            Console.WriteLine("List of Authors");

            Console.Write("Show (1) Summary or (2) Details? ");
            var option = Console.ReadLine();

            var authorsWithBooks = authorService.AuthorsWithBooksCount();
            foreach (var author in authorsWithBooks)
            {
                Console.WriteLine($"{author.Id} - {author.FullName} (Books: {author.BooksCount})");

                if (option == "2") // Opción de detalle
                {
                    if (author.Books!.Any())
                    {
                        Console.WriteLine("   📚 Books:");
                        foreach (var book in author.Books!)
                        {
                            Console.WriteLine($"     - {book.Title}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("   🚫 No books available.");
                    }
                }
            }
            Console.ReadLine();

        }

        private void ListOfAuthorsWithBooks(IAuthorService authorService)
        {
            Console.Clear();
            Console.WriteLine("List of Books");

            var authorsWithBooks = authorService.GetAllWithBooks();
            foreach (var item in authorsWithBooks)
            {
                Console.WriteLine($"AuthorId:{item.Id} - Author: {item.FullName}");
                Console.WriteLine("    Books");
                if (item.Books.Count > 0)
                {
                    foreach (var book in item.Books)
                    {
                        Console.WriteLine($"        {book.Title}");
                    }

                }
                else
                {
                    Console.WriteLine("         No books available yet");
                }
            }
            Console.WriteLine("ENTER to continue");
            Console.ReadLine();
        }

        private void AuthorsList(IAuthorService authorService)
        {
            Console.Clear();
            Console.WriteLine("List of Authors");
           
            var authors = authorService.GetAll();
            foreach (var author in authors)
            {
                Console.WriteLine($"{author.LastName}, {author.FirstName}");
            }

            Console.WriteLine("ENTER to continue");
            Console.ReadLine();
        }
    }
}
