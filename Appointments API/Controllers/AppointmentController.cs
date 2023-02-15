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
    public async Task<IActionResult> GetAppointments([FromQuery] SearchDto searchDto)
    {
        var paginatedAppointments = await _appointmentService.SearchAsync(searchDto, HttpContext.RequestAborted);

        return Ok(paginatedAppointments);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var appoitment = await _appointmentService.GetByIdAsync(id, HttpContext.RequestAborted);

        return Ok(appoitment);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AppointmentDto appointmentDto)
    {
        await _appointmentService.CreateAsync(appointmentDto, HttpContext.RequestAborted);

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateAppointmentDto updateAppointmentDto)
    {
        await _appointmentService.UpdateAsync(id, updateAppointmentDto, HttpContext.RequestAborted);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var existingAppointment = await _appointmentService.GetByIdAsync(id, HttpContext.RequestAborted);
        if (existingAppointment == null)
        {
            return BadRequest($"Appointment with Id = {id} not found");
        }
        await _appointmentService.DeleteAsync(id, HttpContext.RequestAborted);

        return NoContent();
    }
}
