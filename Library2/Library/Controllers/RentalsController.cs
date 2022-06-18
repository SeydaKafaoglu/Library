using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class RentalsController : LibraryControllerController
    {
        // GET: Rentals
        public ActionResult Index(string error = null)
        {
            if (error != null)
            {
                if (error != "")
                {
                    ViewData["error"] = error;
                }
            }
            return View();
        }
        public void ProcessRentals(long bookId, long memberId, bool type =true)
        {
            bool? gender = null;
            Models.libraryTableAdapters.QueriesTableAdapter queriesTableAdapter = new Models.libraryTableAdapters.QueriesTableAdapter();
            MembersController membersController = new MembersController();
            Models.libraryTableAdapters.BooksTableAdapter booksTableAdapter = new Models.libraryTableAdapters.BooksTableAdapter();
            Models.libraryTableAdapters.RentalsTableAdapter rentalsTableAdapter = new Models.libraryTableAdapters.RentalsTableAdapter();
            
            Models.libraryTableAdapters.TransactionsTableAdapter transactionsTableAdapter = new Models.libraryTableAdapters.TransactionsTableAdapter();


            string errror = "";
            if (membersController.MemberExists(memberId) == false)
            {
                if (Request.Params["gender"] != null)
                {
                    gender = Convert.ToBoolean(Request.Params["gender"]);
                }
                membersController.ProcessNewMember(memberId, Request.Params["memberName"], Request.Params["memberSurName"], Request.Params["eMail"], Convert.ToInt64(Request.Params["phone"]), Request.Params["address"], gender);
            }

            if (rentalsTableAdapter.CheckRent(memberId, bookId).Value == 0)
            {
                queriesTableAdapter.AddRental(memberId, bookId, DateTime.Today, DateTime.Today.AddDays(30));
            }
            else
            {
                errror = "Kitap zaten ödünç verilmiş";
            }

            transactionsTableAdapter.AddTransaction(memberId, bookId, DateTime.Today, type);
            booksTableAdapter.DecraseActiveStock(bookId);
            Response.Redirect("~/rentals?error=" + errror, false);
        }
        public ActionResult Return()
        {

            return View();

        }
        public ActionResult CurrentRentals(long id)
        {
            Models.libraryTableAdapters.RentalsTableAdapter rentalsTableAdapter = new Models.libraryTableAdapters.RentalsTableAdapter();
            Models.library.RentalsDataTable rentalsTable = new Models.library.RentalsDataTable();

            rentalsTableAdapter.Fill(rentalsTable, id);
            ViewData["rentals"] = rentalsTable.Rows;
            return PartialView();

        }

        public void ProcessReturn(long bookId, long member, bool type=false)
        {
            Models.libraryTableAdapters.RentalsTableAdapter rentalsTableAdapter = new Models.libraryTableAdapters.RentalsTableAdapter();

            Models.libraryTableAdapters.BooksTableAdapter booksTableAdapter = new Models.libraryTableAdapters.BooksTableAdapter();
            Models.libraryTableAdapters.TransactionsTableAdapter transactionsTableAdapter = new Models.libraryTableAdapters.TransactionsTableAdapter();
            long? delayFee = null;
            long? collectedby = null; //null olabilir denilenler boyle tanımlanıyor.
            DateTime deadline = rentalsTableAdapter.Deadline(member, bookId).Value;//deadline değeri için select single value yaptık.hesaplama için gerekliydi.

            int latency = (DateTime.Today - deadline).Days;
            if (latency > 0)
            {
                delayFee = latency * 50;
                collectedby = (long)Session["userId"];
            }

            rentalsTableAdapter.ReturnBook(DateTime.Today, delayFee, collectedby, member, bookId);
            transactionsTableAdapter.AddTransaction(member, bookId, DateTime.Today, type);
            booksTableAdapter.IncreaseBookStock(bookId);
            Response.Redirect("~/rentals/return", false);

        }
    }
}
