using System.Collections;

namespace HalloDocMVC.Models
{
    public class ViewDataUserProfile
    {
        public int? Userid { get; set; }

        public string? Aspnetuserid { get; set; }

        public string? Firstname { get; set; }

        public string? Lastname { get; set; }

        public string Email { get; set; }

        public string? Mobile { get; set; }

        public BitArray? Ismobile { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public int? Regionid { get; set; }

        public string? Zipcode { get; set; }

        public string? Strmonth { get; set; }
        public DateTime Birthdate { get; set; }

        public int? Intyear { get; set; }

        public int? Intdate { get; set; }

        public string Createdby { get; set; } = null!;

        public DateTime Createddate { get; set; }

        public string? Modifiedby { get; set; }

        public DateTime? Modifieddate { get; set; }

        public short? Status { get; set; }

        public string? Ip { get; set; }
    }
}