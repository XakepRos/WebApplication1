namespace WebApplication1.Models
{
    public class ProfileViewModel
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public DateTime?  DOB { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public  string? DepartmentType { get; set; }
        public string? Department { get; set; }
        public string? Description { get; set; }
        public string? Remarks { get; set; }
        public string? ProfileImage { get; set; }
        public string? ProfileImageUpload { get; set; }
        public string? DocumentType { get; set; }
        public string? DocumentUpload { get; set; }

    }
}
