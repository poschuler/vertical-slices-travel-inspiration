﻿using Azure.Core;
using MediatR;
using System.Diagnostics;
using TravelInspiration.API.Shared.Metrics;

namespace TravelInspiration.API.Shared.Behaviours
{
    public class HandlerPerformanceMetricBehaviour<TRequest, TResponse>
        (HandlerPerformanceMetric handlerPerformanceMetric)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public HandlerPerformanceMetric _handlerPerformanceMetric { get; } = handlerPerformanceMetric;
        private readonly Stopwatch _timer = new();

        public async Task<TResponse> Handle(
            TRequest request, 
            RequestHandlerDelegate<TResponse> next, 
            CancellationToken cancellationToken)
        {
            _timer.Start();
            var response = await next();
            _timer.Stop();

            _handlerPerformanceMetric.MiliSecondsElapsed(_timer.ElapsedMilliseconds);
            return response;
        }
    }
}
