using FoodPantry.Controllers.Extensions;
using FoodPantry.Core.Services;
using FoodPantry.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

using System.Web.Mvc;

using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace FoodPantry.Controllers
{
    /// <summary>
    /// Main controller for the application.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Service to access term information.
        /// </summary>
        private TermsService TermsService;

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
            TermsService = new TermsService(this);
            StudentService = new StudentService(this, PageSize);
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Main landing page for the user.
        /// </summary>
        /// <param name="studentId">the student of interest.</param>
        /// <param name="page">of the logs.</param>
        /// <returns></returns>
        //[Authorize(Roles = "FoodPantry_Users")]
        [AllowAnonymous]
        public ActionResult Index(string studentId, int? page)
        {
            var terms = HttpRuntime
                            .Cache
                            .GetOrStore("CurrentTerms", () => TermsService.TermRepository.FindAllRegistrationTerms());
            ViewBag.Terms = terms;

            if (!String.IsNullOrEmpty(studentId))
            {
                studentId = studentId.TrimStart('0');

                // Should get moved to a service.
                var foodPantryLogs = StudentService.FindLogsForStudentId(studentId, page);
                var studentInfo = StudentService.FindStudentBio(studentId);
                var transcript = StudentService.FindTranscriptForActiveTerms(studentId, terms);
                var countsForMonth = StudentService.CountsBagsForCurrentMonth(studentId);
                var note = StudentService.FindNoteForStudentId(studentId);

                ViewBag.studentId = studentId;
                ViewBag.transcript = transcript;
                ViewBag.studentInfo = studentInfo;
                ViewBag.studentNote = note;
                try
                {
                    ViewBag.userFound = foodPantryLogs.First();
                }
                catch (Exception) { }
                ViewBag.countsForMonth = countsForMonth;
                return View(foodPantryLogs);
            }
            return View();
        }

        /// <summary>
        /// After a success signin, redirect to Index. If implementing signout try using,
        /// WSFederationAuthenticationModule.FederatedSignOut(null, new Uri(replyUrl));
        /// </summary>
        /// <returns>The redirect to the Index action.</returns>
        public ActionResult SignIn()
        {
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Cleanup database connections on close.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && StudentService != null)
            {
                StudentService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}