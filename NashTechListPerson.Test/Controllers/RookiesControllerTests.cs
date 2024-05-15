using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using NashTechListPersonMVC.BusinessLogic.Interfaces;
using NashTechListPersonMVC.Model.Models;
using NashTechListPersonMVC.WebApp.Areas.NashTech.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NashTechListPersonMVC.BusinessLogic.ViewModels;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NashTechListPersonMVC.Tests.Controllers
{
    public class RookiesControllerTests
    {
        private readonly Mock<IPersonBusinessLogic> _mockPersonBusinessLogic;
        private readonly RookiesController _controller;

        public RookiesControllerTests()
        {
            _mockPersonBusinessLogic = new Mock<IPersonBusinessLogic>();
            _controller = new RookiesController(_mockPersonBusinessLogic.Object);
        }

        [Fact]
        public async Task Index_ListPerson_ReturnsAllMembers()
        {
            // Arrange
            var mockMembers = new List<Person>
            {
                new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Gender = GenderType.Male },
                new Person { Id = Guid.NewGuid(), FirstName = "Lisa", LastName = "Maria", Gender = GenderType.Female },
                new Person { Id = Guid.NewGuid(), FirstName = "Leston", LastName = "Store", Gender = GenderType.Male }
            };
            _mockPersonBusinessLogic.Setup(x => x.GetAllMember()).ReturnsAsync(mockMembers);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Person>>(viewResult.ViewData.Model);
            Assert.Equal(3, model.Count());
        }

        [Fact]
        public async Task Index_Exception_ReturnsExceptionErrors()
        {
            // Arrange
            _mockPersonBusinessLogic.Setup(bl => bl.GetAllMember()).ThrowsAsync(new Exception("Throw exception"));

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Error", viewResult.ViewName);
            Assert.Equal("Throw exception", viewResult.ViewData["ErrorMessage"]);
        }

        // checkkkkkkkkkkkkkkkkkkkkkkkkkkkk
        [Fact]
        public async Task DisplayOldestMember_OldestMember_ReturnsViewWithOldestMember()
        {
            // Arrange
            var expectedMember = new Person { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Gender = GenderType.Male, DateOfBirth= new DateTime(1991 ,1, 1) };
            var expectedMembers = new List<Person> { expectedMember };
            _mockPersonBusinessLogic.Setup(m => m.GetOldestMember()).Returns(Task.FromResult(expectedMembers.AsEnumerable()));

            // Act
            var result = await _controller.DisplayOldestMember();

            // Assert
            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);
            Assert.Equal("Index", viewResult.ViewName);

            var model = viewResult.ViewData.Model;
            Assert.IsType<List<Person>>(model);

            var personList = model as List<Person>;
            Assert.NotNull(personList);
            Assert.Single(personList);

            var personModel = personList.First();
            Assert.Equal(expectedMember.Id, personModel.Id);
            Assert.Equal(expectedMember.FirstName, personModel.FirstName);
            Assert.Equal(expectedMember.LastName, personModel.LastName);
            Assert.Equal(expectedMember.Gender, personModel.Gender);
        }



        [Fact]
        public async Task DisplayOldestMember_Exception_ReturnsExceptionErrors()
        {
            // Arrange 
            var expectedException = new Exception("Throw exception");
            _mockPersonBusinessLogic.Setup(m => m.GetOldestMember()).ThrowsAsync(expectedException);

            // Act 
            var result = await _controller.DisplayOldestMember();

            // Assert 
            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);
            Assert.Equal("Error", viewResult.ViewName);

            Assert.NotNull(viewResult.ViewData["ErrorMessage"]);
            Assert.Equal(expectedException.Message, viewResult.ViewData["ErrorMessage"].ToString());
        }

        [Fact]
        public async Task DisplayMaleMembers_ListMember_ReturnsViewWithMaleMembers()
        {
            // Arrange
            var maleMembers = new List<Person>
        {
            new Person { FirstName = "John",LastName="Jack", Gender = GenderType.Male },
            new Person { FirstName = "David",LastName="Jack", Gender = GenderType.Male },
            new Person { FirstName = "David",LastName="Jack", Gender = GenderType.Female }


        };
            _mockPersonBusinessLogic.Setup(bl => bl.GetMaleMembers()).ReturnsAsync(maleMembers);

            // Act
            var result = await _controller.DisplayMaleMembers() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ViewName);
            Assert.Equal(maleMembers, result.Model);
        }

        [Fact]
        public async Task DisplayMaleMembers_Exception_ReturnsErrorView()
        {
            // Arrange
            var exceptionMessage = "Throw Exception";
            _mockPersonBusinessLogic.Setup(bl => bl.GetMaleMembers()).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.DisplayMaleMembers() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Error", result.ViewName);
            Assert.Equal(exceptionMessage, _controller.ViewBag.ErrorMessage);
        }


        [Fact]
        public async Task FilterMember_Equal2000_ReturnFilteredMembers()
        {
            // Arrange
            var filter = "equal2000";
            var mockMembers = new List<Person>
            {
                new Person { Id = Guid.NewGuid(), FirstName = "Dung", LastName = "Tran", Gender = GenderType.Male, DateOfBirth = new DateTime(2000-1-1) }
            };
            _mockPersonBusinessLogic.Setup(x => x.FilterPersonListByYear(filter)).ReturnsAsync(mockMembers);

            // Act
            var result = await _controller.FilterMember(filter);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Person>>(viewResult.ViewData.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task FilterMember_Exception_ReturnsExceptionErrors()
        {
            // Arrange
            var filter = "equal2000";
            _mockPersonBusinessLogic.Setup(x => x.FilterPersonListByYear(filter)).ThrowsAsync(new Exception("Throw exception"));

            // Act
            var result = await _controller.FilterMember(filter);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Error", viewResult.ViewName);
            Assert.Equal("Throw exception", viewResult.ViewData["ErrorMessage"]);
        }

        [Fact]
        public async Task ExportExcel_DownloadExcelFile_ReturnsFile()
        {
            // Arrange
            var mockData = new byte[] { 1, 2, 3 };
            _mockPersonBusinessLogic.Setup(bl => bl.ExportExcelFile()).ReturnsAsync(mockData);

            // Act
            var result = await _controller.ExportExcel();

            // Assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileResult.ContentType);
            Assert.Equal("PersonList.xlsx", fileResult.FileDownloadName);
        }

        [Fact]
        public async Task ExportExcel_Exception_ReturnExceptionError()
        {
            // Arrange
            _mockPersonBusinessLogic.Setup(x => x.ExportExcelFile()).ThrowsAsync(new Exception("Throw exception"));

            // Act
            var result = await _controller.ExportExcel();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Error", viewResult.ViewName);
            Assert.Equal("Throw exception", viewResult.ViewData["ErrorMessage"]);
        }

        [Fact]
        public void AddPerson_ValidModel_ReturnTrue()
        {
            // Arrange
            var model = new PersonCreateEditViewModel
            {
                FirstName = "Tran",
                LastName = "Dung",
                Gender = GenderType.Male,
                DateOfBirth = new DateTime(2002, 1, 1),
                PhoneNumber = "123456789",
                BirthPlace = "Ha Noi",
                IsGraduated = true
            };
            _mockPersonBusinessLogic.Setup(x => x.Add(It.IsAny<Person>())).Returns(true);

            // Act
            var result = _controller.AddPerson(model);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void AddPerson_Exception_ReturnsExceptionError()
        {
            // Arrange
            var model = new PersonCreateEditViewModel
            {
                FirstName = "Tran",
                LastName = "Dung",
                Gender = GenderType.Male,
                DateOfBirth = new DateTime(2002, 1, 1),
                PhoneNumber = "123456789",
                BirthPlace = "Ha Noi",
                IsGraduated = true
            };
            _mockPersonBusinessLogic.Setup(x => x.Add(It.IsAny<Person>())).Returns(false);

            // Act
            var result = _controller.AddPerson(model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Error", viewResult.ViewName);
            Assert.Equal("Failed to add person. Please try again.", viewResult.ViewData["ErrorMessage"]);
        }

        [Fact]
        public async Task Update_CheckCondition_ValidId_ReturnValidPersonToEdit()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var mockPerson = new Person
            {
                FirstName = "Tran",
                LastName = "Dung",
                Gender = GenderType.Male,
                DateOfBirth = new DateTime(2002, 1, 1),
                PhoneNumber = "123456789",
                BirthPlace = "Ha Noi",
                IsGraduated = true
            };
        _mockPersonBusinessLogic.Setup(x => x.GetByIdAsync(personId)).ReturnsAsync(mockPerson);

        // Act
        var result = await _controller.Update(personId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<PersonCreateEditViewModel>(viewResult.ViewData.Model);
        Assert.Equal(mockPerson.Id, model.Id);
        }

        [Fact]
        public async Task Update_CheckCondition_InvalidId_ReturnsNotFoundPerson()
        {
            // Arrange
            var personId = Guid.NewGuid();
            _mockPersonBusinessLogic.Setup(x => x.GetByIdAsync(personId)).ReturnsAsync((Person)null);

            // Act
            var result = await _controller.Update(personId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Update_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var mockPerson = new Person
            {
               
                FirstName = "Tran",
                LastName = "Dung",
                Gender = GenderType.Male,
                DateOfBirth = new DateTime(2002, 1, 1),
                PhoneNumber = "123456789",
                BirthPlace = "Ha Noi",
                IsGraduated = true
            };
            _mockPersonBusinessLogic.Setup(x => x.GetByIdAsync(personId)).ReturnsAsync(mockPerson);
            _mockPersonBusinessLogic.Setup(x => x.Update(It.IsAny<Person>())).Returns(true);

            var model = new PersonCreateEditViewModel
            {
                FirstName = "Tran Upadte",
                LastName = "Dung Update",
                Gender = GenderType.Male,
                DateOfBirth = new DateTime(2002, 1, 1),
                PhoneNumber = "123456789",
                BirthPlace = "Ha Noi Capital",
                IsGraduated = true
            };

            // Act
            var result = await _controller.Update(personId, model);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
        
        [Fact]
        public async Task Update_ValidModel_ReturnsException_WhenUpdateFails()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var mockPerson = new Person
            {
                FirstName = "Tran",
                LastName = "Dung",
                Gender = GenderType.Male,
                DateOfBirth = new DateTime(2002, 1, 1),
                PhoneNumber = "123456789",
                BirthPlace = "Ha Noi",
                IsGraduated = true
            };
            _mockPersonBusinessLogic.Setup(x => x.GetByIdAsync(personId)).ReturnsAsync(mockPerson);
            _mockPersonBusinessLogic.Setup(x => x.Update(It.IsAny<Person>())).Returns(false);

            var model = new PersonCreateEditViewModel
            {
                FirstName = "Tran Upadte",
                LastName = "Dung Update",
                Gender = GenderType.Male,
                DateOfBirth = new DateTime(2002, 1, 1),
                PhoneNumber = "123456789",
                BirthPlace = "Ha Noi Capital",
                IsGraduated = true
            };

            // Act
            var result = await _controller.Update(personId, model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Error", viewResult.ViewName);
            Assert.Equal("Failed to update person. Please try again.", viewResult.ViewData["ErrorMessage"]);
        }

        [Fact]
        public async Task Delete_CheckCondition_ValidId_ReturnsViewResult_WithPersonDetails()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var mockPerson = new Person { Id = personId, FirstName = "Dung", LastName = "Tran" , DateOfBirth = new DateTime(2000-1-1)};
            _mockPersonBusinessLogic.Setup(x => x.GetByIdAsync(personId)).ReturnsAsync(mockPerson);

            // Act
            var result = await _controller.Delete(personId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Person>(viewResult.ViewData.Model);
            Assert.Equal(mockPerson.Id, model.Id);
        }

        [Fact]
        public async Task Delete_CheckCondition_InvalidId_ReturnsErrorView()
        {
            // Arrange
            var personId = Guid.NewGuid();
            _mockPersonBusinessLogic.Setup(x =>  x.GetByIdAsync(personId)).ReturnsAsync((Person)null);

            // Act
            var result = await _controller.Delete(personId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Error", viewResult.ViewName);
        }

        [Fact]
        public async Task DeletePerson_ValidId_RedirectsToDeleteConfirm()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var mockPerson = new Person { Id = personId, FirstName = "Dung", LastName = "Tran", DateOfBirth = new DateTime(2000 - 1 - 1) };
            _mockPersonBusinessLogic.Setup(x => x.GetByIdAsync(personId)).ReturnsAsync(mockPerson);

            // Act
            var result = await _controller.DeletePerson(personId);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("DeleteComfirm", redirectToActionResult.ActionName);
            Assert.Equal(mockPerson.FullName, redirectToActionResult.RouteValues["deletedPersonName"]);
        }

        [Fact]
        public async Task DeletePerson_InvalidId_ReturnsErrorView()
        {
            // Arrange
            var personId = Guid.NewGuid();
            _mockPersonBusinessLogic.Setup(x => x.GetByIdAsync(personId)).ReturnsAsync((Person)null);

            // Act
            var result = await _controller.DeletePerson(personId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Error", viewResult.ViewName);
        }

        [Fact]
        public async Task Detail_ValidId_ReturnsViewResult_WithPersonDetails()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var mockPerson = new Person { Id = personId, FirstName = "Dung", LastName = "Tran", DateOfBirth = new DateTime(2000 - 1 - 1) };
            _mockPersonBusinessLogic.Setup(x => x.GetByIdAsync(personId)).ReturnsAsync(mockPerson);

            // Act
            var result = await _controller.Detail(personId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Person>(viewResult.ViewData.Model);
            Assert.Equal(mockPerson.Id, model.Id);
        }

        [Fact]
        public async Task Detail_InvalidId_ReturnsErrorView()
        {
            // Arrange
            var personId = Guid.NewGuid();
            _mockPersonBusinessLogic.Setup(bl => bl.GetByIdAsync(personId)).ReturnsAsync((Person)null);

            // Act
            var result = await _controller.Detail(personId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Error", viewResult.ViewName);
        }
    }
}
