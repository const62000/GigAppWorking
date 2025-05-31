using GIgApp.Contracts.Responses;
using MediatR;


namespace GigApp.Application.CQRS.Abstractions.Command
{
    internal interface ICommand : IRequest<BaseResult>
    {
    }
}
