using System.Collections.Generic;

namespace MVCContactBook.ViewModel
{
    public class ContactModel
    {
        public int PersonID { get; set; }
        public int OrganizationID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public int ContactNo { get; set; }
        public string Email { get; set; }
        public string OrganizationName { get; set; }

    }
    public class ContactModel2
    {
        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public string Address { get; set; }
        public int ContactNo { get; set; }
        public string Email { get; set; }
    }
    public class allName
    {
        public int organizationID { get; set; }
        public int OrganizationName { get; set; }
    }


    
}