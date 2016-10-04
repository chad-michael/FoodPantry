using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FoodPantry.Core.Repositories;
using System.Collections.Generic;
using FoodPantry.Models;
using System.Web.Script.Serialization;
using FoodPantry.Data.Models;

namespace FoodPantry.Tests.Core.Repositories
{
    [TestClass]
    public class ColleagueWsStudentsRepositoryUnitTest
    {
        // [TestMethod]
        public void TestFindAllRegistrationTerms()
        {
            string username = "coll-emobile";
            string password = "33WYUp4K26ovaADB";
            string serviceUrl = "https://emobileapi1.delta.edu:444/colleagueapi/";
            ColleagueWsTermsRepository colleagueWsTermsRepository = new ColleagueWsTermsRepository(serviceUrl, username, password);
            IList<Term> terms = colleagueWsTermsRepository.FindAllRegistrationTerms();
            Assert.IsTrue(terms.Count > 0);
        }

        // [TestMethod]
        public void TestJsonParsing()
        {
            IList<Term> terms = CurrentTerms();
            Assert.IsTrue(terms.Count > 0);
        }

        private IList<Term> CurrentTerms()
        {
            string json = "[{\"Code\":\"16/SP\",\"Description\":\"SPRING 2016\",\"StartDate\":\"2016-05-09T00:00:00\",\"EndDate\":\"2016-08-18T00:00:00\",\"ReportingYear\":2015,\"Sequence\":8,\"ReportingTerm\":\"16/SP\",\"FinancialPeriod\":\"Past\",\"DefaultOnPlan\":true,\"IsActive\":false,\"RegistrationDates\":[{\"RegistrationStartDate\":\"2016-03-14T00:00:00\",\"RegistrationEndDate\":\"2016-05-08T00:00:00\",\"PreRegistrationStartDate\":\"2016-03-08T00:00:00\",\"PreRegistrationEndDate\":\"2016-03-13T00:00:00\",\"AddStartDate\":\"2016-05-09T00:00:00\",\"AddEndDate\":\"2016-08-18T00:00:00\",\"DropStartDate\":\"2016-05-09T00:00:00\",\"DropEndDate\":\"2016-08-18T00:00:00\",\"DropGradeRequiredDate\":null,\"Location\":\"\"}],\"FinancialAidYears\":[]},{\"Code\":\"16/WI\",\"Description\":\"WINTER 2016\",\"StartDate\":\"2016-01-09T00:00:00\",\"EndDate\":\"2016-04-29T00:00:00\",\"ReportingYear\":2015,\"Sequence\":5,\"ReportingTerm\":\"16/WI\",\"FinancialPeriod\":\"Past\",\"DefaultOnPlan\":true,\"IsActive\":false,\"RegistrationDates\":[{\"RegistrationStartDate\":\"2015-11-02T00:00:00\",\"RegistrationEndDate\":\"2016-01-08T00:00:00\",\"PreRegistrationStartDate\":\"2015-10-22T00:00:00\",\"PreRegistrationEndDate\":\"2015-11-01T00:00:00\",\"AddStartDate\":\"2016-01-09T00:00:00\",\"AddEndDate\":\"2016-04-29T00:00:00\",\"DropStartDate\":\"2016-01-09T00:00:00\",\"DropEndDate\":\"2016-04-29T00:00:00\",\"DropGradeRequiredDate\":null,\"Location\":\"\"}],\"FinancialAidYears\":[]},{\"Code\":\"16/CW\",\"Description\":\"NON-CREDIT WINTER 2016\",\"StartDate\":\"2016-01-09T00:00:00\",\"EndDate\":\"2016-04-29T00:00:00\",\"ReportingYear\":2015,\"Sequence\":6,\"ReportingTerm\":\"16/CW\",\"FinancialPeriod\":\"Past\",\"DefaultOnPlan\":false,\"IsActive\":false,\"RegistrationDates\":[{\"RegistrationStartDate\":\"2015-10-12T00:00:00\",\"RegistrationEndDate\":\"2016-01-08T00:00:00\",\"PreRegistrationStartDate\":\"2015-10-12T00:00:00\",\"PreRegistrationEndDate\":\"2015-10-12T00:00:00\",\"AddStartDate\":\"2016-01-09T00:00:00\",\"AddEndDate\":\"2016-04-29T00:00:00\",\"DropStartDate\":\"2016-01-09T00:00:00\",\"DropEndDate\":\"2016-04-29T00:00:00\",\"DropGradeRequiredDate\":null,\"Location\":\"\"}],\"FinancialAidYears\":[]},{\"Code\":\"15/FA\",\"Description\":\"FALL 2015\",\"StartDate\":\"2015-08-29T00:00:00\",\"EndDate\":\"2015-12-20T00:00:00\",\"ReportingYear\":2015,\"Sequence\":2,\"ReportingTerm\":\"15/FA\",\"FinancialPeriod\":\"Past\",\"DefaultOnPlan\":true,\"IsActive\":false,\"RegistrationDates\":[{\"RegistrationStartDate\":\"2015-04-20T00:00:00\",\"RegistrationEndDate\":\"2015-08-28T00:00:00\",\"PreRegistrationStartDate\":\"2015-04-09T00:00:00\",\"PreRegistrationEndDate\":\"2015-04-19T00:00:00\",\"AddStartDate\":\"2015-08-29T00:00:00\",\"AddEndDate\":\"2015-12-20T00:00:00\",\"DropStartDate\":\"2015-08-29T00:00:00\",\"DropEndDate\":\"2015-12-20T00:00:00\",\"DropGradeRequiredDate\":null,\"Location\":\"\"}],\"FinancialAidYears\":[]},{\"Code\":\"15/CF\",\"Description\":\"NON-CREDIT FALL 2015\",\"StartDate\":\"2015-08-29T00:00:00\",\"EndDate\":\"2015-12-20T00:00:00\",\"ReportingYear\":2015,\"Sequence\":3,\"ReportingTerm\":\"15/CF\",\"FinancialPeriod\":\"Past\",\"DefaultOnPlan\":false,\"IsActive\":false,\"RegistrationDates\":[{\"RegistrationStartDate\":\"2015-06-01T00:00:00\",\"RegistrationEndDate\":\"2015-08-28T00:00:00\",\"PreRegistrationStartDate\":\"2015-06-01T00:00:00\",\"PreRegistrationEndDate\":\"2015-06-01T00:00:00\",\"AddStartDate\":\"2015-08-29T00:00:00\",\"AddEndDate\":\"2015-12-20T00:00:00\",\"DropStartDate\":null,\"DropEndDate\":null,\"DropGradeRequiredDate\":null,\"Location\":\"\"}],\"FinancialAidYears\":[]}]";
            var jsonSerializer = new JavaScriptSerializer();
            return jsonSerializer.Deserialize<IList<Term>>(json);
        }

