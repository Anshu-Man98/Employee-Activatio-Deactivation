using EmployeeDeactivation.Data;
using EmployeeDeactivation.Interface;
using EmployeeDeactivation.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmployeeDeactivation.BusinessLayer
{
    public class ManagerApprovalOperation : IManagerApprovalOperation
    {
        private readonly EmployeeDeactivationContext _context;

        public ManagerApprovalOperation(EmployeeDeactivationContext context )
        {
            _context = context;
            
        }

        public void AddPendingDeactivationRequestToDatabase(ManagerApprovalStatus managerApprovalStatus)
        {
            var count = 0;
            ManagerApprovalStatus ManagerApprovalStatus = new ManagerApprovalStatus()
            {
                EmployeeName = managerApprovalStatus.EmployeeName,
                EmployeeGId = managerApprovalStatus.EmployeeGId,
                EmployeeLastWorkingDate = managerApprovalStatus.EmployeeLastWorkingDate,
                EmployeeTeamName = managerApprovalStatus.EmployeeTeamName,
                SponsorFirstName = managerApprovalStatus.SponsorFirstName,
                SponsorLastName = managerApprovalStatus.SponsorLastName,
                DeactivationWorkFlowPdfAttachment = managerApprovalStatus.DeactivationWorkFlowPdfAttachment,
                ReportingManagerEmail = managerApprovalStatus.ReportingManagerEmail,
                WorkFlowStatus = managerApprovalStatus.WorkFlowStatus
            };
            var pendingDeactivationWorkflow = RetrieveDeactivationDetails();
            foreach (var item in pendingDeactivationWorkflow)
            {
                if (item.EmployeeGId == managerApprovalStatus.EmployeeGId)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                _context.Add(ManagerApprovalStatus);
                _context.SaveChanges();
            }
        }

        public bool AddDeactivationTaskToDatabase(DeactivationStatus deactivationStatus)
        {
            DeactivationStatus DeactivationStatus = new DeactivationStatus()
            {
                EmployeeId = deactivationStatus.EmployeeId,
                EmployeeName = deactivationStatus.EmployeeName,
                TimesheetApproval = deactivationStatus.TimesheetApproval,
                EmployeeRemovedFromDLEmailList = deactivationStatus.EmployeeRemovedFromDLEmailList,
                HardwaresCollected = deactivationStatus.HardwaresCollected,
                RaisedWindowsDeactivationRequestNexus = deactivationStatus.RaisedWindowsDeactivationRequestNexus,
                TimerDate = deactivationStatus.TimerDate,
                LastWorkingDate = deactivationStatus.LastWorkingDate,
                ReportingManagerEmail = deactivationStatus.ReportingManagerEmail,
                EmployeeEmail=deactivationStatus.EmployeeEmail,
                FromEmail = deactivationStatus.FromEmail
            };
            if (deactivationStatus.TimesheetApproval.Trim() == "true" && deactivationStatus.EmployeeRemovedFromDLEmailList.Trim() == "true" && deactivationStatus.HardwaresCollected.Trim() == "true" && deactivationStatus.RaisedWindowsDeactivationRequestNexus.Trim() == "true")
            {
                _context.Remove(_context.DeactivationStatus.Single(a => a.EmployeeId == deactivationStatus.EmployeeId));
                _context.SaveChanges();
                return true;
            }
            var allDeactivationTask = RetrieveDeactivationTasks();
            foreach (var item in allDeactivationTask)
            {
                if (item.EmployeeId == deactivationStatus.EmployeeId)
                {
                    _context.Remove(_context.DeactivationStatus.Single(a => a.EmployeeId == deactivationStatus.EmployeeId));
                    _context.SaveChanges();
                }
            }
            _context.Add(DeactivationStatus);
            _context.SaveChanges();
            return true;
        }

        public bool AddActivationTaskToDatabase(ActivationStatus activationStatus)
        {
            ActivationStatus ActivationStatus = new ActivationStatus()
            {
                EmployeeId = activationStatus.EmployeeId,
                EmployeeName = activationStatus.EmployeeName,
                UpdateConsolidateIoTResourcesList = activationStatus.UpdateConsolidateIoTResourcesList,
                BirthdayListUpdated = activationStatus.BirthdayListUpdated,
                EmailIdAddedToTeamDL = activationStatus.EmailIdAddedToTeamDL,
                MemberAddedToTimesheet = activationStatus.MemberAddedToTimesheet,
                InitiateHardwareShipmentRequest = activationStatus.InitiateHardwareShipmentRequest,
                MemberDetailsUpdatedInEDMTTool = activationStatus.MemberDetailsUpdatedInEDMTTool,
                InductionPlanAssignment = activationStatus.InductionPlanAssignment,
                InductionTrainingRecordUpdated = activationStatus.InductionTrainingRecordUpdated,
                VivekEcampusListUpdated = activationStatus.VivekEcampusListUpdated,
                ReportingManagerEmail = activationStatus.ReportingManagerEmail,
                TimerDate = activationStatus.ActivationDate,
                ActivationDate = activationStatus.ActivationDate,
                EmployeeEmail= activationStatus.EmployeeEmail,
                FromEmail = activationStatus.FromEmail
            };
            if (activationStatus.UpdateConsolidateIoTResourcesList.Trim() == "true" && activationStatus.BirthdayListUpdated.Trim() == "true" && activationStatus.EmailIdAddedToTeamDL.Trim() == "true" && activationStatus.MemberAddedToTimesheet.Trim() == "true" && activationStatus.InitiateHardwareShipmentRequest.Trim() == "true" && activationStatus.MemberDetailsUpdatedInEDMTTool.Trim() == "true" && activationStatus.InductionPlanAssignment.Trim() == "true" && activationStatus.InductionTrainingRecordUpdated.Trim() == "true" && activationStatus.VivekEcampusListUpdated.Trim() == "true")
            {
                _context.Remove(_context.ActivationStatus.Single(a => a.EmployeeId == activationStatus.EmployeeId));
                _context.SaveChanges();
                return true;
            }
            var allActivationTask = RetrieveActivationTasks();
            foreach (var item in allActivationTask)
            {
                if (item.EmployeeId == activationStatus.EmployeeId)
                {
                    _context.Remove(_context.ActivationStatus.Single(a => a.EmployeeId == activationStatus.EmployeeId));
                    _context.SaveChanges();
                }
            }
            _context.Add(ActivationStatus);
            _context.SaveChanges();
            return true;
        }

        public List<ManagerApprovalStatus> GetPendingDeactivationWorkflowForParticularManager(string userEmail)
        {
            List<ManagerApprovalStatus> pendingDeactivationWorkflows = new List<ManagerApprovalStatus>();
            var allDeactivationWorkflow = RetrieveDeactivationDetails();
            foreach (var item in allDeactivationWorkflow)
            {
                if (item.WorkFlowStatus.ToLower() == "pending" && item.ReportingManagerEmail == userEmail)
                {
                    pendingDeactivationWorkflows.Add(item);
                }
            }
            return pendingDeactivationWorkflows;
        }

        public List<ActivationStatus> GetActivationTasksForParticularManager(string userEmail)
        {
            List<ActivationStatus> activationTasks = new List<ActivationStatus>();
            var allActivationTask = RetrieveActivationTasks();
            foreach (var item in allActivationTask)
            {
                if (item.ReportingManagerEmail == userEmail)
                {
                    activationTasks.Add(item);
                }
            }
            return activationTasks;
        }

        public List<DeactivationStatus> GetDeactivationTasksForParticularManager(string userEmail)
        {
            List<DeactivationStatus> deactivationTasks = new List<DeactivationStatus>();
            var allDeactivationTask = RetrieveDeactivationTasks();
            foreach (var item in allDeactivationTask)
            {
                if (item.ReportingManagerEmail == userEmail)
                {
                    deactivationTasks.Add(item);
                }
            }
            return deactivationTasks;
        }

        public byte[] DownloadDeactivationPdfFromDatabase(string gId)
        {
            byte[] pdf = null;
            var allDeactivationWorkflow = RetrieveDeactivationDetails();
            foreach (var item in allDeactivationWorkflow)
            {
                if (item.EmployeeGId.ToLower() == gId.ToLower())
                {
                    return item.DeactivationWorkFlowPdfAttachment;
                }
            }
            return pdf;
        }
        public bool ApproveRequest(string gId)
        {
            try
            {
                var allDeactivationWorkflow = RetrieveDeactivationDetails();
                foreach (var i in allDeactivationWorkflow)
                {
                    if (i.EmployeeGId == gId && i.WorkFlowStatus.ToLower() == "pending")
                    {
                        i.WorkFlowStatus = "approve";
                        return _context.SaveChanges() == 1;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public bool DeclineRequest(string gId)
        {
            try
            {
                var allDeactivationWorkflow = RetrieveDeactivationDetails();
                foreach (var i in allDeactivationWorkflow)
                {
                    if (i.EmployeeGId.ToLower() == gId.ToLower() && i.WorkFlowStatus.ToLower() == "pending")
                    {
                        i.WorkFlowStatus = "denied";
                        return _context.SaveChanges() == 1;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public List<ManagerApprovalStatus> GetAllPendingDeactivationWorkflows()
        {
            List<ManagerApprovalStatus> pendingDeactivationWorkflows = new List<ManagerApprovalStatus>();
            var allDeactivationWorkflow = RetrieveDeactivationDetails();
            foreach (var item in allDeactivationWorkflow)
            {
                if (item.WorkFlowStatus.ToLower() == "pending")
                {
                    pendingDeactivationWorkflows.Add(item);
                }

            }
            return pendingDeactivationWorkflows;
        }

        public List<ManagerApprovalStatus> GetAllApprovedDeactivationWorkflows()
        {
            List<ManagerApprovalStatus> approvedDeactivationWorkflows = new List<ManagerApprovalStatus>();
            var allDeactivationWorkflow = RetrieveDeactivationDetails();
            foreach (var item in allDeactivationWorkflow)
            {
                if (item.WorkFlowStatus.ToLower() == "approve")
                {
                    approvedDeactivationWorkflows.Add(item);
                }
            }
            return approvedDeactivationWorkflows;
        }

        public string GetApprovedDeactivationWorkReportingManagerEmailflowsBasedOnGid(string gid)
        {
            var allDeactivationWorkflow = GetAllApprovedDeactivationWorkflows();
            foreach (var item in allDeactivationWorkflow)
            {
                if (gid.ToLower() == item.EmployeeGId.ToLower())
                {
                    return item.ReportingManagerEmail;
                }
            }
            return null;

        }

        public List<ManagerApprovalStatus> GetAllDeclinedDeactivationWorkflows()
        {
            List<ManagerApprovalStatus> declinedDeactivationWorkflows = new List<ManagerApprovalStatus>();
            var allDeactivationWorkflow = RetrieveDeactivationDetails();
            foreach (var item in allDeactivationWorkflow)
            {
                if (item.WorkFlowStatus.ToLower() == "denied")
                {
                    declinedDeactivationWorkflows.Add(item);
                }

            }
            return declinedDeactivationWorkflows;
        }
        private List<ManagerApprovalStatus> RetrieveDeactivationDetails()
        {
            _ = new List<ManagerApprovalStatus>();
            var allDeactivatedRequestsStatus = _context.ManagerApprovalStatus.ToList();
            return allDeactivatedRequestsStatus;
        }

        public List<DeactivationStatus> RetrieveDeactivationTasks()
        {
            _ = new List<DeactivationStatus>();
            return _context.DeactivationStatus.ToList();
        }

        private string TimerValue()
        {
            var config = _context.Tokens.ToList();
            foreach (var token in config)
            {
                if (token.TokenName == "EmailTimer")
                {
                    return token.TokenValue;
                }
            }
            return null;
        }

        public List<DeactivationStatus> RetrieveDeactivationTasksBasedOnDate()
        {
            try
            {
                int Timer = Int16.Parse(TimerValue());
                var DeactivationTasksBasedOnDate = _context.DeactivationStatus.ToList();
                List<DeactivationStatus> DeactivationTaskDetails = new List<DeactivationStatus>();

                foreach (var Details in DeactivationTasksBasedOnDate)
                {
                    if (DateTime.Today.ToString() == Convert.ToDateTime(Details.TimerDate).AddDays(Timer).ToString() && DateTime.Today < Convert.ToDateTime(Details.LastWorkingDate))
                    {
                        DateTime TimerDate = Convert.ToDateTime(Details.TimerDate).AddDays(Timer);
                        Details.TimerDate = TimerDate;

                        DeactivationTaskDetails.Add(Details);
                    }
                }
                _context.SaveChanges();
                
                return DeactivationTaskDetails;
            }
            catch
            {
                return null;
            }

        }

        public List<ActivationStatus> RetrieveActivationTasksBasedOnDate()
        {
            try
            {
                int Timer = Int16.Parse(TimerValue());
                var ActivationTasksBasedOnDate = _context.ActivationStatus.ToList();
                List<ActivationStatus> ActivationTaskDetails = new List<ActivationStatus>();

                foreach (var Details in ActivationTasksBasedOnDate)
                {
                    if (DateTime.Today.ToString() == Convert.ToDateTime(Details.TimerDate).AddDays(Timer).ToString() && DateTime.Today < Convert.ToDateTime(Details.ActivationDate).AddDays(16))
                    {
                        DateTime TimerDate = Convert.ToDateTime(Details.TimerDate).AddDays(Timer);
                        Details.TimerDate = TimerDate;
                        ActivationTaskDetails.Add(Details);
                    }
                }
                _context.SaveChanges();

                return ActivationTaskDetails;
            }
            catch
            {
                return null;
            }

        }



        public List<ActivationStatus> RetrieveActivationTasks()
        {
            _ = new List<ActivationStatus>();
            return _context.ActivationStatus.ToList();

        }

        public List<ManagerApprovalStatus> RetrieveDeactivatedEmployeeDataBasedLastWorkingDate(DateTime LastworkingDate)
        {
            List<ManagerApprovalStatus> TodaysApprovedEmployeeData = new List<ManagerApprovalStatus>();
            var employeeDetails = GetAllApprovedDeactivationWorkflows();
            foreach (var employee in employeeDetails)
            {
                
                if (DateTime.Parse(employee.EmployeeLastWorkingDate) == LastworkingDate)
                {
                    
                    TodaysApprovedEmployeeData.Add(employee);
                }
              
            }
            
            return TodaysApprovedEmployeeData;
        }

    }

}
