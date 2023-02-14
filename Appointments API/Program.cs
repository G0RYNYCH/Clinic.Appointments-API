using Appointments_API.Interfaces;
using Appointments_API.Models.Dto;
using Appointments_API.Models.Validators;
using Appointments_API.Repositories;
using Appointments_API.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMvc();//TODO: why do we add Mvc to pipeline
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddTransient<IValidator<AppointmentDto>, AppointmentDtoValidator>();
builder.Services.AddTransient<IValidator<UpdateAppointmentDto>, UpdateAppointmentDtoValidator>();
builder.Services.AddTransient<IValidator<SearchDto>, SearchDtoValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
                {
                    options.MapType<DateOnly>(() => 
                    new OpenApiSchema 
                    { 
                        Type = "string",
                        Format = "date",
                        Example = new OpenApiString("2023-01-01")
                    });
                    options.MapType<TimeOnly>(() => 
                    new OpenApiSchema
                    { 
                        Type = "string",
                        Format = "time",
                        Example = new OpenApiString("12:01:01")
                    });
                });
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<AppointmentDbContext>(x =>
        x.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
