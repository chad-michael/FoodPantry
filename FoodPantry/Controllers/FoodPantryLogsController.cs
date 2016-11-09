using FoodPantry.Core.Services;
using FoodPantry.Data.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Configuration;
using System.Web.Mvc;

namespace FoodPantry.Controllers
{
    [Authorize(Roles = "FoodPantry_Users")]
    public class FoodPantryLogsController : Controller
    {
        private StudentService _studentService;
        private const int PageSize = 10;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _studentService = new StudentService(this, PageSize);
            base.OnActionExecuting(filterContext);
        }

        // GET: FoodPantryLogs
        [Authorize]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            var db = new FoodPantryDataModel();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortParm = string.IsNullOrEmpty(sortOrder) ? "date_insert_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            IEnumerable<FoodPantryLog> foodPantryLogs = db.FoodPantryLogs
                .Include(f => f.EnrollmentStatus)
                .Include(f => f.InfoSource)
                .Include(f => f.StudentType1)
                .Include(f => f.Location)
                .ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                foodPantryLogs = foodPantryLogs.Where(s => s.Title.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "student_name_desc":
                    foodPantryLogs = foodPantryLogs.OrderByDescending(s => s.StudentName);
                    break;

                case "date_insert_desc":
                    foodPantryLogs = foodPantryLogs.OrderByDescending(s => s.DateInserted);
                    break;

                default:
                    foodPantryLogs = foodPantryLogs.OrderByDescending(s => s.DateInserted);
                    break;
            }

