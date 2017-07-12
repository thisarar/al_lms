using AlLibraryManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlLibraryManagementSystem.DataAccess
{
    class BorrowedBooksDataAccess
    {
        public static bool Insert(BorrowedBy borrowed)
        {
            try
            {
                library_dbEntities dbContext = new library_dbEntities();
                dbContext.BorrowedBies.Add(borrowed);
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static bool Update(BorrowedBy borrowed)
        {
            try
            {
                library_dbEntities dbContextBorrowed = new library_dbEntities();
                BorrowedBy borrowedBy = (from bb in dbContextBorrowed.BorrowedBies
                                 where bb.BorrowedId == borrowed.BorrowedId
                                 select bb).SingleOrDefault();

                borrowedBy.returned = borrowed.returned;
                dbContextBorrowed.SaveChanges();
                 return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static List<BorrowedBy> SearchBorrowedBooks(int memberId)
        {
            library_dbEntities dbContextBorrowed = new library_dbEntities();
            members_dbEntities memberDbContext = new members_dbEntities();
            var memberIds = (from m in memberDbContext.Members
                       where m.MemberId == memberId
                       select m).ToArray();

            var borrowedMemberIds = (from m in dbContextBorrowed.BorrowedBies
                        select m).ToArray();

            var books = (from bm in borrowedMemberIds where bm.returned == 0
                         join m in memberIds
                         on bm.MemberId equals m.MemberId
                         select bm);

             return books.ToList();
        }
    }
}
