using System;
using System.Threading.Tasks;
using AppointmentsAPI.Models.Dto;
using AppointmentsAPI.Models.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace Appointments_API.Tests.Models.Validators;

public class SearchDtoValidatorTests
{
    private readonly SearchDtoValidator _validator;

    public SearchDtoValidatorTests()
    {
        _validator = new SearchDtoValidator();
    }

    [Fact]
    public async Task SearchDtoValidator_ValidModel_Succeeded()
    {
        //Arrange
        var searchDto = new SearchDto()
        {
            PageNumber = 1,
            PageSize = 1
        };

        //Act
        var result = await _validator.TestValidateAsync(searchDto);

        //Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PageNumber);
        result.ShouldNotHaveValidationErrorFor(x => x.PageSize);
    }
    
    [Fact]
    public async Task SearchDtoValidator_InValidPageNumber_ReturnsError()
    {
        //Arrange
        var searchDto = new SearchDto()
        {
            PageNumber = 0,
            PageSize = 1
        };
    
        //Act
        var result = await _validator.TestValidateAsync(searchDto);
    
        //Assert
        result.ShouldHaveValidationErrorFor(x => x.PageNumber)
            .Only()
            .WithErrorMessage("Page number must be greater than 0");
        result.ShouldNotHaveValidationErrorFor(x => x.PageSize);
    }
    
    [Fact]
    public async Task SearchDtoValidator_InValidPageSize_ReturnsError()
    {
        //Arrange
        var searchDto = new SearchDto()
        {
            PageNumber = 1,
            PageSize = 0
        };
    
        //Act
        var result = await _validator.TestValidateAsync(searchDto);
    
        //Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PageNumber);
        result.ShouldHaveValidationErrorFor(x => x.PageSize)
            .Only()
            .WithErrorMessage("Page size must be greater than 0");
    }
}