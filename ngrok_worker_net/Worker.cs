namespace ngrok_worker_net;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;
    private string? _lastUrl;

    public Worker(ILogger<Worker> logger, IConfiguration configuration)
    {
        var last = Environment.GetEnvironmentVariable("LAST_NGROK_URL");
        Console.WriteLine("Last ngrok url: " + last);
        if (string.IsNullOrEmpty(last))
        {
            Console.WriteLine("Not exist, setting empty url");
            last = "";
            Environment.SetEnvironmentVariable("LAST_NGROK_URL", "");
        }
        Console.WriteLine(last);
        _logger = logger;
        _configuration = configuration;
        _lastUrl = Environment.GetEnvironmentVariable("LAST_NGROK_URL");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            NgrokApi ngrokApi = new NgrokApi(configuration: _configuration, logger: _logger);
            string? url = await ngrokApi.Main();
            if (_lastUrl != url)
            {
                Console.WriteLine("Url not matching: " + url);
                _lastUrl = url;
                Environment.SetEnvironmentVariable("LAST_NGROK_URL", url);
                EmailSender emailSender = new EmailSender(configuration: _configuration, logger: _logger);
                emailSender.SendMail(body: _lastUrl);
            }

            await Task.Delay(5000, stoppingToken);
        }
    }
}