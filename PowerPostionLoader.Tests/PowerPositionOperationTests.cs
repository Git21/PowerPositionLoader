using PowerPositionLoader;
using Services;
using Xunit;

namespace PowerPostionLoader.Tests
{
    public class PowerPositionOperationTests
    {
        public PowerPositionOperationTests()
        {

        }
        [Fact]
        public void GivenTradesAreNotEmptyShouldReturnSumOfVolumesForGivenPeriods()
        {
            //Arrange
			var powerTrades = new List<PowerTrade>();
			powerTrades.Add(PowerTrade.Create(DateTime.Now, 24));
			powerTrades[0].Periods.ToList().ForEach(p => p.Volume = 100);
			powerTrades.Add(PowerTrade.Create(DateTime.Now, 24));
			powerTrades[1].Periods.ToList().ForEach(p => p.Volume = 50);

            //Act
            var powerPositionOperation = new PowerPositionOperation();
            var pp = powerPositionOperation.SumPeriods(powerTrades);

            Assert.Equal(24, pp.Count());
            Assert.Equal(150, pp.ToList()[0].Volume);
		}
        [Fact]
        public void GivenTradesAreEmptyShouldReturnSumOfVolumesForGivenPeriods()
        {
            //Arrange
            var powerTrades = new List<PowerTrade>();
           
            //Act
            var powerPositionOperation = new PowerPositionOperation();
            var pp = powerPositionOperation.SumPeriods(powerTrades);

            Assert.Empty(pp);
        }

    }
}
