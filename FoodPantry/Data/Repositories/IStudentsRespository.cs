using FoodPantry.Data.Models;
using FoodPantry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPantry.Data.Repositories
{
    interface IStudentsRespository
    {
        Student Find(string studentId);

        IList<UngradedTerm> FindAllUngradedTerms(string studentId);
    }
}
