using ASP.NETCoreWebApi.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
namespace ASP.NETCoreWebApi
{
    public class Program
    {
        static void Main()
        {
            try
            {
                var app = BuildApi();

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseAuthorization();

                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\n");
            }
        }
        static WebApplication BuildApi()
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder();

            IServiceCollection services = builder.Services;

            services.AddControllers(options =>
            {
                options.Filters.Add<JsonExceptionFilter>();
            });

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new MediaTypeApiVersionReader();
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
            });

            services.AddSwaggerGen(); // -D

            services.AddEndpointsApiExplorer();

            return builder.Build();
        }
    }
}