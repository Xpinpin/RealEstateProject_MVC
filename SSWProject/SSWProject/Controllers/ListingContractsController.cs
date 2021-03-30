using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SSWProject.Models;

namespace SSWProject.Controllers
{
    [Authorize]
    public class ListingContractsController : Controller
    {
        private RealEstateContext db = new RealEstateContext();

        // GET: ListingContracts
        public ActionResult Index()
        {
            var listingContracts = db.ListingContracts.Include(l => l.Agent).Include(l => l.Listing);

            return View(listingContracts.ToList());
        }

        public ActionResult ViewingListingsFromCreate()
        {
            return RedirectToAction("Index", "Listings");
        }

        public ActionResult ViewListings(string customerID)
        {
            Session["customerID"] = customerID;
            return RedirectToAction("Index", "Listings");
        }

        // GET: ListingContracts/Details/5
        public ActionResult Details(int? contractId)
        {
            if (contractId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListingContract listingContract = db.ListingContracts.Find(contractId);
            ViewBag.CustomerName = db.Listings.Find(contractId).CustomerName;
            if (listingContract == null)
            {
                return HttpNotFound();
            }
            return View(listingContract);
        }

        // GET: ListingContracts/Create
        public ActionResult Create()
        {
            ViewBag.AgentId = new SelectList(db.Agents, "ID", "AgentName");
            //ViewBag.ListingId = new SelectList(db.Listings, "ListingID", "StreetAddress");
            ViewBag.CustomerName = db.Customers.Find(Convert.ToInt32(Session["customerID"])).CustomerName;
            ViewBag.ListingAddress = db.Listings.Find(Convert.ToInt32(Session["ListingID"])).StreetAddress;
            ViewBag.ListingId = Convert.ToInt32(Session["ListingID"]);
            ViewBag.StartDate = DateTime.Now.ToShortDateString();
            ViewBag.EndDate = DateTime.Now.AddMonths(3).ToShortDateString();
            return View();
        }

        // POST: ListingContracts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContractId,AgentId,ListingId,StartDate,EndDate,IsSigned")] ListingContract listingContract)
        {
            if (ModelState.IsValid)
            {
                if (db.ListingContracts.Any(c => c.ListingId == listingContract.ListingId
                 && c.EndDate.CompareTo(listingContract.StartDate) > 0))
                {
                    ViewBag.AgentId = new SelectList(db.Agents, "ID", "AgentName");
                    //ViewBag.ListingId = new SelectList(db.Listings, "ListingID", "StreetAddress", listingContract.ListingId);
                    ViewBag.CustomerName = db.Customers.Find(Convert.ToInt32(Session["customerID"])).CustomerName;
                    ViewBag.ListingAddress = db.Listings.Find(Convert.ToInt32(Session["ListingID"])).StreetAddress;
                    ViewBag.ListingId = Convert.ToInt32(Session["ListingID"]);
                    ViewBag.StartDate = DateTime.Now.ToShortDateString();
                    ViewBag.EndDate = DateTime.Now.AddMonths(3).ToShortDateString();
                    ViewBag.ErrMsg = "This listing is still in valid time of a contract. Please try again at later time.";
                    return View(listingContract);
                }
                db.ListingContracts.Add(listingContract);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AgentId = new SelectList(db.Agents, "ID", "AgentName");
            //ViewBag.ListingId = new SelectList(db.Listings, "ListingID", "StreetAddress", listingContract.ListingId);
            ViewBag.CustomerName = db.Customers.Find(Convert.ToInt32(Session["customerID"])).CustomerName;
            ViewBag.ListingAddress = db.Listings.Find(Convert.ToInt32(Session["ListingID"])).StreetAddress;
            ViewBag.ListingId = Convert.ToInt32(Session["ListingID"]);
            ViewBag.StartDate = DateTime.Now.Date;
            ViewBag.EndDate = DateTime.Now.AddMonths(3).Date;
            return View(listingContract);
        }

        // GET: ListingContracts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListingContract listingContract = db.ListingContracts.Find(id);
            if (listingContract == null)
            {
                return HttpNotFound();
            }
            ViewBag.AgentId = new SelectList(db.Agents, "ID", "FirstName", listingContract.AgentId);
            ViewBag.ListingId = new SelectList(db.Listings, "ListingID", "StreetAddress", listingContract.ListingId);
            return View(listingContract);
        }

        // POST: ListingContracts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AgentId,ListingId,SalesPrice,StartDate,EndDate,IsSigned")] ListingContract listingContract)
        {
            if (ModelState.IsValid)
            {
                db.Entry(listingContract).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AgentId = new SelectList(db.Agents, "ID", "FirstName", listingContract.AgentId);
            ViewBag.ListingId = new SelectList(db.Listings, "ListingID", "StreetAddress", listingContract.ListingId);
            return View(listingContract);
        }

        // GET: ListingContracts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListingContract listingContract = db.ListingContracts.Find(id);
            if (listingContract == null)
            {
                return HttpNotFound();
            }
            return View(listingContract);
        }

        // POST: ListingContracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ListingContract listingContract = db.ListingContracts.Find(id);
            db.ListingContracts.Remove(listingContract);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
