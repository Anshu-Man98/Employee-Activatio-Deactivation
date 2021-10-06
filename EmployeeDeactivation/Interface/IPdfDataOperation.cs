using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDeactivation.Interface
{
    public interface IPdfDataOperation
    {
        byte[] FillDeactivationPdfForm(string gId);
        byte[] FillActivationPdfForm(string gId);
        byte[] HelpPdfDeactivation();
    }
}
