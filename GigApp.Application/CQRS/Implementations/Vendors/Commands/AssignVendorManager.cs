using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.Interfaces.Vendors;
using GIgApp.Contracts.Responses;
using Mapster;


namespace GigApp.Application.CQRS.Implementations.Vendors.Commands;

public class AssignVendorManager
{
    public class Command : ICommand
    {
        public int VendorId { get; set; }
        public string Email { get; set; }
    }

    public class Handler(IVendorRepository _vendorRepository) : ICommandHandler<Command>
    {
        public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var result = await _vendorRepository.AddVendorManager(request.VendorId, request.Email);
            return result;
        }
    }
}