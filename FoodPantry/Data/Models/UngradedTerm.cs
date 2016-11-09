using System;

namespace FoodPantry.Data.Models
{
    public class UngradedTerm
    {
        public string Code;
        public string Description;
        public DateTime? StartDate;
        public DateTime? EndDate;
        public long? ReportingYear;
        public long? Sequence;
        public string ReportingTerm;
        public string FinancialPeriod;
        public bool? DefaultOnPlan;
        public bool? IsActive;
    }
}