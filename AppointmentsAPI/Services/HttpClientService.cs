namespace AppointmentsAPI.Services;

public class HttpClientService
{
    public HttpClient HttpClient { get; }

    public HttpClientService(HttpClient httpClient)
    {
        httpClient.BaseAddress = new Uri("https://localhost:7116/api/");
        HttpClient = httpClient;
    }

    public async Task<bool> IsDoctorExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        using var response = await HttpClient.GetAsync($"Doctors/GetById/{id}", HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();
        //var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        //var doctor = await JsonSerializer.DeserializeAsync<object>(stream, _options, cancellationToken);
        
        return true;
    }
    
    public async Task<bool> IsPatientExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        using var response = await HttpClient.GetAsync($"Patients/GetById/{id}", HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();
        
        return true;
    }
}