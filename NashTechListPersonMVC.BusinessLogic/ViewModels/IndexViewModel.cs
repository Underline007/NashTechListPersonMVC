using NashTechListPersonMVC.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NashTechListPersonMVC.BusinessLogic.ViewModels
{
    public class PaginationViewModel
    {
        public List<Person> Members { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }

    }
  
}
