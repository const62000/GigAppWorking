using GIgApp.Contracts.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Abstractions.Query
{
    internal interface IQueryHandler<TQuery> : IRequestHandler<TQuery,BaseResult> where TQuery : IQuery
    {
    }
}
