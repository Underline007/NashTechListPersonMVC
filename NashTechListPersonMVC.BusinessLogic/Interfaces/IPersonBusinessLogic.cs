using NashTechListPersonMVC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NashTechListPersonMVC.BusinessLogic.Interfaces
{
    public interface IPersonBusinessLogic
    {
        public Task<IEnumerable<Person>> GetAllMember();
        public Task<IEnumerable<Person>> GetMaleMembers();
        public Task<IEnumerable<Person>> GetOldestMember();
        public Task<IEnumerable<Person>> GetPersonListContainFullName();
        public Task<IEnumerable<Person>> FilterPersonListByYear(int year);
        Task<byte[]> ExportExcelFile();

    }
}
