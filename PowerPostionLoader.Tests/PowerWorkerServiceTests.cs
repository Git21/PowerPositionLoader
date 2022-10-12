using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PowerPositionLoader;
using Services;
using Xunit;
namespace PowerPostionLoader.Tests
{
    
    public class PowerWorkerServiceTests
    {
        public Mock<ILogger<PowerWorkerService>> mockLogger;
        private Mock<IPowerService> mockPowerService;
        private Mock<IFileProvider> mockFileProvider;
        private Mock<IOptions<PowerPositionOptions>> mockOptions;
        private Mock<IPowerPositionOperation> mockOperations;
        public PowerWorkerServiceTests()
        {
            mockLogger = new Mock<ILogger<PowerWorkerService>>();
            mockPowerService =  new Mock<IPowerService>();

            mockFileProvider = new Mock<IFileProvider>();
            
            mockOptions = new Mock<IOptions<PowerPositionOptions>>();
            mockOptions.Setup(s => s.Value).Returns(new PowerPositionOptions { ExtractInterval=60, FilePath=""});

            mockOperations = new Mock<IPowerPositionOperation>();
            var powerPeriods = new List<PowerPeriod>();
            powerPeriods.Add(new PowerPeriod{ Period = 1, Volume=150});
            powerPeriods.Add(new PowerPeriod { Period = 2, Volume = 150 });
            mockOperations.Setup(o => o.SumPeriods(It.IsAny<IEnumerable<PowerTrade>>())).Returns(powerPeriods);
        }  
        
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
        [Fact]
        public async void GivenTradesAreReturnedWriteToFileCalled()
        {
            //Arrange
            mockPowerService.Setup(p => p.GetTradesAsync(It.IsAny<DateTime>())).Returns(Task.FromResult(GetPowerTrades().AsEnumerable<PowerTrade>()));

            var workerService = new PowerWorkerService(mockLogger.Object, mockPowerService.Object, mockFileProvider.Object, 
                mockOperations.Object, mockOptions.Object);
            //Act
            await workerService.StartAsync(CancellationToken.None);
            //Assert
            mockFileProvider.Verify(fp => fp.WriteToFile(It.IsAny<string>(), It.IsAny<IEnumerable<PowerPeriod>>(), It.IsAny<Dictionary<int, string>>()), Times.AtLeastOnce);
        }

        [Fact]
        public void GivenTradesReturnedExceptionItThrowsException()
        {
            //Arrange
            mockPowerService.Setup(p => p.GetTradesAsync(It.IsAny<DateTime>())).Throws(new PowerServiceException("Unit Test Error"));

            var workerService = new PowerWorkerService(mockLogger.Object, mockPowerService.Object, mockFileProvider.Object,
                mockOperations.Object, mockOptions.Object);

            _ = Assert.ThrowsAsync<Exception>(async () => await workerService.StartAsync(CancellationToken.None));
        }

        private List<PowerTrade> GetPowerTrades()
        {
            var powerTrades = new List<PowerTrade>();
            powerTrades.Add(PowerTrade.Create(DateTime.Now, 24));
            powerTrades[0].Periods.ToList().ForEach(p => p.Volume = 100);
            powerTrades.Add(PowerTrade.Create(DateTime.Now, 24));
            powerTrades[1].Periods.ToList().ForEach(p => p.Volume = 50);

            return powerTrades;
        }
    }
}