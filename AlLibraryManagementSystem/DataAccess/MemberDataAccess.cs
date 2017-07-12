using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlLibraryManagementSystem.Models;

namespace AlLibraryManagementSystem.DataAccess
{
    class MemberDataAccess
    {
        public static bool Insert(Member member)
        {
            try
            {
                members_dbEntities dbContext = new members_dbEntities();
                dbContext.Members.Add(member);
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static List<Member> ListMembers()
        {
            members_dbEntities dbContext = new members_dbEntities();
            var members = from m in dbContext.Members select m;
            return members.ToList();
        }

        public static Member FindMember(int memberId)
        {
            members_dbEntities dbContext = new members_dbEntities();
            return (from m in dbContext.Members where m.MemberId==memberId select m).Single();
            
        }
    }
}
