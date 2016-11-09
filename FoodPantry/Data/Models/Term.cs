using System;

namespace FoodPantry.Data.Models
{
    public class Term
    {
        public string Code;
        public string Description;
        public DateTime StartDate;
        public DateTime EndDate;
        public string ReportingYear;
        public string Sequence;
        public string ReportingTerm;
        public string FinancialPeriod;
        public bool DefaultOnPlan;
        public bool IsActive;
    }
}