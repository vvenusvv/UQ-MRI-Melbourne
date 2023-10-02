using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FIT5032_Portfolio.Models;
using Microsoft.AspNet.Identity;

namespace FIT5032_Portfolio.Controllers
{
    [RequireHttps]
    [Authorize]
    public class AppointmentsController : Controller
    {
        private FIT5032_PortfolioEntities db = new FIT5032_PortfolioEntities();

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

        // GET: Appointments
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var appointments = db.Appointments.Where(a=> a.UserId == userId).ToList();
            return View(appointments);
        }

        public ActionResult Calendar()
        {
            var userId = User.Identity.GetUserId();

            var appointments = db.Appointments.Where(a => a.UserId == userId).ToList();
            return View(appointments);
        }

        public ActionResult RetrieveResult(int? id)
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

        [HttpPost, ActionName("RetrieveResult")]
        [ValidateAntiForgeryToken]
        public ActionResult SendResult()
        {
            try
            {
                string toEmail = User.Identity.GetUserName();
                string subject = "MRI Result";
                string contents = "Enclosed is your MRI result. \n\nThank you for choosing UQ MRI Melbourne.";

                string GoogleID = "portfolio.use.personal@gmail.com";
                string TempPwd = "ewfufqmsrwuakkoj";

                string SmtpServer = "smtp.gmail.com";
                int SmtpPort = 587;

                MailMessage email = new MailMessage();
                email.From = new MailAddress(GoogleID);
                email.Subject = subject;
                email.Body = contents;
                email.IsBodyHtml = true;
                email.SubjectEncoding = Encoding.UTF8;
                email.To.Add(new MailAddress(toEmail));

                string attachmentPath = Server.MapPath("~/Attachment/result.txt");

                Attachment newAttach = new Attachment(attachmentPath);
                email.Attachments.Add(newAttach);

                using (SmtpClient client = new SmtpClient(SmtpServer, SmtpPort))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(GoogleID, TempPwd);
                    client.Send(email);
                }

                ViewBag.Message = "MRI Result has been send to your email.";
                return View();
            }
            catch
            {
                ViewBag.Message = "Failed to send result to your email.";
                return View();
            }
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
        public ActionResult Create()
        {
            ViewBag.MriId = new SelectList(db.MRIServiceProviders, "Id", "Name");
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,Time,MriId")] Appointment appointment)
        {
            appointment.UserId = User.Identity.GetUserId();

            ModelState.Clear();
            TryValidateModel(appointment);
            
            if (ModelState.IsValid)
            {
                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MriId = new SelectList(db.MRIServiceProviders, "Id", "Name", appointment.MriId);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
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
            ViewBag.MriId = new SelectList(db.MRIServiceProviders, "Id", "Name", appointment.MriId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,Time,MriId")] Appointment appointment)
        {
            appointment.UserId = User.Identity.GetUserId();

            ModelState.Clear();
            TryValidateModel(appointment);

            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MriId = new SelectList(db.MRIServiceProviders, "Id", "Name", appointment.MriId);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
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
