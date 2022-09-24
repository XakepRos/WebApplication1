namespace WebApplication1.Models
{
    public class ProfileViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string  Address { get; set; }
        public DateTime Dob { get; set; }
        public int Age { get; set; }
        public string  Designation { get; set; }
        public string Department { get; set; }
        public string  DepartmentRole { get; set; }
        public double ContactNumber { get; set; }
        public string ProfileImg { get; set; }

        public string DocumentType { get; set; }
        public string DocumentName { get; set; }
        public string Document { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public string UploadDocument { get; set; }

    }
}
