using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDeactivation.Models
{
    public class PdfAttachment
    {
        public byte[] ActivationWorkflowPdfAttachment { get; set; }
        public byte[] DecativationWorkFlowPdfAttachment { get; set; }
    }
}
