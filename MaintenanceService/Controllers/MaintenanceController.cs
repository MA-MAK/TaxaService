using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("[controller]")]
[ApiController]
public class MaintenanceController : ControllerBase
{
    private readonly MaintenanceRepository _maintenanceRepository;

    public MaintenanceController(MaintenanceRepository maintenanceRepository)
    {
        _maintenanceRepository = maintenanceRepository;
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

    [HttpPost]
    public async Task<ActionResult> CreateMaintenanceVisit([FromBody] MaintenanceVisit maintenanceVisit)
    {
        await _maintenanceRepository.InsertMaintenanceVisit(maintenanceVisit);
        return CreatedAtAction(nameof(GetMaintenanceVisitById), new { id = maintenanceVisit.Id }, maintenanceVisit);
    }

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
}