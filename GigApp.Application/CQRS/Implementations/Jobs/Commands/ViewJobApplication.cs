using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.Interfaces.Jobs;
using GigApp.Application.Interfaces.Notifications;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Jobs.Commands
{
    public class ViewJobApplication
    {
        public class Command : ICommand
        {
            public long ApplicationId { get; set; }
        }
        internal sealed class Handler(IJobApplicationRepository _jobApplicationRepository,INotificationRepository _notificationRepository) : ICommandHandler<Command>
        {
            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await _jobApplicationRepository.ChangeJobApplicationStatus(request.ApplicationId, "Viewed");
                if(!result.Status)
                    return result;
                await _notificationRepository.JobApplicationViewedNotification(request.ApplicationId);
                return result;
            }
        }
    }
}
