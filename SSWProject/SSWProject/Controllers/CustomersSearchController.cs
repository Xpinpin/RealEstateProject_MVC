using SSWProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSWProject.Controllers
{
    [Authorize]
    public class CustomersSearchController : Controller
    {
        // GET: CustomersSearch
        RealEstateContext db = new RealEstateContext();

        // GET: CustomersSearch
        [HttpGet]
        public ActionResult Index(int? customerNumber, string name, string phoneNumber)
        {
            var query = db.Customers.AsQueryable();
            if (customerNumber.HasValue)
            {
                query = query.Where(q => q.ID == customerNumber);
            }
            if (!String.IsNullOrEmpty(name))
            {
                query = query.Where(q => q.FirstName == name || q.LastName == name || q.FirstName + " " + q.LastName == name);
            }

            if (!String.IsNullOrEmpty(phoneNumber))
            {
                query = query.Where(q => q.CellPhoneNumber == phoneNumber);
            }
            ViewBag.CusID = new SelectList(query, "ID", "CustomerName");

            return View(query);
        }

        [HttpPost]
        public ActionResult Index(string customerID)
        {
            if (customerID == "")
            {
                var query = db.Customers.AsQueryable();

                ViewBag.CusID = new SelectList(query, "ID", "CustomerName");
                ViewBag.ErrorSelect = "You must select a customer to load the listings";
                return View(query);
            }
            Session["customerID"] = customerID;

            return RedirectToAction("Index", "Listings");
        }

    }
}