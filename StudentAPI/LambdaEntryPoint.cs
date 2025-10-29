using Microsoft.AspNetCore.Hosting;
using Amazon.Lambda.AspNetCoreServer;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace StudentAPI
{
    public class LambdaEntryPoint : APIGatewayHttpApiV2ProxyFunction
    {
        protected override void Init(IWebHostBuilder builder)
        {
            builder
                .ConfigureServices((context, services) =>
                {
                    // Add services to the container
                    services.AddControllers();
                    services.AddEndpointsApiExplorer();
                    services.AddAuthentication();
                    services.AddAuthorization();
                    services.AddSwaggerGen(c =>
                    {
                        c.SwaggerDoc("v1", new OpenApiInfo
                        {
                            Title = "Student API",
                            Version = "v1",
                            Description = "An API to manage student information"
                        });
                    });
                })
                .Configure(app =>
                {
                    app.UseSwagger();
                    app.UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Student API V1");
                        c.RoutePrefix = "swagger";
                    });

                    app.UseHttpsRedirection();
                    app.UseRouting();
                    app.UseAuthentication();
                    app.UseAuthorization();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });
                });
        }

        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
        public async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
        {
            var response = await base.FunctionHandlerAsync(request, context);
            context.Logger.LogLine($"Request: {System.Text.Json.JsonSerializer.Serialize(request)}");
            context.Logger.LogLine($"Response: {System.Text.Json.JsonSerializer.Serialize(response)}");
            return response;
        }
    }
}