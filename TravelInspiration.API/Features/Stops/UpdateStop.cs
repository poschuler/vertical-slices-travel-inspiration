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
    public class UpdateStop : ISlice
    {
        public void AddEnpoint(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapPut("api/itineraries/{itineraryId}/stops/{stopId}",
                (
                    int itineraryId,
                    int stopId,
                    UpdateStopCommand updateStopCommand,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    updateStopCommand.ItineraryId = itineraryId;
                    updateStopCommand.StopId = stopId;
                    return mediator.Send(
                        updateStopCommand,
                        cancellationToken
                    );


                }).RequireAuthorization(AuthorizationPolices.HasWriteActionPolicy);
        }

        public sealed class UpdateStopCommand : IRequest<IResult>
        {
            public int ItineraryId { get; set; }
            public int StopId { get; set; }
            public string Name { get; set; } = string.Empty;
            public string? ImageUri { get; set; }
            public bool? Suggested { get; set; }

        }

        public sealed class UpdateStopValidator : AbstractValidator<UpdateStopCommand>
        {
            public UpdateStopValidator()
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

        public sealed class UpdateStopCommandHandler(
            TravelInspirationDbContext dbContext,
            IMapper mapper) : IRequestHandler<UpdateStopCommand, IResult>

        {
            private readonly TravelInspirationDbContext _dbContext = dbContext;
            private readonly IMapper _mapper = mapper;

            public async Task<IResult> Handle(UpdateStopCommand request, CancellationToken cancellationToken)
            {
                var stopToUpdate = await _dbContext.Stops
                    .FirstOrDefaultAsync(s => s.Id == request.StopId 
                        && s.ItineraryId == request.ItineraryId, cancellationToken);

                if (stopToUpdate == null)
                {
                    return Results.NotFound();
                }

                stopToUpdate.HandleUpdateCommand(request);

                await _dbContext.SaveChangesAsync(cancellationToken);
                var mappedResult = _mapper.Map<StopDto>(stopToUpdate);
                return Results.Ok(mappedResult);
            }
        }

        public sealed class StopDto
        {
            public required int Id { get; set; }
            public required string Name { get; set; }
            public bool? Suggested { get; set; }
            public Uri? ImageUri { get; set; }
            public required int ItineraryId { get; set; }
        }

        public sealed class StopMapProfileAfterUpdate : Profile
        {
            public StopMapProfileAfterUpdate()
            {
                CreateMap<Stop, StopDto>();
            }
        }

        public sealed class SuggestStopUpdateEventHandler(
            ILogger<SuggestStopUpdateEventHandler> logger) : INotificationHandler<StopUpdatedEvent>
        {
            private readonly ILogger<SuggestStopUpdateEventHandler> _logger = logger;

            public Task Handle(StopUpdatedEvent notification, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Listener {Listener} to domain event {domainEvent} triggered.",
                    GetType().Name,
                    notification.GetType().Name);

                //do some AI magic to generate a new itinerary
                return Task.CompletedTask;
            }
        }


    }
}
