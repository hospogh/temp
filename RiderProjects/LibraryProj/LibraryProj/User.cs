namespace LibraryProj
{
    public sealed class User
    {
        public string Name { get; set; }

        public bool IsAdmin { get; set; }
        public string Surname { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public string Telephone { get; set; }
        public decimal Money { get; set; }
        public List<UserHistory> UserHistory { get; set; }


        public User()
        {
        }

        public User(bool isAdmin, string name, string surname, string nickname, string password)
        {
            IsAdmin = isAdmin;
            Name = name;
            Surname = surname;
            Nickname = nickname;
            Password = password;
            UserHistory = new List<UserHistory>();
            Money = 0.0m;
        }

        public User(bool isAdmin, string name, string surname, string nickname, string password, string telephone) :
            this(isAdmin, name, surname, nickname, password)
        {
            Telephone = telephone;
        }

        public void AddBook(Book book, List<Book> allBooks)
        {
            if (IsAdmin)
            {
                allBooks.Add(book);
            }
            else
            {
                Console.WriteLine("IsAdmin:false");
            }
        }

        public void RemoveBook(Guid bookId, List<Book> allBooks)
        {
            if (!IsAdmin)
            {
                Console.WriteLine("IsAdmin:false");
                return;
            }
            foreach (Book book in allBooks)
            {
                if (book.Id != bookId) continue;
                allBooks.Remove(book);
                Console.WriteLine("Book is removed.");
                return;
            }
            Console.WriteLine("No Book with this Id.");
        }
    }

    public class UserHistory
    {
        public Book Book { get; set; }
        public DateTime StartUsingTime { get; set; }
        public DateTime EndUsingTime { get; set; }

        public UserHistory(Book book, DateTime endTime)
        {
            Book = book;
            EndUsingTime = endTime;
            StartUsingTime = DateTime.Now;
        }

        public UserHistory()
        {
        }

        public UserHistory(Book book, DateTime startTime, DateTime endTime)
            : this(book, endTime)
        {
            StartUsingTime = startTime;
        }
    }

}