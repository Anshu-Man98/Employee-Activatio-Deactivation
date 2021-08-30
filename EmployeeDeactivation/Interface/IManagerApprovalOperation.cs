using EmployeeDeactivation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDeactivation.Interface
{
    public interface IManagerApprovalOperation
    {
        void AddPendingDeactivationRequestToDatabase(ManagerApprovalStatus managerApprovalStatus);
        byte[] DownloadDeactivationPdfFromDatabase(string gId);
        List<ManagerApprovalStatus> GetPendingDeactivationWorkflowForParticularManager(string userEmail);
        List<ManagerApprovalStatus> GetAllPendingDeactivationWorkflows();
        List<ManagerApprovalStatus> GetAllApprovedDeactivationWorkflows();
        List<DeactivationStatus> RetrieveDeactivationTasks();
        bool ApproveRequest(string gId);
        bool DeclineRequest(string gId);
        bool AddDeactivationTaskToDatabase(DeactivationStatus deactivationStatus);
    }
}
