using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlLibraryManagementSystem.Models;

namespace AlLibraryManagementSystem.DataAccess
{
    /**
     * Access to the database to perform all CRUD and and other DB operations.
     * This class operates with the LINQ model of User in the database.
     **/
    class UserDataAccess
    {
        /**
         * Fetches user by username and password. If the user exists in the database, this
         * returns true if the credentials match with a record. If not, this returns false.
         **/
        public static bool Login(User user)
        {
            library_dbEntities dataContext = new library_dbEntities();
            try
            {
                var memberName = dataContext.Users.First().username;
                Console.WriteLine(memberName);
                User logingUser = (from User in dataContext.Users where User.username.Equals(user.username) && User.password.Equals(user.password) select User).FirstOrDefault();
                if (logingUser != null)
                {
                    return true;
                }
                return false;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public static bool CreateUser(User user)
        {
            library_dbEntities dataContext = new library_dbEntities();
            try
            {
                dataContext.Users.Add(user);
                dataContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
