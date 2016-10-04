using FoodPantry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPantry.Data.Repositories
{
    interface ITermsRespository
    {
        IList<Term> FindAllRegistrationTerms();
    }
}
