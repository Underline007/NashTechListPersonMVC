using Microsoft.AspNetCore.Mvc;
using NashTechListPersonMVC.BusinessLogic.Interfaces;
using NashTechListPersonMVC.BusinessLogic.ViewModels;
using NashTechListPersonMVC.Model.Models;
using OfficeOpenXml;

namespace NashTechListPersonMVC.WebApp.Areas.NashTech.Controllers
{
    [Area("NashTech")]
    public class RookiesController : Controller
    {
        private readonly IPersonBusinessLogic _personBusinessLogic;

        public RookiesController(IPersonBusinessLogic personBusinessLogic)
        {
            _personBusinessLogic = personBusinessLogic;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var allMembers = await _personBusinessLogic.GetAllMember();
                return View("Index", allMembers);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        public async Task<IActionResult> DisplayOldestMember()
        {
            try
            {
                var oldestMember = await _personBusinessLogic.GetOldestMember();
                return View("Index", oldestMember);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        public async Task<IActionResult> FilterMember(string filter)
        {
            try
            {
                var filterMember = await _personBusinessLogic.FilterPersonListByYear(filter);
                return View("Index", filterMember);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        public async Task<IActionResult> ExportExcel()
        {
            try
            {
                var excelData = await _personBusinessLogic.ExportExcelFile();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var fileName = "PersonList.xlsx";
                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        public IActionResult AddPerson(PersonCreateEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var person = new Person
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Gender = model.Gender,
                    DateOfBirth = model.DateOfBirth,
                    PhoneNumber = model.PhoneNumber,
                    BirthPlace = model.BirthPlace,
                    IsGraduated = model.IsGraduated
                };

                var result = _personBusinessLogic.Add(person);

                if (result)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = "Failed to add person. Please try again.";
                    return View("Error");
                }
            }
            else
            {
                return View("Add", model);
            }
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var person = await _personBusinessLogic.GetByIdAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            var editPersonViewModel = new PersonCreateEditViewModel
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Gender = person.Gender,
                DateOfBirth = person.DateOfBirth,
                PhoneNumber = person.PhoneNumber,
                BirthPlace = person.BirthPlace,
                IsGraduated = person.IsGraduated
            };

            return View("Update", editPersonViewModel);
        }

        // POST: Rookies/Update/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Guid id, PersonCreateEditViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingPerson = await _personBusinessLogic.GetByIdAsync(id);

                    if (existingPerson == null)
                    {
                        ViewBag.ErrorMessage = "Person not found.";
                        return View("Error");
                    }

                    existingPerson.FirstName = model.FirstName;
                    existingPerson.LastName = model.LastName;
                    existingPerson.Gender = model.Gender;
                    existingPerson.DateOfBirth = model.DateOfBirth;
                    existingPerson.PhoneNumber = model.PhoneNumber;
                    existingPerson.BirthPlace = model.BirthPlace;
                    existingPerson.IsGraduated = model.IsGraduated;

                    var result = _personBusinessLogic.Update(existingPerson);

                    if (result)
                    {
                        return RedirectToAction("Index", "Rookies");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Failed to update person. Please try again.";
                        return View("Error");
                    }
                }
                else
                {
                    return View("Update", model);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        public async Task<IActionResult> Detail(Guid Id)
        {
            var personDetails = await _personBusinessLogic.GetByIdAsync(Id);
            if (personDetails == null) return View("Error");
            return View(personDetails);
        }

        public async Task<IActionResult> Delete(Guid Id)
        {
            var personDetails = await _personBusinessLogic.GetByIdAsync(Id);
            if (personDetails == null) return View("Error");
            return View(personDetails);
        }

        public async Task<IActionResult> DeletePerson(Guid Id)
        {
            var personDetails = await _personBusinessLogic.GetByIdAsync(Id);
            if (personDetails == null) return View("Error");

            var deletedPersonName = personDetails.FullName;
            _personBusinessLogic.Delete(personDetails);

            return RedirectToAction("DeleteComfirm", new { deletedPersonName });
        }

        public IActionResult DeleteComfirm(string deletedPerson)
        {
            ViewBag.DeletedPersonName = deletedPerson;
            return View();
        }

    }
}
