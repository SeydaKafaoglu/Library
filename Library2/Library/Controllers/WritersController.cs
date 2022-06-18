using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class WritersController : LibraryControllerController
    {
        // GET: Writers
        public ActionResult Index()
        {
            Models.libraryTableAdapters.WriterTableAdapter writerAdapter = new Models.libraryTableAdapters.WriterTableAdapter();
            Models.library.WriterDataTable writerTable = new Models.library.WriterDataTable();
            //Models.library.Users1Row users1Row;

            writerAdapter.Fill(writerTable);
            ViewData["writerResults"] = writerTable.Rows;
            return View();
        }
        public ActionResult Add(string error = null)
        {

            ViewData["title"] = "Yazar ekle";
            if (error == null)
            {
                ViewData["error"] = error;
            }
            return View();

        }
        public void ProcessNewWriter(string writerName, string surname, string AKA)
        {

            Models.libraryTableAdapters.WriterTableAdapter writerAdapter;

            if (writerName == null)
            {
                Response.Redirect("~/writers/add");
                return;
            }
            writerName = writerName.Trim();
            if (writerName == "")
            {
                Response.Redirect("~/writers/add");
                return;
            }
            if (writerName.Length > 100)
            {
                Response.Redirect("~/writers/add");
                return;

            }
            if (surname != null)
            {
                surname = surname.Trim();
                if (surname == "")
                {
                    surname = null;


                }
                else
                {
                    if (surname.Length > 50)
                    {
                        Response.Redirect("~/writers/add");
                        return;

                    }
                }

            }
            if (AKA != null)
            {
                AKA = AKA.Trim();
                if (AKA == "")
                {
                    AKA = null;

                }
                else
                {
                    if (AKA.Length > 50)
                    {
                        Response.Redirect("~/writers/add");
                        return;

                    }
                }

            }

            try
            {
                writerAdapter = new Models.libraryTableAdapters.WriterTableAdapter();
                writerAdapter.AddWriter(writerName, surname, AKA);
                Response.Redirect("~/writers");

            }
            catch
            {
                Response.Redirect("~/writers/add");

            }

        }
        public void Delete(long id)
        {
            Models.libraryTableAdapters.WriterTableAdapter writerAdapter = new Models.libraryTableAdapters.WriterTableAdapter();
            writerAdapter.DeletedWriter(id);
            Response.Redirect("~/writers");
        }
        public ActionResult Update(long id)
        {
            Models.libraryTableAdapters.WriterTableAdapter writerAdapter = new Models.libraryTableAdapters.WriterTableAdapter();
            Models.library.WriterDataTable writerTable = new Models.library.WriterDataTable();
            Models.library.WriterRow writerRow;

            writerAdapter.FillBy(writerTable, id);
            writerRow = (Models.library.WriterRow)writerTable.Rows[0];
            ViewData["title"] = writerRow.Name;
            ViewData["writerRow"] = writerRow;
            return View();

        }
        public void ProcessWriter(long id,string writerName, string surname, string AKA)
        {
            Models.libraryTableAdapters.WriterTableAdapter writerAdapter;

            if (writerName == null)
            {
                Response.Redirect("~/writers/update/" + id.ToString());
                return;
            }
            writerName = writerName.Trim();
            if (writerName == "")
            {
                Response.Redirect("~/writers/update/" + id.ToString());
                return;
            }
            if (writerName.Length > 100)
            {
                Response.Redirect("~/writers/update/" + id.ToString());
                return;

            }
            if (surname != null)
            {
                surname = surname.Trim();
                if (surname == "")
                {
                    surname = null;


                }
                else
                {
                    if (surname.Length > 50)
                    {
                        Response.Redirect("~/writers/update/" + id.ToString());
                        return;

                    }
                }

            }
            if (AKA != null)
            {
                AKA = AKA.Trim();
                if (AKA == "")
                {
                    AKA = null;

                }
                else
                {
                    if (AKA.Length > 50)
                    {
                        Response.Redirect("~/writers/update/" + id.ToString());
                        return;

                    }
                }

            }

            try
            {
                writerAdapter = new Models.libraryTableAdapters.WriterTableAdapter();
                writerAdapter.UpdateWriter(writerName, surname, AKA, id);
                Response.Redirect("~/writers");

            }
            catch
            {
                Response.Redirect("~/writers/update/" + id.ToString());

            }

        }
    }
}