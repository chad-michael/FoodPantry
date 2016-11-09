using FoodPantry.Data.Models;
using FoodPantry.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Script.Serialization;

namespace FoodPantry.Core.Repositories
{
    /// <summary>
    /// Connects to the colleague webservice for terms.
    /// </summary>
    public class ColleagueWsTermsRepository : ColleagueWsRepository, ITermsRespository
    {
        public ColleagueWsTermsRepository(string serviceUrl, string username, string password)
            : base(serviceUrl, username, password)
        {
            ;
        }

        public ColleagueWsTermsRepository(string serviceUrl, string sessionToken)
            : base(serviceUrl, sessionToken)
        {
            ;
        }

        /// <summary>
        /// Retrieves all Terms that are open for pre-registration or registration.
        /// </summary>
        /// <returns>A list of terms.</returns>
        public IList<Term> FindAllRegistrationTerms()
        {
            DateTime startDate = DateTime.Today.AddDays(-120);
            DateTime startsOnOrAfter;
            DateTime.TryParse(startDate.ToString(CultureInfo.InvariantCulture), out startsOnOrAfter);

            //string json = GetService("terms/registration");
            string json = GetService("terms?startsOnOrAfter=" + startsOnOrAfter);
            if (json == "{}")
            {
                return new List<Term>();
            }

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            IList<Term> terms = jsonSerializer.Deserialize<IList<Term>>(json);

            var fullTermList = terms.Where(t => t.Code.EndsWith("/FA") || t.Code.EndsWith("/WI") || t.Code.EndsWith("/SP")).ToList();
            var termList = fullTermList.Where(t => t.Code.StartsWith("16") || t.Code.StartsWith("17")).ToList();

            var wantedTermsToDisplay = termList
                .Where(t => t.StartDate < (DateTime.Today.AddDays(300)))
                .OrderBy(t => t.StartDate)
                .ToList();

            return wantedTermsToDisplay;
        }
    }
}