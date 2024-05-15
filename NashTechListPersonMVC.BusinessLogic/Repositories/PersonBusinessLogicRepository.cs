using ClosedXML.Excel;
using NashTechListPersonMVC.BusinessLogic.Interfaces;
using NashTechListPersonMVC.Model.Models;

namespace NashTechListPersonMVC.BusinessLogic.Repositories
{
    public class PersonBusinessLogicRepository : IPersonBusinessLogic
    {
        
        public static List<Person> ListPersonData = new List<Person>
        {

          new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe1", Gender = GenderType.Male, DateOfBirth = new DateTime(1991, 1, 15), PhoneNumber = "1234567890", BirthPlace = "New York", IsGraduated = true },
          new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe2", Gender = GenderType.Female, DateOfBirth = new DateTime(1991, 5, 15), PhoneNumber = "1234567890", BirthPlace = "Ha Noi", IsGraduated = true },
          new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe3", Gender = GenderType.Male, DateOfBirth = new DateTime(1992, 5, 15), PhoneNumber = "1234567890", BirthPlace = "New York", IsGraduated = true },
          new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe4", Gender = GenderType.Female, DateOfBirth = new DateTime(1993, 5, 15), PhoneNumber = "1234567890", BirthPlace = "New York", IsGraduated = true },
          new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe5", Gender = GenderType.Male, DateOfBirth = new DateTime(1994, 5, 15), PhoneNumber = "1234567890", BirthPlace = "Ha Noi", IsGraduated = false },
          new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe6", Gender = GenderType.Female, DateOfBirth = new DateTime(2000, 5, 15), PhoneNumber = "1234567890", BirthPlace = "New York", IsGraduated = false },
          new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe7", Gender = GenderType.Female, DateOfBirth = new DateTime(2000, 5, 15), PhoneNumber = "1234567890", BirthPlace = "New York", IsGraduated = true },
          new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe8", Gender = GenderType.Female, DateOfBirth = new DateTime(2002, 5, 15), PhoneNumber = "1234567890", BirthPlace = "Ha Noi", IsGraduated = false },
          new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe9", Gender = GenderType.Male, DateOfBirth = new DateTime(2002, 5, 15), PhoneNumber = "1234567890", BirthPlace = "New York", IsGraduated = true },
          new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe10", Gender = GenderType.Male, DateOfBirth = new DateTime(1998, 5, 15), PhoneNumber = "1234567890", BirthPlace = "Ha Noi", IsGraduated = true },
          new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe11", Gender = GenderType.Female, DateOfBirth = new DateTime(1998, 5, 15), PhoneNumber = "1234567890", BirthPlace = "New York", IsGraduated = true },
          new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe12", Gender = GenderType.Male, DateOfBirth = new DateTime(2005, 5, 15), PhoneNumber = "1234567890", BirthPlace = "New York", IsGraduated = false },
          new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe13", Gender = GenderType.Male, DateOfBirth = new DateTime(1999, 5, 15), PhoneNumber = "1234567890", BirthPlace = "New York", IsGraduated = true },
          new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe14", Gender = GenderType.Female, DateOfBirth = new DateTime(2000, 5, 15), PhoneNumber = "1234567890", BirthPlace = "New York", IsGraduated = false },
          new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe15", Gender = GenderType.Male, DateOfBirth = new DateTime(1998, 5, 15), PhoneNumber = "1234567890", BirthPlace = "Ha Noi", IsGraduated = true },
          new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe16", Gender = GenderType.Male, DateOfBirth = new DateTime(1996, 5, 15), PhoneNumber = "1234567890", BirthPlace = "New York", IsGraduated = false },
          new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe17", Gender = GenderType.Female, DateOfBirth = new DateTime(2000, 5, 15), PhoneNumber = "1234567890", BirthPlace = "New York", IsGraduated = true },
          new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe18", Gender = GenderType.Female, DateOfBirth = new DateTime(1994, 5, 15), PhoneNumber = "1234567890", BirthPlace = "New York", IsGraduated = true },
          new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe19", Gender = GenderType.Female, DateOfBirth = new DateTime(2000, 5, 15), PhoneNumber = "1234567890", BirthPlace = "New York", IsGraduated = true },
          new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe20", Gender = GenderType.Female, DateOfBirth = new DateTime(1993, 5, 15), PhoneNumber = "1234567890", BirthPlace = "Ha Noi", IsGraduated = true },

        };

