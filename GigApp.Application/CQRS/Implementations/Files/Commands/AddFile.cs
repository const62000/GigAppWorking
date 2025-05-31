using FluentValidation;
using GigApp.Application.CQRS.Abstractions.Command;
using GigApp.Application.Interfaces.FileSaver;
using GIgApp.Contracts.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApp.Application.CQRS.Implementations.Files.Commands
{
    public class AddFile
    {
        public class Command : ICommand
        {
            public string FileName { get; set; } = string.Empty;
            public string Text { get; set; } = string.Empty;
            public IFormFile? File { get; set; }
            public string BaseUrl { get; set; } = string.Empty;
        }
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Text).NotEmpty();
                RuleFor(x => x.File).NotNull();
            }
        }
        internal sealed class Handler(IFilesSaverRepository _filesSaverRepository,IValidator<Command> _validator) : ICommandHandler<Command>
        {
            public async Task<BaseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return new BaseResult(new { }, false, validationResult.Errors.First().ErrorMessage);
                }
                var path = await _filesSaverRepository.SaveImage(request.FileName, request.Text, request.File!, request.BaseUrl);
                return new BaseResult(new
                {
                    path = path
                }, true, string.Empty);
            }
        }
    }
}
