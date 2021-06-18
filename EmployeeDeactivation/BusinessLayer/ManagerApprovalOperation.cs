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
    public class ManagerApprovalOperation: IManagerApprovalOperation
    {
        private readonly EmployeeDeactivationContext _context;
        public ManagerApprovalOperation(EmployeeDeactivationContext context)
        {
            _context = context;
        }

        public void PdfAttachment(string employeeName, string lastWorkingDatee, string gId, string teamName, string sponsorName, string memoryStream, string reportingManagerEmail)
        {
            byte[] bytes = System.Convert.FromBase64String(memoryStream);
            ManagerApprovalStatus ManagerApprovalStatus = new ManagerApprovalStatus()
            {
                EmployeeName = employeeName,
                EmployeeGId = gId,
                EmployeeLastWorkingDate = lastWorkingDatee,
                EmployeeTeamName = teamName,
                SponsorName = sponsorName,
                DeactivationWorkFlowPdfAttachment = bytes,
                ReportingManagerEmail= reportingManagerEmail,
                WorkFlowStatus = "pending"
            };
            _context.Add(ManagerApprovalStatus);
            _context.SaveChanges();

        }

        public List<ManagerApprovalStatus> RetrieveDeactivationDetailss()
        {
            List<ManagerApprovalStatus> deactivationDetails = new List<ManagerApprovalStatus>();
            var infoo = _context.ManagerApprovalStatus.ToList();
            foreach (var item in infoo)
            {
                    deactivationDetails.Add(new ManagerApprovalStatus
                {
                    EmployeeName = item.EmployeeName,
                    EmployeeLastWorkingDate = item.EmployeeLastWorkingDate,
                    EmployeeGId = item.EmployeeGId,
                    EmployeeTeamName = item.EmployeeTeamName,
                    SponsorName = item.SponsorName,
                    DeactivationWorkFlowPdfAttachment = item.DeactivationWorkFlowPdfAttachment,
                    ReportingManagerEmail = item.ReportingManagerEmail,
                    WorkFlowStatus = item.WorkFlowStatus
                });
            }
            return deactivationDetails;
        }

        public byte[] Getpdf(string GId)
        {
            var DDetails = RetrieveDeactivationDetailss();
            foreach (var item in DDetails)
            {
                if (item.EmployeeGId == GId)
                {
                    byte[] be = item.DeactivationWorkFlowPdfAttachment;
                    return be;
                }
            }
            byte[] bb = null;
            return bb;
        }

        public List<ManagerApprovalStatus> GetPendingDeactivationWorkflowForParticularManager(string userEmail)
        {
            
            List<ManagerApprovalStatus> pendingDeactivationWorkflows = new List<ManagerApprovalStatus>();
            var allDeactivationWorkfolw = (RetrieveDeactivationDetailss());
            foreach (var item in allDeactivationWorkfolw)
            {
                if (item.WorkFlowStatus.ToLower() == "pending" && item.ReportingManagerEmail == userEmail)
                {
                    pendingDeactivationWorkflows.Add(item);
                }

            }
            return pendingDeactivationWorkflows;
        }

        public List<ManagerApprovalStatus> GetAllPendingDeactivationWorkflows()
        {

            List<ManagerApprovalStatus> pendingDeactivationWorkflows = new List<ManagerApprovalStatus>();
            var allDeactivationWorkfolw = (RetrieveDeactivationDetailss());
            foreach (var item in allDeactivationWorkfolw)
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
            var allDeactivationWorkfolw = (RetrieveDeactivationDetailss());
            foreach (var item in allDeactivationWorkfolw)
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
            var allDeactivationWorkfolw = (RetrieveDeactivationDetailss());
            foreach (var item in allDeactivationWorkfolw)
            {
                if (item.WorkFlowStatus.ToLower() == "denied")
                {
                    declinedDeactivationWorkflows.Add(item);
                }

            }
            return declinedDeactivationWorkflows;
        }

        public bool ApproveRequest(string gId)
        {
            var check = _context.ManagerApprovalStatus.ToList();
            foreach (var i in check)
            {
                if (i.EmployeeGId == gId && i.WorkFlowStatus.ToLower() == "pending")
                {
                    i.WorkFlowStatus = "approve";
                    _context.SaveChanges();
                    return true;          
                }
            }
            return false;
        }

        public bool DeclineRequest(string gId)
        {
            var check = _context.ManagerApprovalStatus.ToList();
            foreach (var i in check)
            {
                if (i.EmployeeGId == gId && i.WorkFlowStatus.ToLower() == "pending")
                {
                    i.WorkFlowStatus = "denied";
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

    }
    
}
