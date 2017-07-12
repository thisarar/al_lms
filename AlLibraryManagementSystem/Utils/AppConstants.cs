using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlLibraryManagementSystem.Utils
{
    /**
     * Holds the application constants and alert messages. All the Message boxes messages 
     * and titles should be defined here. The other constants that related to the code also
     * should be defined here.
     **/
    public static class AppConstants
    {
        public static string LoginErrorTitle { get { return "Invalid Credetials"; } }
        public static string LoginErrorMessage { get { return "Please check your username or password!"; } }
        public static string RequiredFieldsTitle { get { return "Data Required"; } }
        public static string RequiredFieldsMessage { get { return "Please fill the required fields!"; } }
        public static string BookCollectedTitle { get { return "Book Collected"; } }
        public static string BookCollectedMessage { get { return "Selected book has been returned. And you charged the fine if have."; } }
        public static string ErrorTitle { get { return "Error"; } }
        public static string ErrorMessage { get { return "Could not proceed your request"; } }
        public static string SelectTitle { get { return "Select an Entry"; } }
        public static string SelectMessage { get { return "Please select a row in order to process"; } }
        public static string SelectBookMessage { get { return "Please select a book in order to issue"; } }
        public static string IssueSuccessMessage { get { return "Book successfully issued to the selected member"; } }
        public static string IssueSuccessTitle { get { return "Success"; } }
        public static string SelectMemberMessage { get { return "Please select a member in order to issue the book"; } }
        public static string SelectMemberTitle { get { return "Error"; } }
        public static string BookNotAvailableMessage { get { return "Requested book is not available at the moment"; } }
        public static string DatabaseErrorMessage { get { return "Error while creating databases."; } }
    }
}
