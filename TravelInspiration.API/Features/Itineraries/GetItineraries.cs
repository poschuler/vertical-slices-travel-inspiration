using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.Persistence;
using TravelInspiration.API.Shared.Slices;

namespace TravelInspiration.API.Features.Itineraries
{
    public sealed class GetItineraries : ISlice
    {
        //"feature": "get-itineraries" (or ["",""] for multiple)

        private AuthorizationPolicy _hasGetItinerariesFeaturePolicy = 
            new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireClaim("feature", "get-itineraries")
            .Build();

        public void AddEnpoint(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet(
                "api/itineraries",
                (
                    string? searchFor,
                    IMediator mediator,
                    CancellationToken cancellationToken
                    ) =>
                {
                    return mediator.Send(
                        new GetItinerariesQuery(searchFor),
                        cancellationToken
                        );

                }).RequireAuthorization(_hasGetItinerariesFeaturePolicy);
        }

        public sealed class GetItinerariesQuery(string? searchFor) : IRequest<IResult> 
        {
            public string? SearchFor { get; } = searchFor;


        }

        //public sealed class GetItinerariesResponse(IEnumerable<ItineraryDto> itineraries)
        //{

        //    public IEnumerable<ItineraryDto> Itineraries { get; } = itineraries;
        //}

        public sealed class GetItinerariesHandler(
            TravelInspirationDbContext dbContext,
            IMapper mapper) :
            IRequestHandler<GetItinerariesQuery, IResult>
        {

            private readonly IMapper _mapper = mapper;
            private readonly TravelInspirationDbContext _dbContext = dbContext;

            public async Task<IResult> Handle(GetItinerariesQuery request, CancellationToken cancellationToken)
            {
                var itineraries = await _dbContext.Itineraries.Where(i =>
                        request.SearchFor == null || i.Name.Contains(request.SearchFor) ||
                        (i.Description != null && i.Description.Contains(request.SearchFor)))
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                var mappedItineraries = _mapper.Map<IEnumerable<ItineraryDto>>(itineraries);

                //return new GetItinerariesResponse(mappedItineraries);
                return Results.Ok(mappedItineraries);
            }
        }

        public sealed class ItineraryDto
        {
            public required int Id { get; set; }
            public required string Name { get; set; }
            public string? Description { get; set; }
            public required string UserId { get; set; }
        }

        public sealed class ItineraryMapProfile : Profile
        {
            public ItineraryMapProfile()
            {
                CreateMap<Itinerary, ItineraryDto>();
            }
        }

    }

}
