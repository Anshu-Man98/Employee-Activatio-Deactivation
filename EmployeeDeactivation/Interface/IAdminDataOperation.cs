using EmployeeDeactivation.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeDeactivation.Interface
{
   public interface IAdminDataOperation
    {
        List<Teams> RetrieveSponsorDetails();
        bool AddSponsorData(Teams team);
        bool DeleteSponsorData(string gId);
        List<EmployeeDetails> DeactivationEmployeeData();
        List<EmployeeDetails> ActivationEmployeeData();
        Teams RetrieveSponsorDetailsAccordingToGid(string gId);
    }
}
