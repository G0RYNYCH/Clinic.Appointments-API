using Appointments_API.Interfaces;
using Appointments_API.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Appointments_API.Controllers;

[ApiController]
[Route("api/appointments")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAppointments([FromQuery] SearchDto searchDto, CancellationToken cancellationToken) //TODO: [FromBody]
    {
        var paginatedAppointments = await _appointmentService.SearchAsync(searchDto, cancellationToken);

        return Ok(paginatedAppointments);
    }

    [HttpGet("{id:guid}")] //TODO: [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromQuery]  Guid id, CancellationToken cancellationToken)
    {
        var appoitment = await _appointmentService.GetByIdAsync(id, cancellationToken);

        return Ok(appoitment);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AppointmentDto appointmentDto, CancellationToken cancellationToken)
    {
        await _appointmentService.CreateAsync(appointmentDto, cancellationToken);

        return Ok();//TODO:
    }

    [HttpPut("{id:guid}")] //TODO: HttpPatch
    public async Task<IActionResult> Update(Guid id, UpdateAppointmentDto updateAppointmentDto, CancellationToken cancellationToken)
    {
        await _appointmentService.UpdateAsync(id, updateAppointmentDto, cancellationToken);

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _appointmentService.DeleteAsync(id, cancellationToken);

        return NoContent();
    }
}
