using Microsoft.AspNetCore.Mvc;
using OpenApi.ErrorCodes.Library.Attributes;
using OpenApi.ErrorCodes.Library.Console.Constants;
using OpenApi.ErrorCodes.Library.Console.OpenApi;
using OpenApi.ErrorCodes.Library.Console.Requests;
using OpenApi.ErrorCodes.Library.Console.Responses;
using OpenApi.ErrorCodes.Library.Filters.Operation;

var MapAttributeToResponse = (CodeSubCodeDescriptionLinkAttribute data, int constant) =>
{
    return new CodeSubCodeResponse()
    {
        Code = data.Code,
        SubCode = constant,
        Description = data.Description,
        Link = data.Link
    };
};

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = builder.Environment.ApplicationName,
        Version = "v1",
    });
    /// add next line
    c.OperationFilter<ResponseCodeFromConstantsOperationFilter<CodeSubCodeResponse, CodeSubCodeDescriptionLinkAttribute>>(MapAttributeToResponse);
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{builder.Environment.ApplicationName} v1"));

var HomeFn =
    // add next line
    [ResponseCodeFromConstants("List of error codes", typeof(ResponseSubCodes))]
([FromBody] InputRequest input) => {

    return "Hello word";
};

app.MapPost("/", HomeFn);
app.Run();