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
    [RequireHttps]
    [Authorize(Roles = "Admin,MRIServiceProvider")]
    public class MRIServiceProvidersController : Controller
    {
        public ActionResult SetSecureCookie()
        {
            var cookie = new HttpCookie("MyCookie");
            cookie.Value = "cookieValue";

            // Set the Secure and HttpOnly flags
            cookie.Secure = true;
            cookie.HttpOnly = true;

            // Add the cookie to the response
            Response.Cookies.Add(cookie);

            return View();
        }

        private FIT5032_PortfolioEntities db = new FIT5032_PortfolioEntities();

        // GET: MRIServiceProviders/Index
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.MRIServiceProviders.ToList());
        }

        // GET: MRIServiceProviders/Index
        public ActionResult Manage()
        {
            return View(db.MRIServiceProviders.ToList());
        }

        // GET: MRIServiceProviders/MRIView
        public ActionResult MRIView()
        {
            var userId = User.Identity.GetUserId();

            var mriServiceProviders = db.MRIServiceProviders.Where(m => m.UserId == userId).ToList();
            return View(mriServiceProviders);
        }

        // GET: MRIServiceProviders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MRIServiceProvider mRIServiceProvider = db.MRIServiceProviders.Find(id);
            if (mRIServiceProvider == null)
            {
                return HttpNotFound();
            }
            return View(mRIServiceProvider);
        }

        // GET: MRIServiceProviders/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: MRIServiceProviders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Name,Postcode,Address,Email,Phone,Open,Close,Lat,Long")] MRIServiceProvider mRIServiceProvider)
        {
            mRIServiceProvider.UserId = User.Identity.GetUserId();

            ModelState.Clear();
            TryValidateModel(mRIServiceProvider);

            if (ModelState.IsValid)
            {
                db.MRIServiceProviders.Add(mRIServiceProvider);
                db.SaveChanges();
                return RedirectToAction("Manage");
            }

            return View(mRIServiceProvider);
        }

        // GET: MRIServiceProviders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MRIServiceProvider mRIServiceProvider = db.MRIServiceProviders.Find(id);
            if (mRIServiceProvider == null)
            {
                return HttpNotFound();
            }
            return View(mRIServiceProvider);
        }

        // POST: MRIServiceProviders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Postcode,Address,Email,Phone,Open,Close,Lat,Long,UserId")] MRIServiceProvider mRIServiceProvider)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mRIServiceProvider).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Manage");
            }
            return View(mRIServiceProvider);
        }

        // GET: MRIServiceProviders/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MRIServiceProvider mRIServiceProvider = db.MRIServiceProviders.Find(id);
            if (mRIServiceProvider == null)
            {
                return HttpNotFound();
            }
            return View(mRIServiceProvider);
        }

        // POST: MRIServiceProviders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            MRIServiceProvider mRIServiceProvider = db.MRIServiceProviders.Find(id);
            db.MRIServiceProviders.Remove(mRIServiceProvider);
            db.SaveChanges();
            return RedirectToAction("Manage");
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
