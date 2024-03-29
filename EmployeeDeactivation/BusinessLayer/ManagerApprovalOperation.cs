﻿using EmployeeDeactivation.Data;
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
    public class ManagerApprovalOperation: IManagerApprovalOperation
    {
        private readonly EmployeeDeactivationContext _context;
        public ManagerApprovalOperation(EmployeeDeactivationContext context)
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
                ReportingManagerEmail= managerApprovalStatus.ReportingManagerEmail,
                WorkFlowStatus = managerApprovalStatus.WorkFlowStatus
            };
            var pendingDeactivationWorkflow = RetrieveDeactivationDetails();
            foreach (var item in pendingDeactivationWorkflow)
            {
                if(item.EmployeeGId==managerApprovalStatus.EmployeeGId)
                {
                    count++;
                }
            }
            if(count==0)
            {
                _context.Add(ManagerApprovalStatus);
                _context.SaveChanges();
            }
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

    }
    
}