            return View(foodPantryLogs.ToPagedList(pageNumber, pageSize));
        }

        [Authorize]
        // GET: FoodPantryLogs/Details/5
        public ActionResult Details(int? id)
        {
            return RedirectToAction("Edit", new { @id = id });
            /*
                        FindReferrer();
                        if (id == null)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }
                        FoodPantryLog foodPantryLog = Db.FoodPantryLogs.Find(id);
                        if (foodPantryLog == null)
                        {
                            return HttpNotFound();
                        }
                        return View(foodPantryLog);
            */
        }

        [Authorize]
        // GET: FoodPantryLogs/CreateWithStudent
        public ActionResult CreateWithStudent([Bind(Include = "StudentIDNO")] FoodPantryLog foodPantryLog)
        {
            FoodPantryLog foodPantryLogLatest;

            FindReferrer();
            using (var db = new FoodPantryDataModel())
            {
                var studentInfo = _studentService.FindStudentBio(foodPantryLog.StudentIDNO);
                var foodPantryLogs = db.FoodPantryLogs
                    .Include(f => f.EnrollmentStatus)
                    .Include(f => f.InfoSource)
                    .Include(f => f.StudentType1)
                    .Include(f => f.Location)
                    .Where(s => s.StudentIDNO == foodPantryLog.StudentIDNO)
                    .OrderByDescending(s => s.DateInserted).ToList();

                foodPantryLogLatest = foodPantryLog;

                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (foodPantryLogs != null && foodPantryLogs.Count > 0)
                {
                    foodPantryLogLatest = foodPantryLogs.First();
                }

                foodPantryLogLatest.StudentIDNO = foodPantryLog.StudentIDNO;
                foodPantryLogLatest.StudentName = studentInfo.FirstName + " " + studentInfo.LastName;
                foodPantryLogLatest.Qty_LargeBag = null;
                foodPantryLogLatest.Qty_SmallBag = null;
                foodPantryLogLatest.DateInserted = null;
                foodPantryLogLatest.Signature = string.Empty;

                ViewBag.EnrollmentStatusID = new SelectList(db.EnrollmentStatus, "EnrollmentStatusID", "Title");
                ViewBag.InfoSourcesID = new SelectList(db.InfoSources, "InfoSourcesID", "Title");
                ViewBag.StudentType = new SelectList(db.StudentTypes, "StudentType1", "StudentTypeDesc");
                //ViewBag.Locations = new SelectList(_db.Locations, "LocationDesc", "LocationDesc");
            }

            return View("Create", foodPantryLogLatest);
        }

        [Authorize]
        // GET: FoodPantryLogs/Create
        public ActionResult Create()
        {
            FindReferrer();

            var db = new FoodPantryDataModel();

            ViewBag.EnrollmentStatusID = new SelectList(db.EnrollmentStatus, "EnrollmentStatusID", "Title");
            ViewBag.InfoSourcesID = new SelectList(db.InfoSources, "InfoSourcesID", "Title");
            ViewBag.StudentType = new SelectList(db.StudentTypes, "StudentType1", "StudentTypeDesc");
            ViewBag.LocationID = new SelectList(db.Locations, "Location", "LocationDesc");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(
            [Bind(Include = "LocationID,FoodPantryLogID,StudentIDNO,WaterBottle,StudentName,NoInHousehold,DeltaStudCount,NoChildren0Thru5,NoChildren6Thru8,StudentType," +
                            "EnrollmentStatusID,InfoSourcesID,ExplainOther,Signature,Inc_Loans,Inc_Spouse,Inc_OnCampus," +
                            "Inc_OffCampus,Inc_Scholarships,Inc_ParentalSupport,Inc_Other,Inc_Grants,Qty_LargeBag,Qty_SmallBag")] FoodPantryLog foodPantryLog)
        {
            ViewBag.Referrer = Session["Referrer"];
            foodPantryLog.DateInserted = DateTime.Now;
            foodPantryLog.Title = "Food pantry entry - " + foodPantryLog.StudentIDNO + " - " + DateTime.Now;

            var db = new FoodPantryDataModel();

            if (ModelState.IsValid)
            {
                db.FoodPantryLogs.Add(foodPantryLog);
                db.SaveChanges();
                return RedirectToReferrer();
            }

            ViewBag.EnrollmentStatusID = new SelectList(db.EnrollmentStatus, "EnrollmentStatusID", "Title", foodPantryLog.EnrollmentStatusID);
            ViewBag.InfoSourcesID = new SelectList(db.InfoSources, "InfoSourcesID", "Title", foodPantryLog.InfoSourcesID);
            ViewBag.StudentType = new SelectList(db.StudentTypes, "StudentType1", "StudentTypeDesc", foodPantryLog.StudentType);
            ViewBag.LocationID = new SelectList(db.Locations, "Location", "LocationDesc", foodPantryLog.LocationID);

            return View(foodPantryLog);
        }

        // GET: FoodPantryLogs/Edit/5
        [Authorize(Roles = "FoodPantry_Admins")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            FindReferrer();

            var db = new FoodPantryDataModel();
            var foodPantryLog = db.FoodPantryLogs
                                    .ToList()
                                    .FirstOrDefault(x => x.FoodPantryLogID == id);

            //if (foodPantryLog == null) return HttpNotFound();

            ViewBag.EnrollmentStatusID = new SelectList(db?.EnrollmentStatus, "EnrollmentStatusID", "Title", foodPantryLog?.EnrollmentStatusID);
            ViewBag.InfoSourcesID = new SelectList(db?.InfoSources, "InfoSourcesID", "Title", foodPantryLog?.InfoSourcesID);
            ViewBag.StudentType = new SelectList(db?.StudentTypes, "StudentType1", "StudentTypeDesc", foodPantryLog?.StudentType);
            ViewBag.LocationDesc = new SelectList(db?.Locations, "Location", "LocationDesc", foodPantryLog?.LocationID);

            return View(foodPantryLog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "FoodPantry_Admins")]
        public ActionResult Edit(
            [Bind(Include = "LocationID,FoodPantryLogID,StudentIDNO,WaterBottle,DateInserted,StudentName,NoInHousehold,DeltaStudCount,NoChildren0Thru5,NoChildren6Thru8," +
                            "StudentType,EnrollmentStatusID,InfoSourcesID,ExplainOther,Signature,Inc_Loans,Inc_Spouse,Inc_OnCampus," +
                            "Inc_OffCampus,Inc_Scholarships,Inc_ParentalSupport,Inc_Other,Inc_Grants,Qty_LargeBag,Qty_SmallBag")] FoodPantryLog foodPantryLog)
        {
            ViewBag.Referrer = Session["Referrer"];
            foodPantryLog.Title = "Food pantry entry - " + foodPantryLog.StudentIDNO + " - " + foodPantryLog.DateInserted;

            var db = new FoodPantryDataModel();

            if (ModelState.IsValid)
            {
                db.Entry(foodPantryLog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToReferrer();
            }

            ViewBag.EnrollmentStatusID = new SelectList(db.EnrollmentStatus, "EnrollmentStatusID", "Title", foodPantryLog.EnrollmentStatusID);
            ViewBag.InfoSourcesID = new SelectList(db.InfoSources, "InfoSourcesID", "Title", foodPantryLog.InfoSourcesID);
            ViewBag.StudentType = new SelectList(db.StudentTypes, "StudentType1", "StudentTypeDesc", foodPantryLog.StudentType);

            return View(foodPantryLog);
        }

        // GET: FoodPantryLogs/Delete/5
        [Authorize(Roles = "FoodPantry_Admins")]
        public ActionResult Delete(int? id)
        {
            FoodPantryLog foodPantryLog;

            FindReferrer();
            return RedirectToReferrer();
            /*
                        if (id == null)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }
                        using (var db = new FoodPantryDataModel())
                        {
                            foodPantryLog = db.FoodPantryLogs.Find(id);
                            if (foodPantryLog == null)
                            {
                                return HttpNotFound();
                            }
                        }

                        return View(foodPantryLog);
            */
        }

        // POST: FoodPantryLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "FoodPantry_Admins")]
        public ActionResult DeleteConfirmed(int id)
        {
            return RedirectToReferrer();
            /*
                        ViewBag.Referrer = Session["Referrer"];
                        FoodPantryLog foodPantryLog = Db.FoodPantryLogs.Find(id);
                        Db.FoodPantryLogs.Remove(foodPantryLog);
                        Db.SaveChanges();
                        return RedirectToReferrer();
            */
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                using (var db = new FoodPantryDataModel())
                {
                    db.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        public ActionResult UserImage(string id)
        {
            try
            {
                var path = WebConfigurationManager.AppSettings["ImagesFolder"] + id + ".jpg";
                if (path.StartsWith("http"))
                {
                    using (var client = new WebClient())
                    {
                        return new FileStreamResult(client.OpenRead(path), "image /jpeg");
                    }
                }
                else
                {
                    return new FileStreamResult(new FileStream(path, FileMode.Open), "image/jpeg");
                }
            }
            catch (Exception e)
            {
                return Redirect(@"~\Content\profile-icon.png");
            }
        }

        private void SendEmailNotification(string studentId)
        {
            var fromEmailAddress = WebConfigurationManager.AppSettings["FromEmailAddress"];
            var emailServer = WebConfigurationManager.AppSettings["EmailServer"];
            var studentEmailAddress = string.Empty;

            EmailSetting emailSetting;
            int numberOfLogs;

            using (var db = new FoodPantryDataModel())
            {
                emailSetting = db.EmailSettings.First(e => e.ActiveEmail);
                numberOfLogs = db.FoodPantryLogs
                    .Where(s => s.StudentIDNO == studentId)
                    .Count(s => s.DateInserted >= emailSetting.DateModified);
            }

            if (numberOfLogs <= 1)
            {
                SendEmail(emailSetting, fromEmailAddress, studentEmailAddress, emailServer);
            }
        }

        /// <summary>
        /// Send an email to a student.
        /// </summary>
        private static void SendEmail(EmailSetting emailSetting, string fromEmailAddress, string studentEmailAddress, string emailServer)
        {
            try
            {
                var mail = new MailMessage(fromEmailAddress, studentEmailAddress);
                var client = new SmtpClient
                {
                    Port = 25,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = emailServer
                };

                mail.Subject = emailSetting.SubjectContents;
                mail.Body = emailSetting.EmailContents;
                client.Send(mail);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// Find the referrer for the page.
        /// </summary>
        private void FindReferrer()
        {
            ViewBag.Referrer = null;
            if (string.IsNullOrEmpty((string)Request.ServerVariables["http_referer"])) return;
            Session["Referrer"] = Request.ServerVariables["http_referer"];
            ViewBag.Referrer = Session["Referrer"];
        }

        /// <summary>
        /// Redirects to the Referrer saved in the Session.
        /// </summary>
        private ActionResult RedirectToReferrer()
        {
            if (!string.IsNullOrEmpty((string)Session["Referrer"]))
            {
                return Redirect((string)Session["Referrer"]);
            }
            return RedirectToAction("Index");
        }
    }
}