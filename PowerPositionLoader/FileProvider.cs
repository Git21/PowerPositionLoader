using Services;
using System.Text;

namespace PowerPositionLoader
{
    public class FileProvider : IFileProvider
    {
        private readonly ILogger<FileProvider> _logger;
        public FileProvider(ILogger<FileProvider> logger)
        {
            _logger = logger;
        }
        public async Task<bool> WriteToFile(string fileName, IEnumerable<PowerPeriod> content, Dictionary<int, string> localTimeMapper)
        {            
            StringBuilder sb = new StringBuilder();

            try
            {
                _logger.LogInformation("Writting to the file");
                sb.AppendLine("Local Time,Volume");
                content.ToList().ForEach(c => sb.AppendLine($"{localTimeMapper[c.Period]},{c.Volume}"));
                var isValid = ValidateOptions(fileName);
                if (!isValid.HasValue || isValid == false)
                {
                    _logger.LogError("Invalid File Path. Directory doesnot exists");
                    return false;
                }

               return await WriteStreamToFile(fileName, sb.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
        private bool? ValidateOptions(string fileName)
        {
            if(string.IsNullOrWhiteSpace(fileName)) return false;
            return Directory.GetParent(fileName)?.Exists;
        }

        private async Task<bool> WriteStreamToFile(string fileName, string content)
        {
            try
            {
                using (var stream = new StreamWriter(fileName))
                {
                    await stream.WriteAsync(content);
                    await stream.FlushAsync();
                    _logger.LogInformation("Writting to file Complete");
                }            
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
            return true;
        }
    }
}
