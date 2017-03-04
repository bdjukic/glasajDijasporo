using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlasajDijasporoService.Model
{
    public class VotingRequest
    {
        public string FirstLastName { get; set; }
        public string BirthPlaceDate { get; set; }
        public string Gender { get; set; }
        public string ParentName { get; set; }
        public string PersonalId { get; set; }
        public string PassportId { get; set; }
        public string HomeCountryAddress { get; set; }
        public string TemporaryAddress { get; set; }
        public string ForeignCountryName { get; set; }
        public string ForeignCountryAddress { get; set; }
        public string VotingLocation { get; set; }
        public string CurrentLocation { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Signature { get; set; }
    }
}