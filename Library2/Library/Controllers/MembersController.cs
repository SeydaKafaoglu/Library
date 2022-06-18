using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;

namespace Library.Controllers
{
    public class MembersController : LibraryControllerController
    {
        // GET: Members
        public ActionResult Index()
        {
            Models.libraryTableAdapters.MembersTableAdapter membersAdapter = new Models.libraryTableAdapters.MembersTableAdapter();
            Models.library.MembersDataTable membersTable = new Models.library.MembersDataTable();


            membersAdapter.Fill(membersTable);

            ViewData["members"] = membersTable.Rows;
            ViewData["title"] = "Üyeler";
            return View();
        }
        public ActionResult Update(long id, string error = null)
        {
            Models.libraryTableAdapters.MembersDetailsTableAdapter membersDetailsAdapter = new Models.libraryTableAdapters.MembersDetailsTableAdapter();
            Models.library.MembersDetailsDataTable membersDetailsTable = new Models.library.MembersDetailsDataTable();
            Models.library.MembersDetailsRow membersDetailsRow;

            membersDetailsAdapter.Fill(membersDetailsTable, id);
            membersDetailsRow = (Models.library.MembersDetailsRow)membersDetailsTable.Rows[0];
            ViewData["title"] = membersDetailsRow.memberName + "" + membersDetailsRow.memberSurname;
            ViewData["members"] = membersDetailsRow;
            if (error == null)
            {
                ViewData["error"] = error;
            }
            return View();
        }
        public void ProcessMembers(long originalId, long memberId, string memberName, string memberSurname, string eMail, long phone, string address, bool banned=false, bool? gender=null)
        {
            Models.libraryTableAdapters.MembersDetailsTableAdapter membersAdapter;
        
            MailAddress mailAddress;

            if (originalId.ToString().Length != 11)
            {
                Response.Redirect("~/members/update/" + originalId.ToString());
                return;
            }
            if (memberName == null)
            {
                Response.Redirect("~/members/update/" + originalId.ToString());
                return;
            }
            memberName = memberName.Trim();
            if (memberName == "")
            {
                Response.Redirect("~/members/update/" + originalId.ToString());
                return;
            }
            if (memberName.Length > 50)
            {
                Response.Redirect("~/members/update/" + originalId.ToString());
                return;

            }
            if (memberSurname == null)
            {
                Response.Redirect("~/members/update/" + originalId.ToString());
                return;
            }
            memberSurname = memberSurname.Trim();
            if (memberSurname == "")
            {
                Response.Redirect("~/members/update/" + originalId.ToString());
                return;
            }
            if (memberSurname.Length > 50)
            {
                Response.Redirect("~/members/update/" + originalId.ToString());
                return;

            }
            if (phone.ToString().Length != 10)
            {
                Response.Redirect("~/members/update/" + originalId.ToString());
                return;
            }
            if (eMail == null)
            {
                Response.Redirect("~/members/update/" + originalId.ToString());
                return;
            }
            eMail = eMail.Trim();
            if (eMail.Length > 100)
            {
                Response.Redirect("~/members/update/" + originalId.ToString());
                return;
            }
            try
            {
                mailAddress = new MailAddress(eMail);
            }
            catch
            {
                Response.Redirect("~/members/update/" + originalId.ToString());
                return;
            }
            if (eMail != mailAddress.Address)
            {
                Response.Redirect("~/members/update/" + originalId.ToString());
                return;
            }
            if (address == null)
            {
                Response.Redirect("~/members/update/" + originalId.ToString());
                return;
            }
            address = address.Trim();
            if (address == "")
            {
                Response.Redirect("~/members/update/" + originalId.ToString());
                return;
            }
            if (address.Length > 200)
            {
                Response.Redirect("~/members/update/" + originalId.ToString());
                return;

            }


            try
            {
                membersAdapter = new Models.libraryTableAdapters.MembersDetailsTableAdapter();
                membersAdapter.UpdateMember(memberId, memberName, memberSurname, phone, eMail, gender, banned, address, originalId);
                Response.Redirect("~/members");

            }
            catch(Exception ex)
            {
                Response.Redirect("~/members/update/" + originalId.ToString());

            }
        }
        public ActionResult MemberData(long id)
        {
            Models.libraryTableAdapters.MembersDetailsTableAdapter membersDetailsAdapter = new Models.libraryTableAdapters.MembersDetailsTableAdapter();
            Models.library.MembersDetailsDataTable membersDetailsTable = new Models.library.MembersDetailsDataTable();

            membersDetailsAdapter.Fill(membersDetailsTable, id);
            if (membersDetailsTable.Rows.Count > 0)
            {
                ViewData["membersRow"] = (Models.library.MembersDetailsRow)membersDetailsTable.Rows[0];
            }
          
            return PartialView(); //tam sayfa değil parça gönder.
        }
        public bool MemberExists(long id)
        {
            Models.libraryTableAdapters.MembersTableAdapter membersAdapter = new Models.libraryTableAdapters.MembersTableAdapter();
            int memberCount = membersAdapter.MemberCount(id).Value;
            if (memberCount ==0)
            {
                return false;
            }
            return true;
        }
        public void ProcessNewMember(long id, string memberName, string memberSurname, string eMail, long phone, string address, bool? gender)
        {
            Models.libraryTableAdapters.MembersTableAdapter membersTableAdapter = new Models.libraryTableAdapters.MembersTableAdapter();
            membersTableAdapter.AddMember(id, memberName, memberSurname, phone, eMail, gender, address);
        }
    }
}