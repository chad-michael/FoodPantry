using FoodPantry.Data.Models;
using FoodPantry.Data.Repositories;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace FoodPantry.Core.Repositories
{
    /// <summary>
    /// Connects to the colleague webservice for terms.
    /// </summary>
    public class ColleagueWsStudentsRepository : ColleagueWsRepository, IStudentsRespository
    {
        public ColleagueWsStudentsRepository(string serviceUrl, string username, string password)
            : base(serviceUrl, username, password)
        {
            ;
        }

        public ColleagueWsStudentsRepository(string serviceUrl, string sessionToken)
            : base(serviceUrl, sessionToken)
        {
            ;
        }

        /// <summary>
        /// Get the student's information
        /// </summary>
        /// <param name="studentId">student to find.</param>
        /// <returns>student information</returns>
        public Student Find(string studentId)
        {
            string json = GetService("students/" + studentId);
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            return jsonSerializer.Deserialize<Student>(json);
        }

        /// <summary>
        /// Find all the ungraded terms for a student.
        /// </summary>
        /// <param name="studentId">student to find.</param>
        /// <returns>the students credits.</returns>
        public IList<UngradedTerm> FindAllUngradedTerms(string studentId)
        {
            string json = GetService("students/" + studentId + "/ungraded-terms");
            if (json == "{}")
            {
                return new List<UngradedTerm>();
            }
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            return jsonSerializer.Deserialize<IList<UngradedTerm>>(json);
        }
    }
}