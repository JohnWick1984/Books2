using System;
using System.Collections;
using System.Collections.Generic;

public class InvalidBookDataException : Exception
{
    public InvalidBookDataException(string message) : base(message)
    {
    }
}

public class Book : ICloneable
{
    private string title;
    private string author;

    public string Title
    {
        get { return title; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidBookDataException("Title cannot be empty or null.");
            }
            title = value;
        }
    }

    public string Author
    {
        get { return author; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidBookDataException("Author cannot be empty or null.");
            }
            author = value;
        }
    }

    public object Clone()
    {
        return new Book { Title = this.Title, Author = this.Author };
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Book otherBook = (Book)obj;
        return Title == otherBook.Title && Author == otherBook.Author;
    }

    public override int GetHashCode()
    {
        return Tuple.Create(Title, Author).GetHashCode();
    }

    public override string ToString()
    {
        return $"{Title} by {Author}";
    }
}

public class BookList : IEnumerable<Book>, ICloneable
{
    private List<Book> books = new List<Book>();

    public void AddBook(Book book)
    {
        try
        {
            book.Title = book.Title; // Проверка на корректность данных перед добавлением
            book.Author = book.Author;
            books.Add(book);
        }
        catch (InvalidBookDataException ex)
        {
            Console.WriteLine($"Error adding book: {ex.Message}");
        }
    }

    public void RemoveBook(Book book)
    {
        books.Remove(book);
    }

    public bool ContainsBook(Book book)
    {
        return books.Contains(book);
    }

    public Book this[int index]
    {
        get
        {
            try
            {
                if (index < 0 || index >= books.Count)
                {
                    throw new IndexOutOfRangeException("Index is out of range.");
                }
                return books[index];
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine($"Error accessing book at index {index}: {ex.Message}");
                throw; // Перевыбрасываем исключение после вывода сообщения
            }
        }
    }

    public IEnumerator<Book> GetEnumerator()
    {
        return books.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public object Clone()
    {
        BookList clone = new BookList();
        foreach (var book in books)
        {
            clone.AddBook((Book)book.Clone());
        }
        return clone;
    }
}

class Program
{
    static void Main()
    {
        try
        {
            BookList bookList = new BookList();

            // Добавление книг
            bookList.AddBook(new Book { Title = "Book1", Author = "Author1" });
            bookList.AddBook(new Book { Title = "Book2", Author = "Author2" });

            // Вывод списка книг
            Console.WriteLine("Books in the list:");
            foreach (var book in bookList)
            {
                Console.WriteLine(book);
            }

            // Добавление книг с некорректными данными
            bookList.AddBook(new Book { Title = null, Author = "Author1" });
            bookList.AddBook(new Book { Title = "Book2", Author = null });

            // Вывод списка книг
            Console.WriteLine("Books in the list:");
            foreach (var book in bookList)
            {
                Console.WriteLine(book);
            }

            // Проверка наличия книги в списке
            Book bookToCheck = new Book { Title = "Book1", Author = "Author1" };
            Console.WriteLine($"Is {bookToCheck} in the list? {bookList.ContainsBook(bookToCheck)}");

            // Попытка обращения к несуществующему индексу
            Console.WriteLine(bookList[10]);
        }
        catch (InvalidBookDataException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (IndexOutOfRangeException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
