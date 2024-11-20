﻿using Azure.Core;
using FluentValidation;
using MediatR;

namespace TravelInspiration.API.Shared.Behaviours
{
    public class ModelValidationBehaviour<TRequest, IResult>
        (IEnumerable<IValidator<TRequest>> validators) :
        IPipelineBehavior<TRequest, IResult>
        where TRequest : IRequest<IResult>
    {
        public IEnumerable<IValidator<TRequest>> _validators { get; } = validators;

        public async Task<IResult> Handle(TRequest request, RequestHandlerDelegate<IResult> next, CancellationToken cancellationToken)
        {
            if(!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var validationResults = _validators
                .Select(v => v.Validate(context))
                .ToList();

            var groupedValidationFailures = validationResults
                .SelectMany(r => r.Errors)
                .GroupBy(f => f.PropertyName)
                .Select(g => new
                {
                    PropertyName = g.Key,
                    VaidationFailures = g.Select(v => new { v.ErrorMessage } )
                })
                .ToList();

            if (groupedValidationFailures.Count != 0)
            {
                var validationProblemDictionary = new Dictionary<string, string[]>();

                foreach (var group in groupedValidationFailures)
                {
                    var errorMessage = group.VaidationFailures.Select(f => f.ErrorMessage);
                    validationProblemDictionary.Add(group.PropertyName, errorMessage.ToArray());

                }
                return (IResult)Results.ValidationProblem(validationProblemDictionary);
            }

            return await next();

        }
    }
}
