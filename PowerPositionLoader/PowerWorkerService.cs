using Microsoft.Extensions.Options;
using Services;

namespace PowerPositionLoader
{
    public class PowerWorkerService : BackgroundService
    {
        private readonly ILogger<PowerWorkerService> _logger;
        private readonly PeriodicTimer _timer;
        private readonly IPowerService _service;
        private readonly IFileProvider _provider;
        private readonly IOptions<PowerPositionOptions> _appSettings;
        private readonly IPowerPositionOperation _ppoService;
        private LocalTimeSlotsToPeriodConverter _converter;
        private DateTime _dateTime;
        public PowerWorkerService(ILogger<PowerWorkerService> logger,
            IPowerService service, IFileProvider provider, IPowerPositionOperation powerPositionOperation,
            IOptions<PowerPositionOptions> appSettings)
        {
            _logger = logger;
            _service = service;
            _provider = provider;
            _appSettings = appSettings;
            _dateTime = DateTime.Now;
            _ppoService = powerPositionOperation;
            _converter = new LocalTimeSlotsToPeriodConverter();
            _timer = new PeriodicTimer(TimeSpan.FromMinutes(_appSettings.Value.ExtractInterval));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Windows Service Executing...");
            _converter.DefinePeriodMappingToTimeSlots(_dateTime);
            var retVal = await DoWorkAsync(_dateTime);
            while (await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
            {                
                if (retVal)
                    _dateTime = _dateTime.AddMinutes(_appSettings.Value.ExtractInterval);
                
                retVal = await DoWorkAsync(_dateTime);
            }
        }

        private async Task<bool> DoWorkAsync(DateTime dateTime)
        {
            try
            {
                var trades = await _service.GetTradesAsync(dateTime);

                var res = _ppoService.SumPeriods(trades);
                var localTimeMaps = _converter.PeriodToLocalTimeDictionary;
                return await _provider.WriteToFile(string.Format(_appSettings.Value.FilePath,_dateTime), res, localTimeMaps);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
