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
    public class ColleagueWsTermsRepositoryUnitTest
    {
        [TestMethod]
        public void TestJsonParsing()
        {
            Student student = GetStudent();
            Assert.IsTrue(student.EmailAddresses.Count > 0);
        }

        private Student GetStudent()
        {
            string json = "{\"DegreePlanId\":11271,\"ProgramIds\":[\"AS.40000\"],\"StudentRestrictionIds\":[\"256481\",\"250051\"],\"HasAdvisor\":false,\"PreferredEmailAddress\":\"amyconrad@delta.edu\",\"IsLegacyStudent\":false,\"IsFirstGenerationStudent\":null,\"IsInternationalStudent\":false,\"AdmitTerms\":[\"15/WI\"],\"AcademicLevelCodes\":[\"UG\"],\"ResidencyStatus\":\"In-District\",\"HighSchoolGpas\":[{\"HighSchoolId\":\"1198776\",\"Gpa\":null,\"LastAttendedYear\":\"1986\"}],\"Advisements\":[],\"StudentTypeCode\":\"DS\",\"ClassLevelCodes\":[\"FR\"],\"IsConfidential\":false,\"AdvisorIds\":[],\"IsTransfer\":false,\"FinancialAidCounselorId\":\"1184295\",\"EmailAddresses\":[{\"Value\":\"amyconrad@delta.edu\",\"TypeCode\":\"INT\",\"IsPreferred\":true},{\"Value\":\"amy.conrad68@yahoo.com\",\"TypeCode\":\"WWW\",\"IsPreferred\":false}],\"Id\":\"1466812\",\"LastName\":\"Conrad\",\"FirstName\":\"Amy\",\"MiddleName\":\"Lynn\",\"BirthNameLast\":null,\"BirthNameFirst\":null,\"BirthNameMiddle\":null,\"PreferredName\":\"Amy L. Conrad\",\"PreferredAddress\":[\"600 N Van Buren Apt 425\",\"Bay City, MI 48708\"],\"Prefix\":\"\",\"Suffix\":\"\",\"Gender\":\"F\",\"BirthDate\":\"1968-08-01T00:00:00\",\"GovernmentId\":\"383-80-6666\",\"RaceCodes\":[\"WH\",\"UR\"],\"EthnicCodes\":[\"NHS\"],\"Ethnicities\":[\"White\"],\"MaritalStatus\":\"Single\"}";
            var jsonSerializer = new JavaScriptSerializer();
            return jsonSerializer.Deserialize<Student>(json);
        }
    }
}
