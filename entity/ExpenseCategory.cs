using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entity
{
    public class ExpenseCategory
    {
        public int CategoryId { get; set; }          
        public string CategoryName { get; set; }     

        
        public ExpenseCategory() { }


        public ExpenseCategory(int categoryId, string categoryName)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
        }
    }
}

