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
using Newtonsoft.Json;

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

        [Authorize(Roles = "MRIServiceProvider")]
        public ActionResult MRIView()
        {
            var userId = User.Identity.GetUserId();
            var mri = db.MRIServiceProviders.Where(m => m.UserId == userId).ToList();
            var mriId = mri.FirstOrDefault()?.Id;

            var appointments = db.Appointments.Where(a => a.MriId == mriId).ToList();

            var labels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

            int[] count = new int[12];
            foreach (var item in appointments)
            {
                if (item.Date.Year == DateTime.Now.Year)
                {
                    switch (item.Date.Month)
                    {
                        case 1:
                            count[0] += 1;
                            break;
                        case 2:
                            count[1] += 1;
                            break;
                        case 3:
                            count[2] += 1;
                            break;
                        case 4:
                            count[3] += 1;
                            break;
                        case 5:
                            count[4] += 1;
                            break;
                        case 6:
                            count[5] += 1;
                            break;
                        case 7:
                            count[6] += 1;
                            break;
                        case 8:
                            count[7] += 1;
                            break;
                        case 9:
                            count[8] += 1;
                            break;
                        case 10:
                            count[9] += 1;
                            break;
                        case 11:
                            count[10] += 1;
                            break;
                        default:
                            count[11] += 1;
                            break;
                    }
                }
            }

            ViewBag.ChartLabels = JsonConvert.SerializeObject(labels);
            ViewBag.ChartData = JsonConvert.SerializeObject(count);

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

                string attachmentPath = Server.MapPath("~/Attachment/mri_report.pdf");

                Attachment newAttach = new Attachment(attachmentPath);
                email.Attachments.Add(newAttach);

                using (SmtpClient client = new SmtpClient(SmtpServer, SmtpPort))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(GoogleID, TempPwd);
                    client.Send(email);
                }

                ViewBag.Result = "MRI Result has been send to your email.";
                return View();
            }
            catch
            {
                ViewBag.Result = "Failed to send result to your email.";
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
                if (!IsConflict(appointment))
                {
                    db.Appointments.Add(appointment);
                    db.SaveChanges();
                    ViewBag.Result = "Appointment Created.";
                    ViewBag.MriId = new SelectList(db.MRIServiceProviders, "Id", "Name", appointment.MriId);
                    return View(); 
                }
                else
                {
                    ViewBag.Result = "Booking not available. \nPlease try again.";
                    ViewBag.MriId = new SelectList(db.MRIServiceProviders, "Id", "Name", appointment.MriId);
                    return View(); ;
                }
            }

            ViewBag.MriId = new SelectList(db.MRIServiceProviders, "Id", "Name", appointment.MriId);
            return View();
        }

        private bool IsConflict(Appointment appointment)
        {
            var existingAppointments = db.Appointments.ToList();

            foreach (var existingAppointment in existingAppointments)
            {
                if (IsOverlap(existingAppointment, appointment))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsOverlap(Appointment existingAppointment, Appointment appointment)
        {
            if (existingAppointment.MriId == appointment.MriId)
            {
                if (existingAppointment.Date == appointment.Date)
                {
                    if (existingAppointment.Time == appointment.Time)
                    {
                        return true;
                    }
                }
            }
            return false;
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
                ViewBag.Result = "Appointment changed.";
                ViewBag.MriId = new SelectList(db.MRIServiceProviders, "Id", "Name", appointment.MriId);
                return View(appointment);
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
            ViewBag.Result = "Appointment deleted.";
            return View();
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
