using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Email;
using GIgApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Mails.Queries
{
    public class SendInvitationToFacility
    {
        public class Query : IQuery
        {
            public string Email { get; set; } = string.Empty;
        }
        public class Validator : AbstractValidator<Query> 
        {
            public Validator() 
            {
                RuleFor(x => x.Email).EmailAddress().WithMessage("Not valid email address");
            }
        }
        internal sealed class Handler(IMailSenderRepository _mailSenderRepository) : IQueryHandler<Query>
        {
            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _mailSenderRepository.SendMail(request.Email, "Invitation", "we invite you");
                return new BaseResult(new {},result,string.Empty);
            }
        }
    }
}
