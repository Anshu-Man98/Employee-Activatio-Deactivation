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
        string GetApprovedDeactivationWorkReportingManagerEmailflowsBasedOnGid(string gid);
        List<DeactivationStatus> RetrieveDeactivationTasks();
        List<ActivationStatus> RetrieveActivationTasks();
        bool ApproveRequest(string gId);
        bool DeclineRequest(string gId);
        bool AddDeactivationTaskToDatabase(DeactivationStatus deactivationStatus);
        bool AddActivationTaskToDatabase(ActivationStatus activationStatus);
        List<ActivationStatus> GetActivationTasksForParticularManager(string userEmail);
        List<DeactivationStatus> GetDeactivationTasksForParticularManager(string userEmail);
        List<ManagerApprovalStatus> RetrieveDeactivatedEmployeeDataBasedLastWorkingDate(DateTime LastworkingDate);
        List<DeactivationStatus> RetrieveDeactivationTasksBasedOnDate();
        List<ActivationStatus> RetrieveActivationTasksBasedOnDate();


    }
}
