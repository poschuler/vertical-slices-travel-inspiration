﻿using MediatR;

namespace TravelInspiration.API.Shared.Domain
{
    public abstract class DomainEvent : INotification
    {
        public bool IsPublished { get; set; }
        public DateTimeOffset OccuredOn { get; } = DateTime.UtcNow;
    }
}
