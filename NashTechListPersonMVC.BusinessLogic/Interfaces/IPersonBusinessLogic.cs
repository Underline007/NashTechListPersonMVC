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
        Task<IEnumerable<Person>> GetAllMember();
        public Task<IEnumerable<Person>> GetMaleMembers();
        public Task<IEnumerable<Person>> GetOldestMember();
        public Task<IEnumerable<Person>> FilterPersonListByYear(string filler);
        Task<byte[]> ExportExcelFile();
        Task<Person> GetByIdAsync(Guid id);
        bool Add(Person person);
        bool Update(Person person);
        bool Delete(Person person);
        
    }
}
