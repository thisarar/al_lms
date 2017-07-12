using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlLibraryManagementSystem.Models;

namespace AlLibraryManagementSystem.DataAccess
{
    /**
     * Access to the database to perform all CRUD and and other DB operations.
     * This class operates with the LINQ model of Book in the database.
     **/
    class BookDataAccess
    {
        /**
         * Creates new book entry in database. Data inserts as a single value. This takes the
         * current Data Context and do the insert, commit and disposal operations on database.
         * If an exception occured, this returns false. If the book successfully added, this
         * will returns false. LINQ Book object should be passed as the inserting object.
         **/
        public static bool Insert(Book book)
        {
            try
            {
                library_dbEntities dbContext = new library_dbEntities();
                dbContext.Books.Add(book);
                dbContext.SaveChanges();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public static List<Book> SearchBooks(String keyword, int categoryId)
        {
            library_dbEntities dbContext = new library_dbEntities();
            var books = from b in dbContext.Books
                        join c in dbContext.Categories on b.CategoryId equals c.CategoryId
                        where c.CategoryId == categoryId
                        select b;
            if (!string.IsNullOrWhiteSpace(keyword))
                books = books.Where(b => b.BookName.ToLower().Contains(keyword.ToLower()));
            return books.ToList();
        }

        public static List<Book> SearchBooks()
        {
            library_dbEntities dbContext = new library_dbEntities();
            var books = from b in dbContext.Books select b;
            return books.ToList();
        }

        public static Book FindBook(int bookId)
        {
            library_dbEntities dbContext = new library_dbEntities();
            return (from b in dbContext.Books where b.BookId == bookId select b).First();

        }

        public static List<Book> ListBooksByCategory(int categoryId)
        {
            library_dbEntities dbContext = new library_dbEntities();
            var books = from b in dbContext.Books
                        join c in dbContext.Categories on b.CategoryId equals c.CategoryId
                        where c.CategoryId == categoryId
                        select b;
            return books.ToList();
        }

        public static int CountPublications(int category)
        {
            library_dbEntities dbContext = new library_dbEntities();
            return (from b in dbContext.Books where b.CategoryId == category select b).Count();
        }
    }
}