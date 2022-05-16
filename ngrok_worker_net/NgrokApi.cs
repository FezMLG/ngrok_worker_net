using System.Net.Mail;
using NgrokApi;

namespace ngrok_worker_net;
public class NgrokApi
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;

    public NgrokApi (ILogger<Worker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }
    public async Task<string?> Main()
    {
        Console.WriteLine("Running ngrok");
        var ngrok = new Ngrok(_configuration["NGROK_API_KEY"]);
        string? url = "";
        await foreach (var t in ngrok.Tunnels.List())
        {
            Console.WriteLine(t.PublicUrl);
            url = t.PublicUrl;
        }

        return url;
    }
}