using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodPantry.Data.Models
{
    public class Student
    {
        public string Id;
        public string LastName;
        public string FirstName;
        public string MiddleName;
        public string BirthNameLast;
        public string BirthNameFirst;
        public string BirthNameMiddle;
        public string PreferredName;
        public IList<string> PreferredAddress;
        public string Prefix;
        public string Suffix;
        public string Gender;
        public DateTime BirthDate;
        public string GovernmentId;
        public IList<string> RaceCodes;
        public IList<string> EthnicCodes;
        public IList<string> Ethnicities;
	    public string MaritalStatus;
        public long DegreePlanId;
        public IList<string> ProgramIds;
        public IList<string> StudentRestrictionIds;
        public bool? HasAdvisor;
        public string PreferredEmailAddress;
        public bool? IsLegacyStudent;
        public bool? IsFirstGenerationStudent;
        public bool? IsInternationalStudent;
        public IList<string> AdmitTerms;
        public IList<string> AcademicLevelCodes;
        public string ResidencyStatus;
        public string StudentTypeCode;
        public IList<string> ClassLevelCodes;
        public bool? IsConfidential;
        public bool? IsTransfer;
        public string FinancialAidCounselorId;
        public IList<EmailAddress> EmailAddresses;
    }
}