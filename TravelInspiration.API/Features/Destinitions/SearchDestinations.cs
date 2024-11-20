using System.Threading;
using TravelInspiration.API.Shared.Networking;
using TravelInspiration.API.Shared.Slices;

namespace TravelInspiration.API.Features.Destinitions
{
    public sealed class SearchDestinations : ISlice
    {
        public void AddEnpoint(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("api/destinations", 
                async (string? searchFor,
                    ILoggerFactory logger,
                    CancellationToken cancellationToken,
                    IDestinationSearchApiClient destinationSearchApiClient) =>
            {
                logger.CreateLogger("EndpointHandler")
                    .LogInformation("SearchDestinations feature called.");

                var resultFromApiCall = await destinationSearchApiClient
                    .GetDestinationsAsync(searchFor, cancellationToken);

                var result = resultFromApiCall.Select(d => new
                {
                    d.Name,
                    d.Description,
                    d.ImageUri
                });

                return Results.Ok(result);
            });

        }
        
    }
}
