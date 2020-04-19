using SyncHRoner.Domain.Enums;
using SyncHRoner.Infrastructure.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SyncHRoner.Dtos
{
    public class ProfileDto
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        [ValidEnum(ErrorMessage = "Gender enum is invalid")]
        public GenderEnum Gender { get; set; }
        [ValidEnum(ErrorMessage = "Country enum is invalid")]
        public List<CountryEnum> Nationalities { get; set; } = new List<CountryEnum>();
        [RegularExpression(@"^(48)?\d{9}$", ErrorMessage = "Phone Number is invalid")]
        public string Phone { get; set; }
        [EmailAddress, MaxLength(255, ErrorMessage = "Email Address is too long")]
        public string Email { get; set; }
        [Range(0.0, 5.0, ErrorMessage = "Rating is invalid. Valid rating is between 0-5")]
        public double Rating { get; set; }
    }
}
