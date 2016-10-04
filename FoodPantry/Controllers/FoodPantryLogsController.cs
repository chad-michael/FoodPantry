using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using FoodPantry.Models;
using PagedList;
using System.Net.Mail;
using System.IO;
using System.Web.Configuration;
using FoodPantry.Data.Repositories;
using FoodPantry.Core.Repositories;
using FoodPantry.Core.Services;

namespace FoodPantry.Controllers
{
    /// <summary>
    /// Controller for manage the food pantry logs.
    /// </summary>
    [Authorize(Roles = "FoodPantry_Users")]
    public class FoodPantryLogsController : Controller
    {
        /// <summary>
        /// Access to the food pantry database.
        /// </summary>
        private Models.FoodPantry db = new Models.FoodPantry();

        /// <summary>
        /// Service to access student information
        /// </summary>
        private StudentService StudentService;

        /// <summary>
        /// Number of items to display on each page.
        /// </summary>
        private int PageSize = 10;

        /// <summary>
        /// Initialize the terms service.
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            StudentService = new StudentService(this, PageSize);
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Displays a list of all the log entries.
        /// </summary>
        /// <param name="sortOrder">for the list.</param>
        /// <param name="currentFilter">field to filter on.</param>
        /// <param name="searchString">to filter on.</param>
        /// <param name="page">currently viewing.</param>
        /// <returns>The index view.</returns>
        // GET: FoodPantryLogs
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortParm = String.IsNullOrEmpty(sortOrder) ? "date_insert_desc" : "";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var foodPantryLogs = db.FoodPantryLogs
                .Include(f => f.EnrollmentStatu)
                .Include(f => f.InfoSource)
                .Include(f => f.StudentType1);

