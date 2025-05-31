using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Query;
using GigApp.Application.Interfaces.Vendors;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Vendors.Queries
{
    public class DeleteVendors
    {
        public class Query : IQuery
        {
            public List<int> vendorIds { get; set; } = new List<int>();
        }
        private bool HaveMoreThanOneItem(List<string> items)
        {
            // Check if the list is not null and has more than one item
            return items != null && items.Count > 1;
        }
        public class Validator : AbstractValidator<Query>
        {
            public Validator() 
            {
                RuleFor(x => x.vendorIds).NotNull().WithMessage(Messages.List_Empty).Must(x=>x.Count>0).WithMessage(Messages.List_Empty);
            }
        }
        internal sealed class Handler(IVendorRepository _vendorRepository,IValidator<Query> _validator) : IQueryHandler<Query>
        {
            public async Task<BaseResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
                }
                var result = await _vendorRepository.DeleteVendors(request.vendorIds);
                return result;
            }
        }
    }
}
