using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelInspiration.API.Features.Stops;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.Domain.Events;

namespace TravelInspiration.API.UnitTests.Shared.Domain
{
    public class StopTests : IDisposable
    {
        private readonly Stop _stopEntity;
        private readonly CreateStop.CreateStopCommand _createStopCommand;

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public StopTests()
        {
            _stopEntity = new Stop("StopForTesting");
            _createStopCommand = new CreateStop.CreateStopCommand
                (42,
                "A name",
                null);
        }

        //unitofwork_stateundertest_expectedbehaviour
        //given_when_then

        [Fact]
        public void WhenExecutingHandleCreateCommand_WhithItineraryId_StopItineraryIdMusMatch()
        {
            // Arrange

            // Act
            _stopEntity.HandleCreateCommand(_createStopCommand);

            // Assert
            Assert.Equal(
                _createStopCommand.ItineraryId, 
                _stopEntity.ItineraryId);

        }

        [Fact]
        public void WhenExecutingHandleCreateCommand_WithValidInput_OneStopCreatedEventMustBeAdded()
        {
            // Arrange

            // Act
            _stopEntity.HandleCreateCommand(_createStopCommand);

            // Assert
            Assert.Single(_stopEntity.DomainEvents);
            Assert.IsType<StopCreatedEvent>(_stopEntity.DomainEvents[0]);

        }
    }
}
