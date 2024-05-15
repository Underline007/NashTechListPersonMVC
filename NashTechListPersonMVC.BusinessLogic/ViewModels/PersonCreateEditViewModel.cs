using NashTechListPersonMVC.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NashTechListPersonMVC.BusinessLogic.ViewModels
{
    public class PersonCreateEditViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage ="First Name is required")]
        [StringLength(30, ErrorMessage = "First name must be less than 30 characters")]
        public string FirstName { get; set; } = String.Empty;

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(30, ErrorMessage = "Last name must be less than 30 characters")]
        public string LastName { get; set; } = String.Empty;
        
        [Required(ErrorMessage = "Gender is required")]
        public GenderType Gender { get; set; }

        [Required(ErrorMessage = "Last Name is required")]

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; } = String.Empty;

        public string BirthPlace { get; set; } = String.Empty;

        public bool IsGraduated { get; set; }
    }
}
