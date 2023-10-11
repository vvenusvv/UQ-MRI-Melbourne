using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FIT5032_Portfolio.Models;
using Microsoft.AspNet.Identity;

namespace FIT5032_Portfolio.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private FIT5032_PortfolioEntities db = new FIT5032_PortfolioEntities();

        // GET: Reviews
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var reviews = db.Reviews.Where(r => r.UserId == userId).ToList();
            return View(reviews);
        }

        [Authorize(Roles = "MRIServiceProvider")]
        public ActionResult MRIView()
        {
            var reviews = db.Reviews.ToList();

            return View(reviews);
        }

        // GET: Reviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // GET: Reviews/Create
        public ActionResult Create()
        {
            ViewBag.AppointmentId = new SelectList(db.Appointments, "Id", "UserId");
            ViewBag.MriId = new SelectList(db.MRIServiceProviders, "Id", "Name");
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Rating,Comment,MriId")] Review review, int id)
        {
            review.UserId = User.Identity.GetUserId();
            review.AppointmentId = id;

            ModelState.Clear();
            TryValidateModel(review);

            if (ModelState.IsValid)
            {
                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MriId = new SelectList(db.MRIServiceProviders, "Id", "Name");
            return View(review);
        }

        // GET: Reviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            ViewBag.AppointmentId = new SelectList(db.Appointments, "Id", "UserId", review.AppointmentId);
            ViewBag.MriId = new SelectList(db.MRIServiceProviders, "Id", "Name");
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Rating,Comment,MriId")] Review review)
        {
            review.UserId = User.Identity.GetUserId();

            ModelState.Clear();
            TryValidateModel(review);

            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MriId = new SelectList(db.MRIServiceProviders, "Id", "Name");
            return View(review);
        }

        // GET: Reviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
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
