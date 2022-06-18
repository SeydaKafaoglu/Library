using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;


namespace Library.Controllers
{
    public class UsersController : LibraryControllerController
    {
        // GET: Users
        public ActionResult Index()
        {

            Models.libraryTableAdapters.Users1TableAdapter users1Adapter = new Models.libraryTableAdapters.Users1TableAdapter();
            Models.library.Users1DataTable users1Table = new Models.library.Users1DataTable();
            //Models.library.Users1Row users1Row;

            users1Adapter.Fill(users1Table);
            ViewData["user1results"] = users1Table.Rows;
            return View();
            //Response.Redirect("~/main", false);

        }
        public ActionResult Add(string error = null)
        {
            ViewData["title"] = "Kullanıcı ekle";
            if (error == null)
            {
                ViewData["error"] = error;
            }
            return View();
        }
        public void ProcessNewUser(string userName, string password, string eMail)
        {
            MailAddress mailAddress;
            string hashedPassword;
            Models.libraryTableAdapters.UsersTableAdapter usersAdapter;

            if (userName == null)
            {
                Response.Redirect("~/users/add/error?= olamaz");
                return;
            }
            userName = userName.Trim();
            if (userName == "")
            {
                Response.Redirect("~/users/add");
                return;
            }
            if (userName.Length > 50)
            {
                Response.Redirect("~/users/add");
                return;

            }
            if (password == null)
            {
                Response.Redirect("~/users/add");
                return;
            }
            password = password.Trim();
            if (password == "")
            {
                Response.Redirect("~/users/add");
                return;

            }
            eMail = eMail.Trim();
            if (eMail.Length > 100)
            {
                Response.Redirect("~/users/add");
                return;
            }
            try
            {
                mailAddress = new MailAddress(eMail);
            }
            catch
            {
                Response.Redirect("~/users/add");
                return;
            }
            if (eMail != mailAddress.Address)
            {
                Response.Redirect("~/users/add");
                return;
            }
            hashedPassword = Utility.CalculateSHA(userName, password); //veritabanına yazılacak şifrelenmiş şifre

            try
            {
                usersAdapter = new Models.libraryTableAdapters.UsersTableAdapter();
                usersAdapter.AddUser(userName, hashedPassword, eMail);
                Response.Redirect("~/users");

            }
            catch
            {
                Response.Redirect("~/users/add");

            }

        }
        public void Delete(long id)
        {
            Models.libraryTableAdapters.UsersTableAdapter usersAdapter = new Models.libraryTableAdapters.UsersTableAdapter();
            usersAdapter.DeletedUser(id);
            Response.Redirect("~/users");
        }
        public ActionResult Update(long id)
        {
            Models.libraryTableAdapters.Users1TableAdapter users1Adapter = new Models.libraryTableAdapters.Users1TableAdapter();
            Models.library.Users1DataTable users1sTable = new Models.library.Users1DataTable();
            Models.library.Users1Row user1Row;

            users1Adapter.FillById(users1sTable, id);
            user1Row = (Models.library.Users1Row)users1sTable.Rows[0];
            ViewData["title"] = user1Row.UsersName;
            ViewData["userRow"] = user1Row;
            return View();

        }
        public void ProcessUser(long id, string userName, string password, string eMail)
        {
            MailAddress mailAddress;
            string hashedPassword;
            Models.libraryTableAdapters.UsersTableAdapter usersAdapter;
            bool changePassword = false;

            if (userName == null)
            {
                Response.Redirect("~/users/update/" + id.ToString());
                return;
            }
            userName = userName.Trim();
            if (userName == "")
            {
                Response.Redirect("~/users/update/" + id.ToString());
                return;
            }
            if (userName.Length > 50)
            {
                Response.Redirect("~/users/update/" + id.ToString());
                return;

            }
            if (password == null)
            {
                Response.Redirect("~/users/update/" + id.ToString());
                return;

            }
            if (password != "    ") //fake password.4 boşluklu
            {
                changePassword = true; //farklı gelirle değiştirilmesi gerekir. 4 space
                password = password.Trim();
                if (password == "")
                {
                    Response.Redirect("~/users/update/" + id.ToString());
                    return;
                }
            }

            eMail = eMail.Trim();
            if (eMail.Length > 100)
            {
                Response.Redirect("~/users/update/" + id.ToString());
                return;
            }
            try
            {
                mailAddress = new MailAddress(eMail);
            }
            catch
            {
                Response.Redirect("~/users/update/" + id.ToString());
                return;
            }
            if (eMail != mailAddress.Address)
            {
                Response.Redirect("~/users/update/" + id.ToString());
                return;
            }

            try
            {
                usersAdapter = new Models.libraryTableAdapters.UsersTableAdapter();
                if (changePassword ==true)
                {

                    hashedPassword = Utility.CalculateSHA(userName, password); //veritabanına yazılacak şifrelenmiş şifre
                    usersAdapter.UpdateUserWithPassword(userName, hashedPassword, eMail, id);
                }
                else
                {
                    usersAdapter.UpdateUserWithoutPassword(userName, eMail, id);
                }

                Response.Redirect("~/users");

            }
            catch
            {
                Response.Redirect("~/users/update/" + id.ToString());

            }

        }
    }
}