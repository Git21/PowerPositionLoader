using Services;

namespace PowerPositionLoader
{
    public class PowerPositionOperation : IPowerPositionOperation
    {
       public  IEnumerable<PowerPeriod> SumPeriods(IEnumerable<PowerTrade> tradeList)
        {
            if(tradeList == null)  return Enumerable.Empty<PowerPeriod>();
            return tradeList.SelectMany(t => t.Periods).GroupBy(g => g.Period).Select(so => new PowerPeriod { Period = so.Key, Volume = so.Sum(x => x.Volume) });
        }
    }
}
