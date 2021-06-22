using EmployeeDeactivation.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeDeactivation.Interface
{
   public interface IAdminDataOperation
    {
        List<Teams> RetrieveSponsorDetails();
        Task<bool> AddSponsorData(Teams team);
        Task<bool> DeleteSponsorData(string gId);
        List<EmployeeDetails> DeactivationEmployeeData();
        List<EmployeeDetails> ActivationEmployeeData();
    }
}
