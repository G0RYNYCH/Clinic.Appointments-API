using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace AppointmentsAPI.Exceptions;

[ExcludeFromCodeCoverage]
public class ErrorDetails
{
    public int StatusCode { get; set; }

    public string? Message { get; set; }

    public override string ToString() => JsonSerializer.Serialize(this);
}
