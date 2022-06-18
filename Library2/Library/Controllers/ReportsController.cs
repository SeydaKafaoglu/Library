using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class ReportsController : LibraryControllerController
    {
        // GET: Reports
        public ActionResult Gains()
        {
            return View();
        }
        public ActionResult ProcessGains(string startDate, string endDate) //tarayıcıdan viewa datetıme yollanırken ilk string getirip convert yapılması lazım. düzgün getirmez.
        {
            if (startDate == "")
            {
                startDate = "01-01-1970";
            }
            if (endDate == "")
            {
                endDate = "31-12-2300";
            }
            Models.library.GainsDataTable gainsTable = new Models.library.GainsDataTable();
            Models.libraryTableAdapters.GainsTableAdapter gainsTableAdapter = new Models.libraryTableAdapters.GainsTableAdapter();

            DateTime reportStart = DateTime.ParseExact(startDate, "dd-MM-yyyy", null); //null hangi ülke için çevirmek istersek.
            DateTime reportEnd = DateTime.ParseExact(endDate, "dd-MM-yyyy", null);
            gainsTableAdapter.Fill(gainsTable, reportStart, reportEnd);
            if (startDate == "01-01-1970")
            {
                startDate = "";
            }
            if (endDate == "31-12-2300")
            {
                endDate = "";
            }
            ViewData["startDate"] = startDate;
            ViewData["endDate"] = endDate;
            ViewData["gains"] = gainsTable.Rows;


            return View();
        }

        public ActionResult ProcessBooksReports()
        {

            Models.libraryTableAdapters.BookReportsTableAdapter bookReportsTableAdapter = new Models.libraryTableAdapters.BookReportsTableAdapter();
            Models.library.BookReportsDataTable bookReportsTable = new Models.library.BookReportsDataTable();
            bookReportsTableAdapter.Fill(bookReportsTable);
            ViewData["reports"] = bookReportsTable.Rows;
            return View();
        }
        public ActionResult Stock()
        {
            Models.libraryTableAdapters.StockControlTableAdapter stockControlTableAdapter = new Models.libraryTableAdapters.StockControlTableAdapter();
            Models.library.StockControlDataTable stockControlTable = new Models.library.StockControlDataTable();
            stockControlTableAdapter.Fill(stockControlTable);
            ViewData["stock"] = stockControlTable.Rows;
            return View();
        }

        public ActionResult StockKontrol(long id)
        {

            Models.libraryTableAdapters.BookReportsTableAdapter bookReportsTableAdapter = new Models.libraryTableAdapters.BookReportsTableAdapter();
            Models.library.BookReportsDataTable bookReportsTable = new Models.library.BookReportsDataTable();
            bookReportsTableAdapter.FillBy(bookReportsTable, id);
            ViewData["control"] = bookReportsTable.Rows;
            return PartialView();
        }
        public ActionResult DelayedBooks()
        {

            Models.libraryTableAdapters.DelayedBooksTableAdapter delayedBooksTableAdapter = new Models.libraryTableAdapters.DelayedBooksTableAdapter();
            Models.library.DelayedBooksDataTable delayedBooksTable = new Models.library.DelayedBooksDataTable();
            delayedBooksTableAdapter.DelayedBooks(delayedBooksTable, DateTime.Today);
            ViewData["delayedBooks"] = delayedBooksTable.Rows;
            return View();
        }
        public ActionResult Transactions()
        {

            return View();
        }
        public ActionResult processTransactions(string startDate, string endDate)
        {
            if (startDate == "")
            {
                startDate = "01-01-1970";
            }
            if (endDate == "")
            {
                endDate = "31-12-2300";
            }
            DateTime reportStart = DateTime.ParseExact(startDate, "dd-MM-yyyy", null); //null hangi ülke için çevirmek istersek.
            DateTime reportEnd = DateTime.ParseExact(endDate, "dd-MM-yyyy", null);
            Models.libraryTableAdapters.TransactionsTableAdapter transactionsTableAdapter = new Models.libraryTableAdapters.TransactionsTableAdapter();
            Models.library.TransactionsDataTable transactionsTable = new Models.library.TransactionsDataTable();
            transactionsTableAdapter.Fill(transactionsTable, reportStart, reportEnd);
            if (startDate == "01-01-1970")
            {
                startDate = "";
            }
            if (endDate == "31-12-2300")
            {
                endDate = "";
            }
            ViewData["transactions"] = transactionsTable.Rows;
            ViewData["startDate"] = startDate;
            ViewData["endDate"] = endDate;
            return View();
        }

    }
}