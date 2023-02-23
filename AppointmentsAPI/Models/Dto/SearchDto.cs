using System.Diagnostics.CodeAnalysis;

namespace AppointmentsAPI.Models.Dto;

[ExcludeFromCodeCoverage]
public class SearchDto
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}