        public Task<IEnumerable<Person>> GetAllMember()
        {
            return Task.FromResult<IEnumerable<Person>>(ListPersonData);
        }

        public Task<IEnumerable<Person>> GetMaleMembers()
        {
            var maleMembers = ListPersonData.Where(p => p.Gender == GenderType.Male).ToList();
            return Task.FromResult<IEnumerable<Person>>(maleMembers);
        }
        public async Task<byte[]> ExportExcelFile()
        {
            var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("PersonList");

            worksheet.Cell(1, 1).Value = "Full Name";
            worksheet.Cell(1, 2).Value = "Age";
            worksheet.Cell(1, 3).Value = "Gender";
            worksheet.Cell(1, 4).Value = "Date of Birth";
            worksheet.Cell(1, 5).Value = "Phone Number";
            worksheet.Cell(1, 6).Value = "Birth Place";
            worksheet.Cell(1, 7).Value = "Graduated";

            int row = 2;

            foreach (var person in ListPersonData)
            {
                worksheet.Cell(row, 1).Value = person.FullName;
                worksheet.Cell(row, 2).Value = person.Age;
                worksheet.Cell(row, 3).Value = person.Gender.ToString();
                worksheet.Cell(row, 4).Value = person.DateOfBirth.ToShortDateString();
                worksheet.Cell(row, 5).Value = person.PhoneNumber;
                worksheet.Cell(row, 6).Value = person.BirthPlace;
                worksheet.Cell(row, 7).Value = person.IsGraduated ? "Yes" : "No";
                
                row++;
            }
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
        }
        public Task<IEnumerable<Person>> FilterPersonListByYear(string filter)
        {
            IEnumerable<Person> filteredList;

            if (filter == "lessthan2000")
            {
                filteredList = ListPersonData.Where(m => m.DateOfBirth.Year < 2000);
            }
            else if (filter == "equal2000")
            {
                filteredList = ListPersonData.Where(m => m.DateOfBirth.Year == 2000);
            }
            else if (filter == "greaterthan2000")

			{
                filteredList = ListPersonData.Where(m => m.DateOfBirth.Year > 2000);
            }
            else
            {
                filteredList = ListPersonData;  
            }

            return Task.FromResult<IEnumerable<Person>>(filteredList);
        }

        public Task<IEnumerable<Person>> GetOldestMember()
        {
            var oldestMember = ListPersonData.OrderByDescending(p => p.Age).ThenBy(m => m.DateOfBirth).Take(1).ToList();
            return Task.FromResult<IEnumerable<Person>>(oldestMember);
        }

        public Task<Person> GetByIdAsync(Guid id)
        {
            var person = ListPersonData.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(person);
        }
        public bool Add(Person person)
        {
            try
            {
                person.Id = Guid.NewGuid();
                ListPersonData.Add(person);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(Person person)
        {
            try
            {
                var existingPerson = ListPersonData.FirstOrDefault(p => p.Id == person.Id);

                if (existingPerson == null)
                {
                    return false;
                }

                existingPerson.FirstName = person.FirstName;
                existingPerson.LastName = person.LastName;
                existingPerson.Gender = person.Gender;
                existingPerson.DateOfBirth = person.DateOfBirth;
                existingPerson.PhoneNumber = person.PhoneNumber;
                existingPerson.BirthPlace = person.BirthPlace;
                existingPerson.IsGraduated = person.IsGraduated;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Delete(Person person)
        {
           
        ListPersonData.Remove(person);
        return true;
            
        }
    }
}
