using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace LibraryProj
{
    internal enum Commands
    {
        none, // none
        signUp, // default mode
        signIn, // default mode 

        exit, // all modes
        searchBook, // all modes 
        allCommands, // all modes (last command for default mode)


        //User commands
        signOut, // user & User modes

        //user & User modes
        bookReserve, // user & User modes 
        getBookId,

        //User commands
        addAdmin,
        addBook,
        removeBook,
        viweBookHistory,
    }

    public static class Library
    {
        //Data
        public static List<Book> Books = new List<Book>();

        private static List<User> _users = new List<User>();

        /* Methods */
        private static Commands DetectCommand(string str)
        {
            str = str.Trim().Split()[0];
            foreach (string val in typeof(Commands).GetEnumNames())
            {
                if (val.Equals(str))
                {
                    return (Commands) Enum.Parse(typeof(Commands), str);
                }
            }
            return Commands.none;
        }

        private static void PrintCommands(User currentUser)
        {
            string res = "";
            string[] cmds = typeof(Commands).GetEnumNames

        private static string GetPassword()
        {
            string password = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        password = password.Substring(0, (password.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            } while (key.Key != ConsoleKey.Enter);
            return Encode.Encrypt(password);
        }

        private static User SignUp()
        {
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("Surname: ");
            string surname = Console.ReadLine();
            newNickname:
            Console.Write("Nickname: ");
            string nickname = Console.ReadLine();
            foreach (User user in _users)
            {
                if (user.Nickname == nickname)
                {
                    Console.WriteLine("nickname is busy");
                    goto newNickname;
                }
            }
            Console.Write("Password: ");
            string password = GetPassword();
            Console.WriteLine();
            Console.Write("Telephone: ");
            string telephone = Console.ReadLine();
            User usr = new User(false, name, surname, nickname, password, telephone);
            _users.Add(usr);
            return usr;
        }

        private static User SignIn()
        {
            Console.Write("Nickname: ");
            string nickname = Console.ReadLine();
            Console.Write("Password: ");
            string password = Encode.Encrypt(GetPassword());
            foreach (var usr in _users)
            {
                if (usr.Nickname == nickname && usr.Password == password)
                {
                    return usr;
                }
            }
            MessageBox.Show("incorrect nickname / password.");
            Console.WriteLine();
            return null;
        }

        //search
        private static List<Book> StricktSearch(string name, string author)
        {
            List<Book> res = new List<Book>();
            foreach (var book in Books)
            {
                if (book.Name == name && book.Author == author)
                {
                    res.Add(book);
                }
            }
            return res;
        }

        private static List<Book> NonStrictSearch(string name, string author)
        {
            List<Book> res = new List<Book>();
            foreach (var book in Books)
            {
                if (book.Name.Contains(name) || book.Author.Contains(author))
                {
                    res.Add(book);
                }
            }
            return res;
        }

        private static void PrintBooks(List<Book> books)
        {
            for (int i = 0; i < books.Count; i++)
            {
                Console.WriteLine($"{i} " + books[i]);
            }
        }

        private static Book SearchBook()
        {
            Console.WriteLine("# strickt and nonstrickt search");
            Console.WriteLine("book name: ");
            string name = Console.ReadLine();
            Console.WriteLine("book author: ");
            string author = Console.ReadLine();
            Console.WriteLine("book age: ");
            List<Book> res = StricktSearch(name, author);
            if (res.Count == 0)
            {
                res = NonStrictSearch(name, author);
            }
            if (res.Count == 0)
            {
                Console.WriteLine($"Your search -name:{name}, author:{author} - did not match any book.");
                return null;
            }
            PrintBooks(res);
            Console.Write("select number for selecting book: ");
            int l = int.Parse(s: Console.ReadLine());
            if (l < 0 || l >= res.Count)
            {
                Console.WriteLine($"number ({l}) is incorrect");
                return null;
            }
            return res[l];
        }

        //book reserve
        private static void BookReserve(User user, Book selectedBook)
        {
            if (user == null)
            {
                Console.WriteLine("no signed in.");
                return;
            }
            if (selectedBook == null)
            {
                Console.WriteLine("no book selected.");
                return;
            }
            Console.WriteLine("are you want to reserve this book? (Y/n) : ");
            string readLine = Console.ReadLine();
            if (readLine != null && readLine.ToLower().Trim() == "y")
            {
                int lgt = selectedBook.BookHistory.Count;
                if (!selectedBook.IsNowUsing)
                {
                    Console.WriteLine("book is already reserved.");
                    return;
                }
                user.UserHistory.Add(new UserHistory(selectedBook, DateTime.Now,
                    DateTime.Now.Add(new TimeSpan(30, 0, 0, 0))));
                selectedBook.BookHistory.Add(new BookHistory(user, DateTime.Now,
                    DateTime.Now.Add(new TimeSpan(30, 0, 0, 0))));
            }
        }

        private static int GetUserIndexByNickname(string nickname, List<User> users)
        {
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].IsAdmin)
                    return i;
            }
            return -1;
        }

        private static void AddAdmin()
        {
            Console.Write("Nickname: ");
            string nickname = Console.ReadLine();
            int ind = GetUserIndexByNickname(nickname, _users);
            if (ind != -1 && !_users[ind].IsAdmin)
            {
                _users[ind].IsAdmin = true;
                Console.WriteLine($"user {nickname} is became admin");
                return;
            }
            Console.WriteLine($"incorrect nickname({nickname})");
        }

        private static void AddBook()
        {
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("Author name: ");
            string author = Console.ReadLine();
            Console.Write("Age: ");
            int age = int.Parse(s: Console.ReadLine());
            Console.Write("Price: ");
            decimal price = Decimal.Parse(s: Console.ReadLine());
            Books.Add(new Book(name, author, age, price));
        }

        private static void RemoveBook(User user)
        {
            Book bk = null;
            Console.Write("Book ID: ");
            Guid id = Guid.Parse(input: Console.ReadLine());
            foreach (Book b in Books)
            {
                if (b.Id == id)
                {
                    bk = b;
                    break;
                }
            }
            if (bk == null)
            {
                Console.WriteLine($"incorrect ID ({id})");
                return;
            }
            Console.Write("Password: ");
            if (user.Password == GetPassword())
            {
                Books.Remove(bk);
                Console.WriteLine($"Deleted book: {bk}");
            }
            else
            {
                Console.WriteLine("incorrect password.");
            }
        }

        public static void Terminal()
        {
            const string bookPath = @"../../Books.json";
            const string usersPath = @"../../Users.json";
            Console.WriteLine();
            _users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(usersPath));
            Books = JsonConvert.DeserializeObject<List<Book>>(File.ReadAllText(bookPath));
            User currentUser = null;
            Commands command = Commands.none;

            PrintCommands(currentUser);
            Console.WriteLine();
            while (command != Commands.exit)
            {
                string line = Console.ReadLine();
                command = DetectCommand(line);
                Book selectedBook = null;
                switch (command)
                {
                    case Commands.exit: return;
                    case Commands.allCommands:
                        PrintCommands(currentUser);
                        break;
                    case Commands.signUp:
                        if (currentUser != null)
                        {
                            command = Commands.none;
                            break;
                        }
                        currentUser = SignUp();
                        _users.Add(currentUser);
                        break;
                    case Commands.signIn:
                        if (currentUser == null)
                        {
                            currentUser = SignIn();
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"{line}: command not found");
                        }
                        break;
                    case Commands.searchBook:
                        selectedBook = SearchBook();
                        Console.WriteLine(selectedBook == null ? "incorrect data." : "book is selected.");
                        break;
                    case Commands.signOut:
                        currentUser = null;
                        selectedBook = null;
                        break;
                    case Commands.bookReserve:
                        BookReserve(currentUser, selectedBook);
                        break;
                    case Commands.addAdmin:
                        if (currentUser is User)
                        {
                            AddAdmin();
                            break;
                        }
                        Console.WriteLine("incorrect command.");
                        break;
                    case Commands.addBook:
                        if (currentUser is User)
                        {
                            AddBook();
                            break;
                        }
                        Console.WriteLine("incorrect command.");
                        break;
                    case Commands.removeBook:
                        if (currentUser is User)
                        {
                            RemoveBook(currentUser);
                            break;
                        }
                        Console.WriteLine("incorrect command.");
                        break;

                    case Commands.viweBookHistory:
                        if (currentUser is User)
                        {
                            if (selectedBook == null)
                            {
                                Console.WriteLine("no selected book.");
                                break;
                            }
                            selectedBook.PrintBookHistory();
                            break;
                        }
                        Console.WriteLine("incorrect command.");
                        break;
                        ;
                }
                if (command == Commands.none)
                {
                    Console.WriteLine("incorrect command.");
                }

                File.WriteAllText(bookPath, JsonConvert.SerializeObject(Books));
                File.WriteAllText(usersPath, JsonConvert.SerializeObject(_users));
            }
        }
    }
}