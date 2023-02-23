using AppointmentsAPI.Interfaces;
using AppointmentsAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentsAPI.Controllers;

[ApiController]
[Route("api/appointments")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly ILogger<AppointmentController> _logger;

    public AppointmentController(IAppointmentService appointmentService, ILogger<AppointmentController> logger)
    {
        _appointmentService = appointmentService ?? throw new ArgumentNullException(nameof(appointmentService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public async Task<IActionResult> GetAppointments([FromQuery] SearchDto searchDto)
    {
        _logger.LogInformation("GetAppointments method is called");

        var paginatedAppointments = await _appointmentService.SearchAsync(searchDto, HttpContext.RequestAborted);

        _logger.LogInformation("GetAppointments method succeeded");

        return Ok(paginatedAppointments);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        _logger.LogInformation("GetById method is called");

        var appointment = await _appointmentService.GetByIdAsync(id, HttpContext.RequestAborted);
        if (appointment == null)
        {
            _logger.LogInformation($"Appointment with Id = {id} is null");

            return BadRequest($"Appointment with Id = {id} not found");
        }

        _logger.LogInformation("GetById method succeeded");

        return Ok(appointment);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AppointmentDto appointmentDto)
    {
        _logger.LogInformation("Create method is called");

        await _appointmentService.CreateAsync(appointmentDto, HttpContext.RequestAborted);

        _logger.LogInformation("Create method succeeded");

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateAppointmentDto updateAppointmentDto)//TODO: optimize query
    {
        _logger.LogInformation("Update method is called");

        var appointment = await _appointmentService.GetByIdAsync(id, HttpContext.RequestAborted);
        if (appointment == null)
        {
            _logger.LogInformation($"Appointment with Id = {id} is null");

            return BadRequest($"Appointment with Id = {id} not found");
        }
        await _appointmentService.UpdateAsync(id, updateAppointmentDto, HttpContext.RequestAborted);

        _logger.LogInformation("Update method succeeded");

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)//TODO: optimize query
    {
        _logger.LogInformation("Delete method is called");

        var existingAppointment = await _appointmentService.GetByIdAsync(id, HttpContext.RequestAborted);
        if (existingAppointment == null)
        {
            _logger.LogInformation($"Appointment with Id = {id} is null");

            return BadRequest($"Appointment with Id = {id} not found");
        }
        await _appointmentService.DeleteAsync(id, HttpContext.RequestAborted);

        _logger.LogInformation("Delete method succeeded");

        return NoContent();
    }
}
