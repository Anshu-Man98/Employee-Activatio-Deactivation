using System;
namespace EmployeeDeactivation.Models
{
    public enum TypeOfWorkflow
    {
        DeactivationWorkFlowInitiated=1,
        DeactivationWorkFlowLastWorkingDay = 2,
        DeactivationWorkFlowReminderManagerOnLastWorkingDay=3,
        DeactivationWorkFlowReminderManagerTwoDaysBeforeLastWorkingDay = 4,
        DeactivationWorkFlowReminderEmployee =5,
        Activation = 6, 
        DeclinedEmail = 7,
        ActivationWorkFlowRemainderToManager = 8,
        ActivationWorkFlowRemainderToEmployee = 9

    }
}
