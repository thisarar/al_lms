using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlLibraryManagementSystem.Models;

namespace AlLibraryManagementSystem.DataAccess
{
    class CategoryDataAccess
    {
        public List<Category> AllCategories()
        {
            using (library_dbEntities context = new library_dbEntities())
            {
                return context.Categories.Select(category => category).ToList<Category>();
            }
        }
    }
}
