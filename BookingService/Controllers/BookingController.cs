using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;
using RabbitMQ.Client;
using BookingService;
using MongoDB.Driver;
using MongoDB.Bson;

namespace BookingService.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingController : ControllerBase
{
    private string _planPath = string.Empty;
    private string _mqHost = string.Empty;
    private readonly UserRepository _userRepo;
    private readonly ILogger<BookingController> _logger;

    public BookingController(ILogger<BookingController> logger, IConfiguration configuration, UserRepository userrepo)
    {
        _logger = logger;
        _planPath = configuration["PlanPath"] ?? String.Empty;
        _mqHost = configuration["rabbitmqHost"] ?? "localhost";
        _userRepo = userrepo;
    }


    [HttpGet("GetExample")]
    public IActionResult GetExample()
    {
        Plan ex = new Plan("jens", DateTime.Now, "start", "stop");

        string exAsJson = JsonSerializer.Serialize(ex);
        return Ok(exAsJson);
    }

    [HttpGet("GetPlan")]
    public IActionResult GetPlan()
    {
        if (Directory.Exists(_planPath))
        {
            string[] fileEntries = Directory.GetFiles(_planPath);
            var planURI = new Uri(fileEntries[0], UriKind.RelativeOrAbsolute);
            return Ok(planURI);
        }
        else
        {
            return StatusCode(500, $"error: {_planPath}");
        }
    }

    [HttpPost("PostBooking")]
    public ActionResult<Plan> PostBooking(Plan plan)
    {
        _logger.LogInformation("posting..");
        try
        {
            //Plan Plan = JsonSerializer.Deserialize<Plan>(jsonString);

            var factory = new ConnectionFactory { HostName = _mqHost };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "booking",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            string message = JsonSerializer.Serialize(plan);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: "booking",
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine($" [x] Sent {message}");

        }
        catch (Exception ex)
        {
            _logger.LogInformation($"exception: {ex.Message}");
            return StatusCode(500, $"{ex.Message}");
        }
        _logger.LogInformation($"OK: Booking posted");
        return Ok(plan);
    }
}