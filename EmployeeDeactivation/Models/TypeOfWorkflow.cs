using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDeactivation.Models
{
    public enum TypeOfWorkflow
    {
        Deactivation=1, Activation=2, ReminderEmail=3, DeclinedEmail=4
    }
}
