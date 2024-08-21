using FlightsData;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace TestHelpers
{
    public class MockFlightsContextFactory
    {
        protected MockFlightsContextFactory() { }

        public async static Task<IDbContextFactory<FlightsContext>> GetFlightsContextFactory(FlightsContext context)
        {
            var factory = new Mock<IDbContextFactory<FlightsContext>>();
            factory
                .Setup(m => m.CreateDbContextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(context);
            return await Task.FromResult(factory.Object);
        }
    }
}
