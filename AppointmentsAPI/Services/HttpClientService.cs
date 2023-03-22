namespace AppointmentsAPI.Services;

public class HttpClientService
{
    public HttpClient HttpClient { get; }
    private readonly string _baseAddress;

    public HttpClientService(HttpClient httpClient, IConfiguration configuration)
    {
        _baseAddress = configuration.GetValue<string>("HttpClientUrl");
        httpClient.BaseAddress = new Uri(_baseAddress);
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