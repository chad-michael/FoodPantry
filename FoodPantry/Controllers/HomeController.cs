using FoodPantry.Controllers.Extensions;
using FoodPantry.Core.Services;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodPantry.Controllers
{
    /// <summary>
    /// Main controller for the application.
    /// </summary>
    public class HomeController : Controller
    {
        private TermsService _termsService;
        private StudentService _studentService;
        private const int PageSize = 10;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _termsService = new TermsService(this);
            _studentService = new StudentService(this, PageSize);
            base.OnActionExecuting(filterContext);
        }

        [Authorize(Roles = "FoodPantry_Users")]
        public ActionResult Index(string studentId, int? page)
        {
            var terms = HttpRuntime
                            .Cache
                            .GetOrStore("CurrentTerms", () => _termsService.TermRepository.FindAllRegistrationTerms());
            ViewBag.Terms = terms;

            if (!string.IsNullOrEmpty(studentId))
            {
                studentId = studentId.TrimStart('0');

                // Should get moved to a service.
                var transcript = _studentService.FindTranscriptForActiveTerms(studentId, terms);

                var foodPantryLogs = _studentService.FindLogsForStudentId(studentId, page);
                var studentInfo = _studentService.FindStudentBio(studentId);

                var countsForMonth = _studentService.CountsBagsForCurrentMonth(studentId);
                var note = _studentService.FindNoteForStudentId(studentId);

                ViewBag.studentId = studentId;
                ViewBag.transcript = transcript;
                ViewBag.studentInfo = studentInfo;
                ViewBag.studentNote = note;

                try
                {
                    ViewBag.userFound = foodPantryLogs.First();
                }
                catch (Exception)
                {
                    // ignored
                }

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
            if (disposing)
            {
                _studentService?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}