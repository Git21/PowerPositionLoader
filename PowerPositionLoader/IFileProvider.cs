using Services;

namespace PowerPositionLoader
{
    public interface IFileProvider
    {
        Task<bool> WriteToFile(string fileName, IEnumerable<PowerPeriod> content, Dictionary<int, string> localTimeMapper);
    }
}
