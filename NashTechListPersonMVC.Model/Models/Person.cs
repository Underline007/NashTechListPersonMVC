using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NashTechListPersonMVC.Model.Models
{
    public class Person
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public GenderType Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string BirthPlace { get; set; } = string.Empty;
        public bool IsGraduated { get; set; }
        public int Age => DateTime.Now.Year - DateOfBirth.Year;

    }
}
