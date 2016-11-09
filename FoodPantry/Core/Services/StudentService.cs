using FoodPantry.Core.Repositories;
using FoodPantry.Data.Models;
using FoodPantry.Data.Repositories;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;

namespace FoodPantry.Core.Services
{
    public class StudentService
    {
        /// <summary>
        /// Access to the food pantry database.
        /// </summary>
        private readonly Data.Models.FoodPantryDataModel _foodPantryDb = new Data.Models.FoodPantryDataModel();

        private readonly int _pageSize = 10;
        private IStudentsRespository _studentsRepository;
        private readonly Controller _parentController;

        internal IStudentsRespository StudentsRepository
        {
            get
            {
                if (_studentsRepository != null)
                {
                    return _studentsRepository;
                }
                var serviceUrl = WebConfigurationManager.AppSettings["ColleagueWebServiceUrl"];
                var serivceSessionToken = (string)_parentController.Session["ColleagueWebserviceSessionToken"];
                if (string.IsNullOrEmpty(serivceSessionToken))
                {
                    var username = WebConfigurationManager.AppSettings["ColleagueWebServiceUsername"];
                    var password = WebConfigurationManager.AppSettings["ColleagueWebServicePassword"];
                    _studentsRepository = new ColleagueWsStudentsRepository(serviceUrl, username, password);
                    _parentController.Session["ColleagueWebserviceSessionToken"] = ((ColleagueWsRepository)_studentsRepository).SessionToken;
                }
                else
                {
                    _studentsRepository = new ColleagueWsStudentsRepository(serviceUrl, serivceSessionToken);
                }
                return _studentsRepository;
            }
            set
            {
                _studentsRepository = value;
            }
        }

        public StudentService(Controller parentController, int pageSize)
        {
            _parentController = parentController;
            _pageSize = pageSize;
        }

        public UngradedTerm FindTranscriptForActiveTerms(string studentId, IList<Term> terms)
        {
            var ungradedTerms = StudentsRepository.FindAllUngradedTerms("1180634");

            var returnCode = (from item in ungradedTerms
                              from term in terms
                              where term.Code == item.Code
                              select item)
                              .FirstOrDefault();

            return returnCode;

            //var ungradedTerms = StudentsRepository.FindAllUngradedTerms(studentId);
            //string code = null;

            //foreach (var term in terms)
            //{
            //    if (term. == (1).ToString())
            //    {
            //        code = term.Code;
            //    }
            //}

            //var ungradedCode = ungradedTerms.FirstOrDefault(x => x.Code == code);

            //return ungradedCode;
        }

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

        public IPagedList<FoodPantryLog> FindLogsForStudentId(string studentId, int? page)
        {
            var foodpantrylog = _foodPantryDb.FoodPantryLogs
                .Where(s => s.StudentIDNO == studentId)
                .OrderByDescending(s => s.DateInserted);

            return foodpantrylog.ToPagedList((page ?? 1), _pageSize);
        }

        public Note FindNoteForStudentId(string studentId)
        {
            var note = _foodPantryDb.Notes.FirstOrDefault(s => s.StudentIDNO == studentId);
            if (note != null) return note;
            note = new Note { Notes = "No notes." };
            return note;
        }

        public BagCounts CountsBagsForCurrentMonth(string studentId)
        {
            var bagCounts = new BagCounts();
            var now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, 1);
            var lastDate = startDate.AddMonths(1).AddDays(-1);
            var results = _foodPantryDb.FoodPantryLogs
                .Where(s => s.StudentIDNO == studentId)
                .Where(s => s.DateInserted >= startDate && s.DateInserted <= lastDate)
                .ToList();

            foreach (var item in results)
            {
                if (item.Qty_LargeBag != null) bagCounts.Larges += item.Qty_LargeBag.Value;
                if (item.Qty_SmallBag != null) bagCounts.Smalls += item.Qty_SmallBag.Value;
            }

            return bagCounts;
        }

        public void Dispose()
        {
            _foodPantryDb.Dispose();
        }
    }
}