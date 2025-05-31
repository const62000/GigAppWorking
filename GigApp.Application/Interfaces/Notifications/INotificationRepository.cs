using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.Interfaces.Notifications
{
    public interface INotificationRepository
    {
        Task<bool> JobCreatedNotification(long jobId,string auth0Id);
        Task<bool> JobAppliedNotification(long jobId,long applicationId,string auth0Id);
        Task<bool> SendPaymentSuccessNotification(long userId);
        Task<bool> SendPaymentFailedNotification(long userId);
        Task<bool> ReceivePaymentSuccessNotification(long userId);
        Task<bool> JobApplicationViewedNotification(long applicationId);
        Task<bool> JobHiredNotification(long jobId,long freelancerId,long managerId);
        Task<BaseResult> GetNotifications(string auth0Id, int page,int pageSize);
    }
}
