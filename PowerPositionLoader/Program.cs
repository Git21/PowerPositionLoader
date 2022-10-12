using PowerPositionLoader;
using Services;
using PowerService = Services.PowerService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<PowerWorkerService>();
        services.AddSingleton<IPowerService, PowerService>();
        services.AddSingleton<IFileProvider, FileProvider>();
        services.AddSingleton<IPowerPositionOperation, PowerPositionOperation>();
    })
    .ConfigureAppConfiguration((context, builder) => 
                    builder.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json",false,true)
                    .AddJsonFile("appsettings.Development.json", true, true)
                    )
    .ConfigureServices((context, builder) =>
            builder.AddOptions().Configure<PowerPositionOptions>(context.Configuration.GetSection("PowerPosistionFile"))
    ).Build();

await host.RunAsync();
