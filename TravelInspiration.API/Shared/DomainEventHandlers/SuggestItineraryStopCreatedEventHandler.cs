using MediatR;
using TravelInspiration.API.Shared.Domain.Events;

namespace TravelInspiration.API.Shared.DomainEventHandlers
{
    public sealed class SuggestItineraryStopCreatedEventHandler(
            ILogger<SuggestStopCreatedEventHandler> logger
            ) : INotificationHandler<StopCreatedEvent>
    {
        public ILogger<SuggestStopCreatedEventHandler> _logger { get; } = logger;
        public Task Handle(StopCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Listener {Listener} to domain event {domainEvent} triggered.",
                GetType().Name,
                notification.GetType().Name);

            //do some AI magic to generate a new itinerary
            return Task.CompletedTask;
        }
    }
}
