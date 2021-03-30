using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using SSWProject.Models;

namespace SSWProject.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private RealEstateContext db = new RealEstateContext();

        //GET: Appointments
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            //var appointments = db.Appointments.Include(a => a.Agents).Include(a => a.Customers);

            return View(db.Appointments.ToList());
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(string agent, DateTime? date)
        {
            //var appointments = db.Appointments.Include(a => a.Agents).Include(a => a.Customers).OrderBy(x => x.ID);
            if (string.IsNullOrEmpty(agent) && date == null)
            {
                TempData["Empty"] = "The fields cannot be empty";
                return View(db.Appointments.ToList());
            }
            else
            {
                if (!string.IsNullOrEmpty(agent) && date != null)
                {
                    var appointments = db.Appointments.Where(b => b.Agents.FirstName.ToLower().Contains(agent.ToLower()) && date == b.Date).OrderBy(b => b.ID);

                    return View(appointments.ToList());
                }
                else if (!string.IsNullOrEmpty(agent))
                {
                    var appointments = db.Appointments.Where(b => b.Agents.FirstName.ToLower().Contains(agent.ToLower())).OrderBy(b => b.ID);

                    return View(appointments.ToList());
                }
               else if (date != null)
                {
                    var appointments = db.Appointments.Where(b => date == b.Date).OrderBy(b => b.ID);

                    return View(appointments.ToList());
                }

            }
            return View(db.Appointments.ToList());

        }

        // GET: Appointments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // GET: Appointments/Create
        [Authorize]
        public ActionResult Create()
        {
            //ViewBag.AgentID = new SelectList(db.Agents, "ID", "FirstName");
            //ViewBag.CustomerID = new SelectList(db.Customers, "ID", "FirstName");
            Appointment appt = new Appointment();
            appt.GetCustomersList = db.Customers.ToList();
            appt.GetAgentsList = db.Agents.ToList();

            return View(appt);
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "ID,AgentID,CustomerID,Date,StartTime,EndTime,Comment")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                if (appointment.Date.CompareTo(DateTime.Now) < 0)
                {
                    appointment.GetCustomersList = db.Customers.ToList();
                    appointment.GetAgentsList = db.Agents.ToList();
                    TempData["ErrMessage"] = "Booking Date cannot be in the past";
                    return View(appointment);
                }
                if (db.Appointments.Any(b => b.AgentID == appointment.AgentID && b.Date == appointment.Date && (appointment.StartTime >= b.StartTime && appointment.StartTime <= b.EndTime)))
                {
                    TempData["ErrMessage"] = "Unfortunately, this time is not available, please choose another time";
                    return RedirectToAction("Index");
                }
                else if (db.Appointments.Any(b => b.AgentID == appointment.AgentID && b.Date == appointment.Date && (appointment.EndTime >= b.StartTime && appointment.EndTime <= b.EndTime)))
                {
                    TempData["ErrMessage"] = "Unfortunately, this time is not available, please choose another time";
                    return RedirectToAction("Index");
                }
                else if (db.Appointments.Any(b => b.AgentID == appointment.AgentID && b.Date == appointment.Date && (appointment.StartTime <= EntityFunctions.AddMinutes(b.EndTime, 15))))
                {
                    TempData["ErrMessage"] = "Unfortunately, this time is not available, please choose another time";
                    return RedirectToAction("Index");
                }
                TempData["ErrMessage"] = "Booked Successfully!";

                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            appointment.GetCustomersList = db.Customers.ToList();
            appointment.GetAgentsList = db.Agents.ToList();
            return View(appointment);

        }

        // GET: Appointments/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            ViewBag.AgentID = new SelectList(db.Agents, "ID", "FirstName", appointment.AgentID);
            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "FirstName", appointment.CustomerID);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ID,AgentID,CustomerID,Date,StartTime,EndTime,Comment")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {

                if (db.Appointments.Any(b => b.AgentID == appointment.AgentID && b.Date == appointment.Date && (appointment.StartTime >= b.StartTime && appointment.StartTime <= b.EndTime)))
                {
                    TempData["Message"] = "This time is not available, please choose another time";
                    return RedirectToAction("Index");
                }
                else if (db.Appointments.Any(b => b.AgentID == appointment.AgentID && b.Date == appointment.Date && (appointment.EndTime >= b.StartTime && appointment.EndTime <= b.EndTime)))
                {
                    TempData["Message"] = "This time is not available, please choose another time";
                    return RedirectToAction("Index");
                }
                else if (db.Appointments.Any(b => b.AgentID == appointment.AgentID && b.Date == appointment.Date && (appointment.StartTime <= EntityFunctions.AddMinutes(b.EndTime, 15))))
                {
                    TempData["Message"] = "This time is not available, please choose another time";
                    return RedirectToAction("Index");
                }
                TempData["Message"] = "Successfully";
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AgentID = new SelectList(db.Agents, "ID", "FirstName", appointment.AgentID);
            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "FirstName", appointment.CustomerID);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            db.Appointments.Remove(appointment);
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
