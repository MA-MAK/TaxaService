using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


[Route("[controller]")]
[ApiController]
public class VehiclesController : ControllerBase
{
    private readonly VehicleRepository _vehicleRepository;

    public VehiclesController(VehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<Vehicle>>> GetAllVehicles()
    {
        var vehicles = await _vehicleRepository.GetAllVehicles();
        return Ok(vehicles);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Vehicle>> GetVehicleById(int id)
    {
        var vehicle = await _vehicleRepository.GetVehicleById(id);

        if (vehicle == null)
        {
            return NotFound();
        }

        return Ok(vehicle);
    }

    [HttpPost]
    public async Task<ActionResult> CreateVehicle([FromBody] Vehicle vehicle)
    {
        await _vehicleRepository.InsertVehicle(vehicle);
        return CreatedAtAction(nameof(GetVehicleById), new { id = vehicle.Id }, vehicle);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateVehicle(int id, [FromBody] Vehicle updatedVehicle)
    {
        await _vehicleRepository.UpdateVehicle(id, updatedVehicle);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteVehicle(int id)
    {
        await _vehicleRepository.DeleteVehicle(id);
        return NoContent();
    }
}

