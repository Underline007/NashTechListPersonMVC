using Microsoft.AspNetCore.Mvc;
using NashTechListPersonMVC.BusinessLogic.Interfaces;
using System;

namespace NashTechListPersonMVC.WebApp.Controllers
{
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

        public async Task<IActionResult> FilterMember(int year)
        {
            try
            {
                var filterMember = await _personBusinessLogic.FilterPersonListByYear(year);
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
                var fileName = "PersonList.xlsx";
                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }
    }
}
