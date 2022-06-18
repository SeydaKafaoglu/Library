using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class BooksController : LibraryControllerController
    {
        // GET: Books
        public ActionResult Index()
        {
            Models.libraryTableAdapters.BooksTableAdapter booksAdapter = new Models.libraryTableAdapters.BooksTableAdapter();
            Models.library.BooksDataTable bookTable = new Models.library.BooksDataTable();

            booksAdapter.Fill(bookTable);
            ViewData["bookResults"] = bookTable.Rows;
            return View();
        
        }
        public ActionResult Add(string error = null)
        {
            Models.libraryTableAdapters.WriterTableAdapter writersAdapter = new Models.libraryTableAdapters.WriterTableAdapter();
            Models.library.WriterDataTable writersTable = new Models.library.WriterDataTable();

            ViewData["title"] = "Kitap ekle";

            if (error == null)
            {
                ViewData["error"] = error;
            }
            writersAdapter.Fill(writersTable);
            ViewData["writers"] = writersTable.Rows;

            return View();

        }
        public void ProcessNewBook(string bookName, string keywords, long writer, byte stock,short closet, byte shelf, bool hirable=false)
        {

            Models.libraryTableAdapters.BooksTableAdapter bookAdapter;
            long id;
           
            DateTime addedOn;
            Models.libraryTableAdapters.QueriesTableAdapter queriesTableAdapter = new Models.libraryTableAdapters.QueriesTableAdapter();
       
            if (bookName == null)
            {
                Response.Redirect("~/books/add");
                return;
            }
            bookName = bookName.Trim();
            if (bookName == "")
            {
                Response.Redirect("~/books/add");
                return;
            }
            if (bookName.Length > 100)
            {
                Response.Redirect("~/books/add");
                return;

            }
            if (keywords != null)
            {
                keywords = keywords.Trim();
                if (keywords == "")
                {
                    keywords = null;


                }
                else
                {
                    if (keywords.Length > 100)
                    {
                        Response.Redirect("~/books/add");
                        return;

                    }
                }

            }
            try
            {
                bookAdapter = new Models.libraryTableAdapters.BooksTableAdapter();
                addedOn = DateTime.Now;
                bookAdapter.AddBook(bookName, keywords,addedOn,stock,stock, closet,shelf,hirable);
                id = bookAdapter.BookId(bookName, addedOn).Value; //value bookıd bizim söylediğimiz kitap isminde ve tarihteki kitabı arayacak. oyle bir kitap olmayabilir. oyle bir kitap yoksa gerçek değerini bul.
                queriesTableAdapter.AddWritersBook(id, writer);          
                Response.Redirect("~/books");

            }
            catch(Exception ex)
            {
                Response.Redirect("~/books/add");

            }

        }
        public void Delete(long id)
        {
            Models.libraryTableAdapters.BooksTableAdapter booksAdapter = new Models.libraryTableAdapters.BooksTableAdapter();
            booksAdapter.DeletedBooks(id);
            Response.Redirect("~/books");
        }
        public ActionResult Update(long id)
        {
            Models.libraryTableAdapters.Books1TableAdapter bookAdapter = new Models.libraryTableAdapters.Books1TableAdapter();
            Models.library.Books1DataTable bookTable = new Models.library.Books1DataTable();
            Models.library.Books1Row books1Row;
            Models.libraryTableAdapters.WriterTableAdapter writersAdapter = new Models.libraryTableAdapters.WriterTableAdapter();
            Models.library.WriterDataTable writersTable = new Models.library.WriterDataTable();


            bookAdapter.FillBy(bookTable, id);
            books1Row = (Models.library.Books1Row)bookTable.Rows[0];
            ViewData["title"] = books1Row.booksName;
            ViewData["booksRow"] = books1Row;
            writersAdapter.Fill(writersTable);
            ViewData["writers"] = writersTable.Rows;

            return View();

        }
        public void ProcessBooks(long id, string booksName, string keywords, long writer, byte stock, byte activeStock, short closet, byte shelf, bool hirable = false)
        {
            Models.libraryTableAdapters.QueriesTableAdapter queriesTableAdapter = new Models.libraryTableAdapters.QueriesTableAdapter();
            Models.libraryTableAdapters.BooksTableAdapter bookAdapter;

            if (booksName == null)
            {
                Response.Redirect("~/books/update" + id.ToString());
                return;
            }
            booksName = booksName.Trim();
            if (booksName == "")
            {
                Response.Redirect("~/books/update" + id.ToString());
                return;
            }
            if (booksName.Length > 100)
            {
                Response.Redirect("~/books/update" + id.ToString());
                return;

            }
            if (keywords != null)
            {
                keywords = keywords.Trim();
                if (keywords == "")
                {
                    keywords = null;


                }
                else
                {
                    if (keywords.Length > 100)
                    {
                        Response.Redirect("~/books/update" + id.ToString());
                        return;

                    }
                }

            }
            try
            {
                bookAdapter = new Models.libraryTableAdapters.BooksTableAdapter();
                bookAdapter.UpdateBook(booksName, keywords,stock,activeStock,closet,shelf,hirable,id);
                queriesTableAdapter.DeleteWriterBook(id);
                queriesTableAdapter.AddWritersBook(id, writer);
                Response.Redirect("~/books");

            }
            catch(Exception ex)
            {
                Response.Redirect("~/books/update" + id.ToString());

            }

        }
        public ActionResult BookData(long id)
        {
            Models.libraryTableAdapters.Books1TableAdapter books1Adapter = new Models.libraryTableAdapters.Books1TableAdapter();
            Models.library.Books1DataTable books1Table = new Models.library.Books1DataTable();
            books1Adapter.FillBy(books1Table, id);                
            ViewData["booksRow"] = (Models.library.Books1Row)books1Table.Rows[0];
            return PartialView(); //tam sayfa değil parça gönder.
        }
    }
}