            if (!String.IsNullOrEmpty(searchString))
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
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(foodPantryLogs.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// Details will display information about the log. We will keep in place but 
        /// just rediret to edit action.
        /// </summary>
        /// <param name="id">of log to look at.</param>
        /// <returns>The details view.</returns>
        // GET: FoodPantryLogs/Details/5
        public ActionResult Details(int? id)
        {
            return RedirectToAction("Edit", new { @id = id });
            FindReferrer();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodPantryLog foodPantryLog = db.FoodPantryLogs.Find(id);
            if (foodPantryLog == null)
            {
                return HttpNotFound();
            }
            return View(foodPantryLog);
        }

        /// <summary>
        /// Create a new log from a student id and using their last log.
        /// </summary>
        /// <param name="foodPantryLog">should contain the student's ID</param>
        /// <returns>The create view.</returns>
        // GET: FoodPantryLogs/CreateWithStudent
        public ActionResult CreateWithStudent([Bind(Include = "StudentIDNO")] FoodPantryLog foodPantryLog)
        {
            FindReferrer();
            var foodPantryLogs = db.FoodPantryLogs
                .Include(f => f.EnrollmentStatu)
                .Include(f => f.InfoSource)
                .Include(f => f.StudentType1)
                .Where(s => s.StudentIDNO == foodPantryLog.StudentIDNO)
                .OrderByDescending(s => s.DateInserted).ToList();
            var studentInfo = StudentService.FindStudentBio(foodPantryLog.StudentIDNO);
            var foodPantryLogLatest = foodPantryLog;
            if (foodPantryLogs != null && foodPantryLogs.Count > 0)
            {
                foodPantryLogLatest = foodPantryLogs.First();
            }

            foodPantryLogLatest.StudentIDNO = foodPantryLog.StudentIDNO;
            foodPantryLogLatest.StudentName = studentInfo.FirstName + " " + studentInfo.LastName;
            foodPantryLogLatest.Qty_LargeBag = null;
            foodPantryLogLatest.Qty_SmallBag = null;
            foodPantryLogLatest.DateInserted = null;
            foodPantryLogLatest.Signature = String.Empty;

            ViewBag.EnrollmentStatusID = new SelectList(db.EnrollmentStatus, "EnrollmentStatusID", "Title");
            ViewBag.InfoSourcesID = new SelectList(db.InfoSources, "InfoSourcesID", "Title");
            ViewBag.StudentType = new SelectList(db.StudentTypes, "StudentType1", "StudentTypeDesc");
            return View("Create", foodPantryLogLatest);
        }

        /// <summary>
        /// Create a new food pantry log.
        /// </summary>
        /// <returns>The create view.</returns>
        // GET: FoodPantryLogs/Create
        public ActionResult Create()
        {
            FindReferrer();
            ViewBag.EnrollmentStatusID = new SelectList(db.EnrollmentStatus, "EnrollmentStatusID", "Title");
            ViewBag.InfoSourcesID = new SelectList(db.InfoSources, "InfoSourcesID", "Title");
            ViewBag.StudentType = new SelectList(db.StudentTypes, "StudentType1", "StudentTypeDesc");
            return View();
        }

        /// <summary>
        /// Save the new food pantry log to the database.
        /// </summary>
        /// <param name="foodPantryLog">log to save.</param>
        /// <returns>a redirect if successful and the edit view is not successful.</returns>
        // POST: FoodPantryLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FoodPantryLogID,StudentIDNO,WaterBottle,StudentName,NoInHousehold,DeltaStudCount,NoChildren0Thru5,NoChildren6Thru8,StudentType,EnrollmentStatusID,InfoSourcesID,ExplainOther,Signature,Inc_Loans,Inc_Spouse,Inc_OnCampus,Inc_OffCampus,Inc_Scholarships,Inc_ParentalSupport,Inc_Other,Inc_Grants,Qty_LargeBag,Qty_SmallBag")] FoodPantryLog foodPantryLog)
        {
            ViewBag.Referrer = Session["Referrer"];
            foodPantryLog.DateInserted = DateTime.Now;
            foodPantryLog.Title = "Food pantry entry - " + foodPantryLog.StudentIDNO + " - " + DateTime.Now;
            if (ModelState.IsValid)
            {
                db.FoodPantryLogs.Add(foodPantryLog);
                db.SaveChanges();
                return RedirectToReferrer();
            }

            ViewBag.EnrollmentStatusID = new SelectList(db.EnrollmentStatus, "EnrollmentStatusID", "Title", foodPantryLog.EnrollmentStatusID);
            ViewBag.InfoSourcesID = new SelectList(db.InfoSources, "InfoSourcesID", "Title", foodPantryLog.InfoSourcesID);
            ViewBag.StudentType = new SelectList(db.StudentTypes, "StudentType1", "StudentTypeDesc", foodPantryLog.StudentType);
            return View(foodPantryLog);
        }

        /// <summary>
        /// Displays the view to edit a log.
        /// </summary>
        /// <param name="id">of the log to edit.</param>
        /// <returns>editing view of for the log.</returns>
        // GET: FoodPantryLogs/Edit/5
        [Authorize(Roles = "FoodPantry_Admins")]
        public ActionResult Edit(int? id)
        {
            FindReferrer();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodPantryLog foodPantryLog = db.FoodPantryLogs.Find(id);
            if (foodPantryLog == null)
            {
                return HttpNotFound();
            }
            ViewBag.EnrollmentStatusID = new SelectList(db.EnrollmentStatus, "EnrollmentStatusID", "Title", foodPantryLog.EnrollmentStatusID);
            ViewBag.InfoSourcesID = new SelectList(db.InfoSources, "InfoSourcesID", "Title", foodPantryLog.InfoSourcesID);
            ViewBag.StudentType = new SelectList(db.StudentTypes, "StudentType1", "StudentTypeDesc", foodPantryLog.StudentType);
            return View(foodPantryLog);
        }


        /// <summary>
        /// Handles the updating of a given log.
        /// </summary>
        /// <param name="foodPantryLog">Log to save.</param>
        /// <returns>The redirect if successful or the view if unsuscessful.</returns>
        // POST: FoodPantryLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "FoodPantry_Admins")]
        public ActionResult Edit([Bind(Include = "FoodPantryLogID,StudentIDNO,WaterBottle,DateInserted,StudentName,NoInHousehold,DeltaStudCount,NoChildren0Thru5,NoChildren6Thru8,StudentType,EnrollmentStatusID,InfoSourcesID,ExplainOther,Signature,Inc_Loans,Inc_Spouse,Inc_OnCampus,Inc_OffCampus,Inc_Scholarships,Inc_ParentalSupport,Inc_Other,Inc_Grants,Qty_LargeBag,Qty_SmallBag")] FoodPantryLog foodPantryLog)
        {
            ViewBag.Referrer = Session["Referrer"];
            foodPantryLog.Title = "Food pantry entry - " + foodPantryLog.StudentIDNO + " - " + foodPantryLog.DateInserted;
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

        /// <summary>
        /// Display information about the log to delete. This is not enable but we can enable later.
        /// </summary>
        /// <param name="id">of the log to delete.</param>
        /// <returns>the view</returns>
        // GET: FoodPantryLogs/Delete/5
        [Authorize(Roles = "FoodPantry_Admins")]
        public ActionResult Delete(int? id)
        {
            FindReferrer();
            return RedirectToReferrer();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodPantryLog foodPantryLog = db.FoodPantryLogs.Find(id);
            if (foodPantryLog == null)
            {
                return HttpNotFound();
            }
            return View(foodPantryLog);
        }

        /// <summary>
        /// Confirm the deletion of the log. This is not enable but we can enable later.
        /// </summary>
        /// <param name="id">of log to delete.</param>
        /// <returns>the redirect</returns>
        // POST: FoodPantryLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "FoodPantry_Admins")]
        public ActionResult DeleteConfirmed(int id)
        {
            return RedirectToReferrer();
            ViewBag.Referrer = Session["Referrer"];
            FoodPantryLog foodPantryLog = db.FoodPantryLogs.Find(id);
            db.FoodPantryLogs.Remove(foodPantryLog);
            db.SaveChanges();
            return RedirectToReferrer();
        }

        /// <summary>
        /// Cleanup database connections on close.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        
        /// <summary>
        /// Returns the student's image from the student's ID
        /// </summary>
        /// <param name="id">of the student</param>
        /// <returns>The student image</returns>
        public ActionResult UserImage(string id)
        {
            try
            {
                var path = WebConfigurationManager.AppSettings["ImagesFolder"] + id + ".jpg";
                if (path.StartsWith("http"))
                {
                    using (WebClient client = new WebClient())
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

        /// <summary>
        /// Send email to a student if there is one log for the semester.
        /// </summary>
        /// <param name="studentId">ID for the student.</param>
        private void SendEmailNotification(string studentId)
        {
            var fromEmailAddress = WebConfigurationManager.AppSettings["FromEmailAddress"];
            var emailServer = WebConfigurationManager.AppSettings["EmailServer"];
            var studentEmailAddress = string.Empty;
            var emailSetting = db.EmailSettings.Where(e => e.ActiveEmail).First();
            var numberOfLogs = db.FoodPantryLogs
                .Where(s => s.StudentIDNO == studentId)
                .Where(s => s.DateInserted >= emailSetting.DateModified)
                .Count();
            if (numberOfLogs <= 1)
            {
                SendEmail(emailSetting, fromEmailAddress, studentEmailAddress, emailServer);
            }
        }

        /// <summary>
        /// Send an email to a student.
        /// </summary>
        /// <param name="emailSetting">containing the subject and body.</param>
        /// <param name="fromEmailAddress">from what email address is the email coming from.</param>
        /// <param name="studentEmailAddress">sending the email to.</param>
        /// <param name="emailServer">server sending the email.</param>
        private void SendEmail(EmailSetting emailSetting, string fromEmailAddress, string studentEmailAddress, string emailServer)
        {
            try
            {
                MailMessage mail = new MailMessage(fromEmailAddress, studentEmailAddress);
                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = emailServer;

                mail.Subject = emailSetting.SubjectContents;
                mail.Body = emailSetting.EmailContents;
                client.Send(mail);
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Find the referrer for the page.
        /// </summary>
        private void FindReferrer()
        {
            ViewBag.Referrer = null;
            if (!String.IsNullOrEmpty((string)Request.ServerVariables["http_referer"]))
            {
                Session["Referrer"] = Request.ServerVariables["http_referer"];
                ViewBag.Referrer = Session["Referrer"];
            }
        }

        /// <summary>
        /// Redirects to the Referrer saved in the Session.
        /// </summary>
        /// <returns>A redirect to the referrer or the index.</returns>
        private ActionResult RedirectToReferrer()
        {
            if (!String.IsNullOrEmpty((string)Session["Referrer"]))
            {
                return Redirect((string)Session["Referrer"]);
            }
            return RedirectToAction("Index");
        }
    }
}
