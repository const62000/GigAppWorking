using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Notifications;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Users.Queries
{
    public class GetNotification
    {
        public class Query : IQuery
        {
            public string Auth0Id { get; set; } = string.Empty;
            public int Page {  get; set; }
            public int PageSize { get; set; }
        }
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Auth0Id).NotEmpty();
                RuleFor(x=>x.Page).NotEqual(0);
                RuleFor(x=>x.PageSize).NotEqual(0);
            }
        }
        internal sealed class Handler(INotificationRepository _notificationRepository) : IQueryHandler<Query>
        {
            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _notificationRepository.GetNotifications(request.Auth0Id, request.Page, request.PageSize);
                return result;
            }
        }
    }
    
}
