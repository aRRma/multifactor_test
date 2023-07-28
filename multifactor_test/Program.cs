using Microsoft.AspNetCore.Http.Json;
using multifactor_test.Models;
using multifactor_test.Models.Dto;
using multifactor_test.Services;
using System.Net.Mime;
using System.Text.Json.Serialization;

namespace multifactor_test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services
                .AddSingleton<AccessService>()
                .AddEndpointsApiExplorer().AddSwaggerGen()
                .Configure<JsonOptions>(_ => { _.SerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
            var app = builder.Build();
            app.UseSwagger().UseSwaggerUI();

            app.MapPost("/requests", async (HttpContext context, AccessService accessService) =>
            {
                var request = await Helpers.GetRequestObjectAsync<ApiRequestDto>(context);
                if (!Helpers.ValidateRequest(request)) return Results.BadRequest();

                try
                {
                    var result = await accessService.CheckAccessInRegistryAsync(request.Resource);
                    return Results.Ok(new ApiResponseDto()
                    {
                        Decision = result ? DecisionType.Granted : DecisionType.Denied,
                        Reason = result ? "" : ErrorMessageConstants.DeniedByUser,
                        Resource = request.Resource
                    });
                }
                catch (TaskCanceledException)
                {
                    return Results.Ok(new ApiResponseDto()
                    {
                        Decision = DecisionType.Denied,
                        Reason = ErrorMessageConstants.TimeOut,
                        Resource = request.Resource
                    });
                }
            }).Accepts<ApiRequestDto>(MediaTypeNames.Application.Json);

            app.MapPost("/access", async (HttpContext context, AccessService accessService) =>
            {
                var request = await Helpers.GetRequestObjectAsync<ApiAccessRequestDto>(context);
                if (!Helpers.ValidateRequest(request)) return Results.BadRequest();

                accessService.AddInRegistry(request.Resource, request.Action);
                return Results.Ok();
            }).Accepts<ApiAccessRequestDto>(MediaTypeNames.Application.Json);

            app.Run();
        }
    }
}