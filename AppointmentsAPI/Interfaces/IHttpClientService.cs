namespace AppointmentsAPI.Interfaces;

public interface IHttpClientService
{
    public HttpClient HttpClient { get; }

    public Task<bool> IsDoctorExistsAsync(Guid id, CancellationToken cancellationToken);

    public Task<bool> IsPatientExistsAsync(Guid id, CancellationToken cancellationToken);
}