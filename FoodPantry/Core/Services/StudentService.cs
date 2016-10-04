using FoodPantry.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Data;
using System.Data.Entity;
using FoodPantry.Data.Models;
using System.Web.Mvc;
using FoodPantry.Data.Repositories;
using System.Web.Configuration;
using FoodPantry.Core.Repositories;

namespace FoodPantry.Core.Services
{
    public class StudentService
    {
        /// <summary>
        /// Access to the food pantry database.
        /// </summary>
        private Models.FoodPantry foodPantryDb = new Models.FoodPantry();

        /// <summary>
        /// Number of items to display on each page.
        /// </summary>
        private int PageSize = 10;

        /// <summary>
        /// The students repository to get informatino from.
        /// </summary>
        private IStudentsRespository studentsRepository;

        /// <summary>
        /// The parent controller to access the session.
        /// </summary>
        private Controller ParentController;

        /// <summary>
        /// Get the terms repository.
        /// </summary>
        internal IStudentsRespository StudentsRepository
        {
            get
            {
                if (studentsRepository != null)
                {
                    return studentsRepository;
                }
                var serviceUrl = WebConfigurationManager.AppSettings["ColleagueWebServiceUrl"];
                var serivceSessionToken = (string)ParentController.Session["ColleagueWebserviceSessionToken"];
                if (string.IsNullOrEmpty(serivceSessionToken))
                {
                    var username = WebConfigurationManager.AppSettings["ColleagueWebServiceUsername"];
                    var password = WebConfigurationManager.AppSettings["ColleagueWebServicePassword"];
                    studentsRepository = new ColleagueWsStudentsRepository(serviceUrl, username, password);
                    ParentController.Session["ColleagueWebserviceSessionToken"] = ((ColleagueWsRepository)studentsRepository).SessionToken;
                }
                else
                {
                    studentsRepository = new ColleagueWsStudentsRepository(serviceUrl, serivceSessionToken);
                }
                return studentsRepository;
            }
            set
            {
                studentsRepository = value;
            }
        }

        /// <summary>
        /// Build the terms service with the parent controller.
        /// </summary>
        /// <param name="parentController"></param>
        public StudentService(Controller parentController, int pageSize)
        {
            ParentController = parentController;
            PageSize = pageSize;
        }

        /// <summary>
        /// Get the first transcript for the student in the available terms.
        /// </summary>
        /// <param name="studentId">id of the student.</param>
        /// <param name="terms">to find the transcript for.</param>
        /// <returns>a transcript or null.</returns>
        public UngradedTerm FindTranscriptForActiveTerms(string studentId, IList<Term> terms)
        {
            var ungradedTerms = StudentsRepository.FindAllUngradedTerms(studentId);

            foreach (var item in ungradedTerms)
            {
                foreach (var term in terms)
                {
                    if (term.Code == item.Code)
                    {
                        return item;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Find the student bio from the student id.
        /// </summary>
        /// <param name="studentId">id of the student.</param>
        /// <returns>a bio of the student.</returns>
        public Student FindStudentBio(string studentId)
        {
            try
            {
                return StudentsRepository.Find(studentId);
            }
            catch (Exception)
            {
                return null;

            }
        }

        /// <summary>
        /// Get a paged list of logs for the student.
        /// </summary>
        /// <param name="studentId">id of the student</param>
        /// <param name="page">page to display</param>
        /// <returns>a paged list.</returns>
        public IPagedList<FoodPantryLog> FindLogsForStudentId(string studentId, int? page)
        {
            return foodPantryDb.FoodPantryLogs
                        .Include(f => f.EnrollmentStatu)
                        .Include(f => f.InfoSource)
                        .Include(f => f.StudentType1)
                        .Where(s => s.StudentIDNO == studentId)
                        .OrderByDescending(s => s.DateInserted)
                        .ToPagedList((page ?? 1), PageSize);
        }

        /// <summary>
        /// Get the first note for the student.
        /// </summary>
        /// <param name="studentId">to lookup by.</param>
        /// <returns>the note or null.</returns>
        public Note FindNoteForStudentId(string studentId)
        {
            var note = foodPantryDb.Notes.Where(s => s.StudentIDNO == studentId).FirstOrDefault();
            if (note == null)
            {
                note = new Note();
                note.Notes = "No notes.";
            }
            return note;
        }

        /// <summary>
        /// Count the number of bags for the student in the current month.
        /// This should get moved to a service.
        /// </summary>
        /// <param name="studentId">of the student to count.</param>
        /// <returns>A bag of counts.</returns>
        public BagCounts CountsBagsForCurrentMonth(string studentId)
        {
            BagCounts bagCounts = new BagCounts();
            DateTime now = DateTime.Now;
            DateTime startDate = new DateTime(now.Year, now.Month, 1);
            DateTime lastDate = startDate.AddMonths(1).AddDays(-1);
            var results = foodPantryDb.FoodPantryLogs
                .Where(s => s.StudentIDNO == studentId)
                .Where(s => s.DateInserted >= startDate && s.DateInserted <= lastDate)
                .ToList();
            foreach (var item in results)
            {
                bagCounts.Larges += item.Qty_LargeBag.Value;
                bagCounts.Smalls += item.Qty_SmallBag.Value;
            }
            return bagCounts;
        }

        /// <summary>
        /// Cleanup database connections.
        /// </summary>
        public void Dispose()
        {
            foodPantryDb.Dispose();
        }
    }
}