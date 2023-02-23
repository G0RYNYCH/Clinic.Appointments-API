using System.Diagnostics.CodeAnalysis;

namespace AppointmentsAPI.Models;

[ExcludeFromCodeCoverage]
public class EntityBase
{
    public Guid Id { get; set; }
}
