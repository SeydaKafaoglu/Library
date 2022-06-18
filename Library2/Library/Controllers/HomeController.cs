using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace Library.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index(string keywords = null)
        {
            Models.libraryTableAdapters.SearchResultsTableAdapter searchAdapter = new Models.libraryTableAdapters.SearchResultsTableAdapter();
            Models.library.SearchResultsDataTable searchResultsTable = new Models.library.SearchResultsDataTable();
            if (keywords != null)
            {
                searchAdapter.Fill(searchResultsTable, keywords.Trim());
                ViewData["results"] = searchResultsTable.Rows;
                ViewData["keywords"] = keywords;
            }
            return View();
        }

        public ActionResult Login(string error = null)
        {
            if (error != null)
            {
                ViewData["error"] = error;
            }
            return View();
        }

        public void CheckLogin(string userName, string password) //sayfaya yönlendirmediği için method yaptık. actionresultsı kaldırdık.
        {
            Models.libraryTableAdapters.UsersTableAdapter usersAdapter = new Models.libraryTableAdapters.UsersTableAdapter();
            Models.library.UsersDataTable usersTable = new Models.library.UsersDataTable();
            string hashedStr;
            Models.library.UsersRow userRow;
            usersAdapter.Fill(usersTable, userName);

            if (usersTable.Rows.Count != 1)
            {
                Response.Redirect("~/home/login?error=Bilinmeyen kullanıcı");//böyle bir kullaıcı yoksa ?=parametre önüne gelir.
                return;
            }
            userRow = (Models.library.UsersRow)usersTable.Rows[0];//0. satırını userRow değişkenine koy
            hashedStr = Utility.CalculateSHA(userName, password);

            if (hashedStr != userRow.Password)
            {
                Response.Redirect("~/home/login?error=Hatalı şifre");
                return;
            }
            Session["userId"] = userRow.id; //kullanıcının linkle direkt girememesini sağlamak için. önce giriş yapıldığını kontrol eder. kontrol etmesek bile session o tarayıcıya ait session açar. sunucuda gezinme süresi vs. kendi tutar.
            Response.Redirect("~/main", false); //sessionu sıfırlar . sıfırlamaması için false. kullanıcı sunucu arası bağlantıyı bitirme.

        }
    }
}