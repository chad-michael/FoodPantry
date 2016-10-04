using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodPantry.Data.Models
{
    public class GradeRestriction
    {
        public bool? IsRestricted;
        public IList<string> Reasons;
    }
}