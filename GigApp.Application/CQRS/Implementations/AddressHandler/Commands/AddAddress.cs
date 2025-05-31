using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.Interfaces.Addresses;
using GigApp.Domain.Entities;
using GIgApp.Contracts.Responses;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.AddressHandler.Commands
{
    public class AddAddress
    {
        public class Command : ICommand 
        {
            public string Auth0Id { get; set; } = string.Empty;
            public string StreetAddress1 {  get; set; } = string.Empty;
            public string StreetAddress2 { get; set; } = string.Empty;
            public string City { get; set; } = string.Empty ;
            public string Country { get; set; } = string.Empty;
            public decimal Latitude { get; set; }
            public decimal Longitude {  get; set; }
        }
        internal sealed class Handler(IAddressesRepository _addressesRepository) : ICommandHandler<Command>
        {
            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var address = request.Adapt<Address>();
                var result = await _addressesRepository.AddAddressAsync(request.Auth0Id,address);
                return result;
            }
        }
    }
}
