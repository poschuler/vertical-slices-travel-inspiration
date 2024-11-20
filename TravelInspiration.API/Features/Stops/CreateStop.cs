using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.Domain.Events;
using TravelInspiration.API.Shared.Persistence;
using TravelInspiration.API.Shared.Security;
using TravelInspiration.API.Shared.Slices;

namespace TravelInspiration.API.Features.Stops
{
    public sealed class CreateStop : ISlice
    {
        //"scope": "write" (or ["read","write"] ...)

        public void AddEnpoint(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapPost(
                "api/itineraries/{itineraryId}/stops",
                (int itineraryId,
                CreateStopCommand createStopCommand,
                IMediator mediator,
                CancellationToken cancellationToken
                ) =>
                {
                    createStopCommand.ItineraryId = itineraryId;
                    return mediator.Send(
                        createStopCommand,
                        cancellationToken
                        );

                }).RequireAuthorization(AuthorizationPolices.HasWriteActionPolicy);
        }

        public sealed class CreateStopCommand(
            int itineraryId,
            string name,
            string? imageUri
            ) : IRequest<IResult>
        {
            public int ItineraryId { get; set; } = itineraryId;
            public string Name { get; } = name;
            public string? ImageUri { get; } = imageUri;

            //on creation via this command, you cannot pass
            //through a "suggested" field value
        }

        public sealed class CreateStopValidator : AbstractValidator<CreateStopCommand>
        {
            public CreateStopValidator()
            {
                RuleFor(c => c.Name)
                    .MaximumLength(200)
                    .NotEmpty();

                RuleFor(c => c.ImageUri)
                    .Must(ImageUri => Uri.TryCreate(ImageUri ?? "", UriKind.Absolute, out _))
                    .When(c => !string.IsNullOrEmpty(c.ImageUri))
                    .WithMessage("ImageUri must be a valid URI");
            }
        }

        public sealed class CreateStopCommandHandler(
            TravelInspirationDbContext dbContext,
            IMapper mapper
            ) : IRequestHandler<CreateStopCommand, IResult>
        {
            public TravelInspirationDbContext _dbContext { get; } = dbContext;
            public IMapper _mapper { get; } = mapper;

            public async Task<IResult> Handle(CreateStopCommand request, CancellationToken cancellationToken)
            {
                //check if itinerary exists
                if (!await _dbContext.Itineraries
                    .AnyAsync(i => i.Id == request.ItineraryId, cancellationToken))
                {
                    return Results.NotFound();
                }

                //create the entity
                var stopEntity = new Stop(request.Name);
                stopEntity.HandleCreateCommand(request);

                _dbContext.Stops.Add(stopEntity);
                await _dbContext.SaveChangesAsync(cancellationToken);

                //map to DTO
                var stopDto = _mapper.Map<StopDto>(stopEntity);

                return Results.Created(
                    $"api/itineraries/{stopEntity.ItineraryId}/stops/{stopEntity.Id}",
                    stopDto);
            }
        }

        public sealed class StopDto
        {
            public required int Id { get; set; }
            public required string Name { get; set; }
            public Uri? ImageUri { get; set; }
            public required int ItineraryId { get; set; }
        }

        public sealed class StopMapProfile : Profile
        {
            public StopMapProfile()
            {
                CreateMap<Stop, StopDto>();
            }
        }

        

        

    }
}
