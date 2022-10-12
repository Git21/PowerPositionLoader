using Services;

namespace PowerPositionLoader
{
    public interface IPowerPositionOperation
    {
        IEnumerable<PowerPeriod> SumPeriods(IEnumerable<PowerTrade> tradeList);
    }
}