        [TestMethod]
        public void TestUngradedTerms()
        {
            IList<UngradedTerm> ungradedTerms = GetUngradedTerms();
            Assert.IsTrue(ungradedTerms.Count > 0);
        }

        private IList<UngradedTerm> GetUngradedTerms()
        {
            string json = "[{\"Code\":\"16/WI\",\"Description\":\"WINTER 2016\",\"StartDate\":\"2016-01-09T00:00:00\",\"EndDate\":\"2016-04-29T00:00:00\",\"ReportingYear\":2015,\"Sequence\":5,\"ReportingTerm\":\"16/WI\",\"FinancialPeriod\":\"Past\",\"DefaultOnPlan\":true,\"IsActive\":true,\"RegistrationDates\":[{\"RegistrationStartDate\":\"2015-11-02T00:00:00\",\"RegistrationEndDate\":\"2016-01-08T00:00:00\",\"PreRegistrationStartDate\":\"2015-10-22T00:00:00\",\"PreRegistrationEndDate\":\"2015-11-01T00:00:00\",\"AddStartDate\":\"2016-01-09T00:00:00\",\"AddEndDate\":\"2016-04-29T00:00:00\",\"DropStartDate\":\"2016-01-09T00:00:00\",\"DropEndDate\":\"2016-04-29T00:00:00\",\"DropGradeRequiredDate\":null,\"Location\":\"\"}],\"FinancialAidYears\":[]}]";
            var jsonSerializer = new JavaScriptSerializer();
            return jsonSerializer.Deserialize<IList<UngradedTerm>>(json);
        }
    }
}
