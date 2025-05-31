using GIgApp.Contracts.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Abstractions.Command
{
    internal interface ICommandHandler<TCommand> : IRequestHandler<TCommand, BaseResult> where TCommand : ICommand
    {
    }
}
