using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TempoHire.Dal.ContextClasses;
using TempoHire.Models;
using PagedList.Mvc;
using PagedList;

namespace TempoHire.Controllers
{
    public class PaidCustomersController : Controller
    {
        private MainDbContext db = new MainDbContext();

        // GET: PaidCustomers
       
        public ActionResult Index ( string sortOrder,string searchString,string currentFilter,int? page)



        {

            ViewBag.CustomerNameParam = string.IsNullOrEmpty(sortOrder) ? "customer_dec" :"";
            ViewBag.CompanyNameParam = string.IsNullOrEmpty(sortOrder) ? "company_dec" : "";
            ViewBag.PinCodeParam = string.IsNullOrEmpty(sortOrder) ? "pin_dec" : "";
            ViewBag.PaymentModeParam = string.IsNullOrEmpty(sortOrder) ? "mode_dec" : "";
            ViewBag.PlanNameParam = string.IsNullOrEmpty(sortOrder) ? "Plan_dec" : "";
            ViewBag.StationParam = string.IsNullOrEmpty(sortOrder) ? "station_dec" : "";
            ViewBag.VehicleTypeNameParam = string.IsNullOrEmpty(sortOrder) ? "vehicale_dec" : "";
            ViewBag.DateParam = string.IsNullOrEmpty(sortOrder) ? "date_dec" : "";
            ViewBag.ContactNumberParam = string.IsNullOrEmpty(sortOrder) ? "contact_dec" : "";
            ViewBag.LocationParam = string.IsNullOrEmpty(sortOrder) ? "location_dec" : "";
            ViewBag.AmountParam = string.IsNullOrEmpty(sortOrder) ? "amount_dec" : "";

            if(searchString!=null)
            {
                page = 1;
            }

            else
            {
                searchString = currentFilter;
            }
            var paidcustomers = from s in db.PaidCustomers select s;
            if(!string.IsNullOrEmpty(searchString))
            {

                paidcustomers = paidcustomers.Where(s => s.CompanyName.ToUpper().Contains(searchString.ToUpper()) || s.CustomerName.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {

                case "customer_dec":

                    paidcustomers = paidcustomers.OrderByDescending(s => s.CustomerName);
                    break;
                case "company_dec":

                    paidcustomers = paidcustomers.OrderByDescending(s => s.CompanyName);
                    break;

                case "pin_dec":

                    paidcustomers = paidcustomers.OrderByDescending(s => s.Pincode);
                    break;

                case "mode_dec":

                    paidcustomers = paidcustomers.OrderByDescending(s => s.PaymentMode);
                    break;


                case "Plan_dec":

                    paidcustomers = paidcustomers.OrderByDescending(s => s.Plan);
                    break;

                case "station_dec":

                    paidcustomers = paidcustomers.OrderByDescending(s => s.Station);
                    break;

                case "vehicale_dec":

                    paidcustomers = paidcustomers.OrderByDescending(s => s.Vehicle);
                    break;
                case "date_dec":

                    paidcustomers = paidcustomers.OrderByDescending(s => s.Date);
                    break;
                case "contact_dec":

                    paidcustomers = paidcustomers.OrderByDescending(s => s.ContactNumber);
                    break;
                case "location_dec":

                    paidcustomers = paidcustomers.OrderByDescending(s => s.Location);
                    break;
                case "amount_dec":

                    paidcustomers = paidcustomers.OrderByDescending(s => s.Amount);
                    break;


                default:
                    paidcustomers = paidcustomers.OrderBy(s => s.CompanyName);
                    break;



            }

           // ViewBag.Total = paidcustomers.Sum(e => e.Amount);
            int pageSize = 6;
            int pageNumber = (page ?? 1);
             var paidCustomers = db.PaidCustomers.Include(p => p.PaymentMode).Include(p => p.Pincode).Include(p => p.Plan).Include(p => p.Station).Include(p => p.Vehicle);
             return View(paidcustomers.ToPagedList(pageNumber,pageSize));

           // return View(paidcustomers.ToPagedList(pageNumber, pageSize));
        }

        // GET: PaidCustomers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaidCustomer paidCustomer = db.PaidCustomers.Find(id);
            if (paidCustomer == null)
            {
                return HttpNotFound();
            }
            return View(paidCustomer);
        }

        // GET: PaidCustomers/Create
        public ActionResult Create()
        {
            ViewBag.PaymentModeID = new SelectList(db.PaymentModes, "PaymentModeID", "PaymentModeName");
            ViewBag.PincodeID = new SelectList(db.Pincodes, "PincodeID", "PinCodeNumber");
            ViewBag.PlanID = new SelectList(db.Plans, "PlanID", "PlanName");
            ViewBag.StationID = new SelectList(db.Stations, "StationID", "StationName");
            ViewBag.VehicleID = new SelectList(db.Vehicles, "VehicleID", "VehicleTypeName");
            return View();
        }

        // POST: PaidCustomers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PaidCustomerID,Date,CustomerName,ContactNumber,CompanyName,PlanID,VehicleID,PaymentModeID,StationID,Location,PincodeID,Amount,AmountPanding")] PaidCustomer paidCustomer)
        {
            if (ModelState.IsValid)
            {
                db.PaidCustomers.Add(paidCustomer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PaymentModeID = new SelectList(db.PaymentModes, "PaymentModeID", "PaymentModeName", paidCustomer.PaymentModeID);
            ViewBag.PincodeID = new SelectList(db.Pincodes, "PincodeID", "PinCodeNumber", paidCustomer.PincodeID);
            ViewBag.PlanID = new SelectList(db.Plans, "PlanID", "PlanName", paidCustomer.PlanID);
            ViewBag.StationID = new SelectList(db.Stations, "StationID", "StationName", paidCustomer.StationID);
            ViewBag.VehicleID = new SelectList(db.Vehicles, "VehicleID", "VehicleTypeName", paidCustomer.VehicleID);
            return View(paidCustomer);
        }

        // GET: PaidCustomers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaidCustomer paidCustomer = db.PaidCustomers.Find(id);
            if (paidCustomer == null)
            {
                return HttpNotFound();
            }
            ViewBag.PaymentModeID = new SelectList(db.PaymentModes, "PaymentModeID", "PaymentModeName", paidCustomer.PaymentModeID);
            ViewBag.PincodeID = new SelectList(db.Pincodes, "PincodeID", "PinCodeNumber", paidCustomer.PincodeID);
            ViewBag.PlanID = new SelectList(db.Plans, "PlanID", "PlanName", paidCustomer.PlanID);
            ViewBag.StationID = new SelectList(db.Stations, "StationID", "StationName", paidCustomer.StationID);
            ViewBag.VehicleID = new SelectList(db.Vehicles, "VehicleID", "VehicleTypeName", paidCustomer.VehicleID);
            return View(paidCustomer);
        }

        // POST: PaidCustomers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PaidCustomerID,Date,CustomerName,ContactNumber,CompanyName,PlanID,VehicleID,PaymentModeID,StationID,Location,PincodeID,Amount,AmountPanding")] PaidCustomer paidCustomer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paidCustomer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PaymentModeID = new SelectList(db.PaymentModes, "PaymentModeID", "PaymentModeName", paidCustomer.PaymentModeID);
            ViewBag.PincodeID = new SelectList(db.Pincodes, "PincodeID", "PinCodeNumber", paidCustomer.PincodeID);
            ViewBag.PlanID = new SelectList(db.Plans, "PlanID", "PlanName", paidCustomer.PlanID);
            ViewBag.StationID = new SelectList(db.Stations, "StationID", "StationName", paidCustomer.StationID);
            ViewBag.VehicleID = new SelectList(db.Vehicles, "VehicleID", "VehicleTypeName", paidCustomer.VehicleID);
            return View(paidCustomer);
        }

        // GET: PaidCustomers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaidCustomer paidCustomer = db.PaidCustomers.Find(id);
            if (paidCustomer == null)
            {
                return HttpNotFound();
            }
            return View(paidCustomer);
        }

        // POST: PaidCustomers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PaidCustomer paidCustomer = db.PaidCustomers.Find(id);
            db.PaidCustomers.Remove(paidCustomer);
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
