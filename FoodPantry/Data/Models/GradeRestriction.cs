using System.Collections.Generic;

namespace FoodPantry.Data.Models
{
    public class GradeRestriction
    {
        public bool? IsRestricted;
        public IList<string> Reasons;
    }
}