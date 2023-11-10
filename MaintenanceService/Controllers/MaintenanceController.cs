using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;
using RabbitMQ.Client;
using MongoDB.Driver;
using MongoDB.Bson;


[Route("[controller]")]
[ApiController]
public class MaintenanceController : ControllerBase
{
    private readonly MaintenanceRepository _maintenanceRepository;
    private string _mqHost = string.Empty;

    private readonly ILogger<MaintenanceController> _logger;

    public MaintenanceController(MaintenanceRepository maintenanceRepository, IConfiguration configuration, ILogger<MaintenanceController> logger)
    {
        _maintenanceRepository = maintenanceRepository;
        _mqHost = configuration["rabbitmqHost"] ?? "localhost";
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<MaintenanceVisit>>> GetAllMaintenanceVisits()
    {
        var maintenanceVisits = await _maintenanceRepository.GetAllMaintenanceVisits();
        return Ok(maintenanceVisits);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MaintenanceVisit>> GetMaintenanceVisitById(string id)
    {
        var maintenanceVisit = await _maintenanceRepository.GetMaintenanceVisitById(id);

        if (maintenanceVisit == null)
        {
            return NotFound();
        }

        return Ok(maintenanceVisit);
    }

    
// Post til RabbitMQ - Og videre til PlanService
    [HttpPost]
    public async Task<ActionResult> CreateMaintenanceVisit([FromBody] MaintenanceVisit maintenanceVisit)
    {
        _logger.LogInformation("posting..");
        try
        {

            var factory = new ConnectionFactory { HostName = _mqHost };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "maintenanceVisits",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            string message = JsonSerializer.Serialize(maintenanceVisit);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: "maintenanceVisits",
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine($" [x] Sent {message}");

        }
        catch (Exception ex)
        {
            _logger.LogInformation($"exception: {ex.Message}");
            return StatusCode(500, $"{ex.Message}");
        }
        _logger.LogInformation($"OK: Maintenance Request posted");
        return Ok(maintenanceVisit);
    }

/*

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateMaintenanceVisit(string id, [FromBody] MaintenanceVisit updatedMaintenanceVisit)
    {
        await _maintenanceRepository.UpdateMaintenanceVisit(id, updatedMaintenanceVisit);
        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMaintenanceVisit(string id)
    {
        await _maintenanceRepository.DeleteMaintenanceVisit(id);
        return NoContent();
    }
    */
}