using FoodPantry.Data.Models;
using System.Collections.Generic;

namespace FoodPantry.Data.Repositories
{
    internal interface IStudentsRespository
    {
        Student Find(string studentId);

        IList<UngradedTerm> FindAllUngradedTerms(string studentId);
    }
}