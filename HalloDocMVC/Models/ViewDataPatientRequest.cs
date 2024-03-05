namespace HalloDocMVC.Models
{
    public class ViewDataPatientRequest
    {
        public string Id { get; set; }
        public string Symptoms { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Relation { get; set; }
        public string RoomSuite { get; set; }
        public IFormFile? UploadFile { get; set; } 
        public string? UploadImage { get; set; }
    }
}