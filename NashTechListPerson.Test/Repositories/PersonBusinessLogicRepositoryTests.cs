using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using NashTechListPersonMVC.BusinessLogic.Repositories;
using NashTechListPersonMVC.Model.Models;

namespace NashTechListPersonMVC.Tests
{
    public class PersonBusinessLogicRepositoryTests
    {
        private readonly PersonBusinessLogicRepository _mockRepository;
        private List<Person> _testData;

        public PersonBusinessLogicRepositoryTests()
        {
            _mockRepository = new PersonBusinessLogicRepository();
        }

        private void InitializeTestData()
        {
            _testData = new List<Person>
            {
                new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe1", Gender = GenderType.Male, DateOfBirth = new DateTime(2002, 1, 15), PhoneNumber = "1234567890", BirthPlace = "New York", IsGraduated = true },
                new Person { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Doe2", Gender = GenderType.Female, DateOfBirth = new DateTime(2000, 5, 15), PhoneNumber = "1234567890", BirthPlace = "Ha Noi", IsGraduated = true },
                new Person { Id = Guid.NewGuid(), FirstName = "Jim", LastName = "Doe3", Gender = GenderType.Male, DateOfBirth = new DateTime(1992, 5, 15), PhoneNumber = "1234567890", BirthPlace = "New York", IsGraduated = true },
                // Add more test data as needed
            };

            
            PersonBusinessLogicRepository.ListPersonData = _testData;
        }

        [Fact]
        public async Task GetAllMember_HasData_ReturnsAllMembers()
        {
            // Arrange
            InitializeTestData();

            // Act
            var result = await _mockRepository.GetAllMember();

            // Assert
            Assert.Equal(_testData.Count, result.Count());
        }

        [Fact]
        public async Task GetMaleMembers_HasData_ReturnsOnlyMaleMembers()
        {
            // Arrange
            InitializeTestData();

            // Act
            var result = await _mockRepository.GetMaleMembers();

            // Assert
            Assert.All(result, p => Assert.Equal(GenderType.Male, p.Gender));
        }

        [Fact]
        public async Task ExportExcelFile_HasData_ReturnsNonEmptyByteArray()
        {
            // Arrange
            InitializeTestData();

            // Act
            var result = await _mockRepository.ExportExcelFile();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData("lessthan2000", 1)] 
        [InlineData("equal2000", 1)]
        [InlineData("greaterthan2000", 1)]
        public async Task FilterPersonListByYear_FilterString_ReturnsFilteredResults(string filter, int expectedCount)
        {
            // Arrange
            InitializeTestData();

            // Act
            var result = await _mockRepository.FilterPersonListByYear(filter);

            // Assert
            Assert.Equal(expectedCount, result.Count());
        }

        [Fact]
        public async Task GetOldestMember_ValidPerson_ReturnsOldestMember()
        {
            // Arrange
            InitializeTestData();

            // Act
            var result = await _mockRepository.GetOldestMember();

            // Assert
            var expected = _testData.OrderByDescending(p => p.Age).ThenBy(m => m.DateOfBirth).First();
            Assert.Equal(expected, result.First());
        }

        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsCorrectPerson()
        {
            // Arrange
            InitializeTestData();
            var expectedPerson = _testData.First();

            // Act
            var result = await _mockRepository.GetByIdAsync(expectedPerson.Id);

            // Assert
            Assert.Equal(expectedPerson, result);
        }

        [Fact]
        public void Add_ValidModel_AddsNewPerson()
        {
            // Arrange
            InitializeTestData();
            var newPerson = new Person
            {
                FirstName = "Tran",
                LastName = "Dung",
                Gender = GenderType.Male,
                DateOfBirth = new DateTime(2000, 1, 1),
                PhoneNumber = "0987654321",
                BirthPlace = "Ha Noi",
                IsGraduated = true
            };

            // Act
            var result = _mockRepository.Add(newPerson);

            // Assert
            Assert.True(result);
            Assert.Contains(newPerson, PersonBusinessLogicRepository.ListPersonData);
        }

        [Fact]
        public void Update_ValidPerson_UpdatesExistingPerson()
        {
            // Arrange
            InitializeTestData();
            var existingPerson = _testData.First();
            var updatedPerson = new Person
            {
                Id = existingPerson.Id,
                FirstName = "Tran",
                LastName = "Dung Update",
                Gender = GenderType.Male,
                DateOfBirth = existingPerson.DateOfBirth,
                PhoneNumber = "0987654321",
                BirthPlace = "Ha Noi",
                IsGraduated = true
            };

            // Act
            var result = _mockRepository.Update(updatedPerson);

            // Assert
            Assert.True(result);
            var person = PersonBusinessLogicRepository.ListPersonData.First(p => p.Id == updatedPerson.Id);
            Assert.Equal("Tran", person.FirstName);
            Assert.Equal("Dung Update", person.LastName);
            Assert.True(person.IsGraduated);
        }

        [Fact]
        public void Delete_ValidPerson_RemovesPerson()
        {
            // Arrange
            InitializeTestData();
            var personToDelete = _testData.First();

            // Act
            var result = _mockRepository.Delete(personToDelete);

            // Assert
            Assert.True(result);
            Assert.DoesNotContain(personToDelete, PersonBusinessLogicRepository.ListPersonData);
        }
    }
}
