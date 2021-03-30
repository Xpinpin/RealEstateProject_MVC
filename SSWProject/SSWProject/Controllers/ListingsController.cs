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
    public class ListingsController : Controller
    {
        private RealEstateContext db = new RealEstateContext();
        public ActionResult Index()
        {
            int customerID = db.Customers.Find(Convert.ToInt32(Session["customerID"])).ID;
            var listings = db.Listings.Where(l => l.CustomerID == customerID);
            ViewBag.CustomerName = db.Customers.Find(Convert.ToInt32(Session["customerID"])).CustomerName;
            return View(listings.ToList());
        }

        // GET: Listings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Listing listing = db.Listings.Include(c => c.Files).SingleOrDefault(c => c.ListingID == id);
            if (listing == null)
            {
                return HttpNotFound();
            }
            return View(listing);
        }

        // GET: Listings/Create
        public ActionResult Create()
        {
            //ViewBag.CustomerID = new SelectList(db.Customers, "ID", "FirstName");
            ViewBag.CustomerID = db.Customers.Find(Convert.ToInt32(Session["customerID"])).ID;
            ViewBag.CustomerName = db.Customers.Find(Convert.ToInt32(Session["customerID"])).CustomerName;
            return View();
        }

        // POST: Listings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ListingID,StreetAddress,Municipality,province,OtherAddress,SquareFootage,BedsNum,BathsNum,StoriesNum,CityArea,FeaturesSummary,HeatingType,SalesPrice,CustomerID")] Listing listing)
        {
            if (ModelState.IsValid)
            {
                db.Listings.Add(listing);
                db.SaveChanges();
                //Session["ListingID"] = listing.ListingID;
                return RedirectToAction("Index");
                //return RedirectToAction("Create","ListingContracts");
            }

            ViewBag.CustomerID = db.Customers.Find(Convert.ToInt32(Session["customerID"])).ID;

            //ViewBag.CustomerID = new SelectList(db.Customers, "ID", "FirstName", listing.CustomerID);
            return View(listing);
        }

        // GET: Listings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Listing listing = db.Listings.Find(id);
            if (listing == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = db.Customers.Find(Convert.ToInt32(Session["customerID"])).ID;
            return View(listing);
        }

        // POST: Listings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ListingID,StreetAddress,Municipality,province,OtherAddress,SquareFootage,BedsNum,BathsNum,StoriesNum,CityArea,FeaturesSummary,HeatingType,SalesPrice,CustomerID")] Listing listing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(listing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = db.Customers.Find(Convert.ToInt32(Session["customerID"])).ID;
            return View(listing);
        }

        public ActionResult SignContract(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Listing listing = db.Listings.Find(id);
            if (listing == null)
            {
                return HttpNotFound();
            }
            Session["ListingID"] = listing.ListingID;
            return RedirectToAction("Create", "ListingContracts");
        }

        // GET: Listings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Listing listing = db.Listings.Find(id);
            if (listing == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = db.Customers.Find(Convert.ToInt32(Session["customerID"])).ID;

            return View(listing);
        }

        // POST: Listings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Listing listing = db.Listings.Find(id);
            db.Listings.Remove(listing);
            ViewBag.CustomerID = db.Customers.Find(Convert.ToInt32(Session["customerID"])).ID;

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
