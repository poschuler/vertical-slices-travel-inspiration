using MediatR;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.Domain.Events;
using TravelInspiration.API.Shared.Persistence;

namespace TravelInspiration.API.Shared.DomainEventHandlers
{
    public sealed class SuggestStopCreatedEventHandler(
            ILogger<SuggestStopCreatedEventHandler> logger,
            TravelInspirationDbContext dbContext
            )
            : INotificationHandler<StopCreatedEvent>
    {
        public ILogger<SuggestStopCreatedEventHandler> _logger { get; } = logger;
        public TravelInspirationDbContext _dbContext { get; } = dbContext;

        public Task Handle(
            StopCreatedEvent notification,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Listener {Listener} to domain event {domainEvent} triggered.",
                GetType().Name,
                notification.GetType().Name);
            var incomingStop = notification.Stop;

            //do some AI magic to generate a new stop...
            var stop = new Stop($"AI-ified stop based on {incomingStop.Name}")
            {
                ItineraryId = incomingStop.ItineraryId,
                ImageUri = new Uri("https://via.placeholder.com/150"),
                Suggested = true
            };

            _dbContext.Stops.Add(stop);
            return Task.CompletedTask;
        }
    }
}
