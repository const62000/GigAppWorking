
using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.Interfaces.Vendors;
using GigApp.Domain.Entities;
using GIgApp.Contracts.Responses;
using GigApp.Application.Interfaces.Auth;
using Mapster;

namespace GigApp.Application.CQRS.Implementations.Vendors.Commands
{
    public class EditVendor
    {
        public class Command : ICommand
        {
            public int VendorId { get; set; }
            public string? Name { get; set; }
            public string? ServicesOffered { get; set; }
            public string? Certifications { get; set; }
            public string? Status { get; set; }
        }

        public class Handler(IVendorRepository vendorRepository) : ICommandHandler<Command>
        {
            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var vendor = request.Adapt<Vendor>();
                var result = await vendorRepository.UpdateVendor(vendor);
                return result;
            }
        }
    }
}