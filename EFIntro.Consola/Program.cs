using EFIntro.Ioc;

namespace EFIntro.Consola
{
    internal class Program
    {
        static IServiceProvider _serviceProvider = null!;

        static void Main(string[] args)
        {
            _serviceProvider = DI.ConfigureDI();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var authorMenu=new AuthorMenu(_serviceProvider);
            var bookMenu=new BookMenu(_serviceProvider);
            //CreateDb();
            do
            {
                Console.Clear();
                Console.WriteLine("Main Menu");
                Console.WriteLine("1 - Authors");
                Console.WriteLine("2 - Books");
                Console.WriteLine("x - Exit");
                Console.Write("Enter an option:");
                var option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        authorMenu.MostrarMenu();
                        break;
                    case "2":
                        bookMenu.MostrarMenu();
                        break;
                    case "x":
                        Console.WriteLine("Fin del Programa");
                        return;
                    default:
                        break;
                }

            } while (true);
        }


        private static void AuthorsMenu()
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
                switch (option)
                {
                    case "1":
                    //    AuthorsList();
                    //    break;
                    //case "2":
                    //    AddAuthors();
                    //    break;
                    //case "3":
                    //    DeleteAuthors();
                    //    break;
                    //case "4":
                    //    EditAuthors();
                    //    break;
                    //case "5":
                    //    ListOfAuthorsWithBooks();
                    //    break;
                    //case "6":
                    //    AuthorsWithBooksSummaryOrDetails();
                    //    break;
                    case "r":
                        return;
                    default:
                        break;
                }

            } while (true);

        }



        //private static void EditAuthors()
        //{
        //    Console.Clear();
        //    Console.WriteLine("Edit An Author");
        //    using (var scope = _serviceProvider.CreateScope())
        //    {
        //        var authorService = scope.ServiceProvider.GetRequiredService<IAuthorService>();
        //        var authors = authorService.GetAll("Id");
        //        foreach (var author in authors)
        //        {
        //            Console.WriteLine($"{author.Id} - {author}");
        //        }
        //        Console.Write("Enter an AuthorID to edit (0 to Escape):");
        //        int authorId;
        //        if (!int.TryParse(Console.ReadLine(), out authorId) || authorId < 0)
        //        {
        //            Console.WriteLine("Invalid AuthorId!!!");
        //            Console.ReadLine();
        //            return;
        //        }
        //        if (authorId == 0) return;
        //        var authorInDb = authorService.GetById(authorId);
        //        if (authorInDb == null)
        //        {
        //            Console.WriteLine("Author does not exist");
        //            Console.ReadLine();
        //            return;
        //        }

        //        Console.WriteLine($"Current Author First Name: {authorInDb.FirstName}");
        //        Console.Write("Enter New First Name (or ENTER to Keep the same)");
        //        var newFirstName = Console.ReadLine();
        //        if (!string.IsNullOrEmpty(newFirstName))
        //        {
        //            authorInDb.FirstName = newFirstName;
        //        }
        //        Console.WriteLine($"Current Author Last Name: {authorInDb.LastName}");
        //        Console.Write("Enter New Last Name (or ENTER to Keep the same)");
        //        var newLastName = Console.ReadLine();
        //        if (!string.IsNullOrEmpty(newLastName))
        //        {
        //            authorInDb.LastName = newLastName;
        //        }

        //        var originalAuthor = authorService.GetById(authorId);

        //        Console.Write($"Are you sure to edit \"{originalAuthor!.FirstName} {originalAuthor.LastName}\"? (y/n):");
        //        var confirm = Console.ReadLine();
        //        if (confirm?.ToLower() == "y")
        //        {
        //            bool exist = authorService.Exist(authorInDb.FirstName,
        //                authorInDb.LastName,
        //                authorInDb.Id);
        //            if (!exist)
        //            {
        //                var authorValidator = new AuthorValidator();

        //                var result = authorValidator.Validate(authorInDb);

        //                if (result.IsValid)
        //                {
        //                    authorService.Save(authorInDb);
        //                    Console.WriteLine("Author successfully edited");

        //                }
        //                else
        //                {
        //                    foreach (var message in result.Errors)
        //                    {
        //                        Console.WriteLine(message);
        //                    }

        //                }

        //            }
        //            else
        //            {
        //                Console.WriteLine("Author already exist!!!");
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine("Operation cancelled by user");
        //        }
        //        Console.ReadLine();
        //    }
        //}




    }
}
