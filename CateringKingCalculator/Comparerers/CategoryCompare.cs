using hebestadt.CateringKingCalculator.Models;
using hebestadt.CateringKingCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hebestadt.CateringKingCalculator.Comparerers
{
    public class CategoryCompare : Comparer<MealItemViewModel>
    {
        public override int Compare(MealItemViewModel x, MealItemViewModel y)
        {
            if (x.CategoryId.CompareTo(y.CategoryId) != 0)
            {
                return x.CategoryId.CompareTo(y.CategoryId);
            }
            else
                return 0;
        }

    }
}
