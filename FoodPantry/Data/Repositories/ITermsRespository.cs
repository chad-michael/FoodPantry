using FoodPantry.Data.Models;
using System.Collections.Generic;

namespace FoodPantry.Data.Repositories
{
    internal interface ITermsRespository
    {
        IList<Term> FindAllRegistrationTerms();
    }
}