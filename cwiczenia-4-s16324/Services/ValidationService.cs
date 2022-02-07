using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cwiczenia_4_s16324.Services
{
    public interface IValidationService
    {
        Boolean orderByValidation(string OrderByValue);
        
    }
    public class ValidationService : IValidationService
    {
        public Boolean orderByValidation(string OrderByCol)
        {
            string[] ColumnNames = {"Name", "Description", "Category", "Area"};
            if (ColumnNames.Contains(OrderByCol)){
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
