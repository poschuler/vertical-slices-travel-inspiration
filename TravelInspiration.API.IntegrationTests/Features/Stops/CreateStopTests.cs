using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelInspiration.API.Features.Stops;
using TravelInspiration.API.IntegrationTests.Fixtures;
using TravelInspiration.API.Shared.Domain.Entities;

namespace TravelInspiration.API.IntegrationTests.Features.Stops
{
    public sealed class CreateStopTests(SliceFixture sliceFixture) : IClassFixture<SliceFixture>
    {
        private readonly SliceFixture _sliceFixture = sliceFixture;

        [Fact]
        public async Task WhenExecutingCreateStopSlice_WhitValidInput_StopMustBeCreated()
        {

            await _sliceFixture.ExecuteInTransactionAsync(
                async (context) =>
            {

                //ARANGE
                var itinerary = new Itinerary("Test", "SomeUserId");
                context.Add(itinerary);
                await context.SaveChangesAsync();

                var cmd = new CreateStop.CreateStopCommand(
                    itinerary.Id,
                    "A stop for testing",
                    null
                    );



                //ACT
                await _sliceFixture.SendAsync(cmd);

                //ASSERT
                context.ChangeTracker.Clear();
                var stop = await context.Stops.FirstOrDefaultAsync(s => s.ItineraryId == itinerary.Id);
                Assert.NotNull(stop);
                Assert.Equal(cmd.ItineraryId, stop.ItineraryId);
                Assert.Equal(cmd.Name, stop.Name);
            });

        }

    }
}
