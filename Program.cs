using AutoMapper;
using KiwiTaskAPI.Database;
using KiwiTaskAPI.Options;
using KiwiTaskAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace KiwiTaskAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 把一个接口和它的实现类注册到容器中
            builder.Services.AddScoped<ITaskRepository, TaskRepository>();
            builder.Services.AddScoped<IAuthServiceRepository, AuthServiceRepository>();
            builder.Services.AddScoped<IMailService, MailServiceRepository>();
            builder.Services.AddScoped<ITaskCategoryRepository, TaskCategoryRepository>();
            builder.Services.AddScoped<IOssService, OssServiceRepository>();
            builder.Services.AddScoped<ITaskMediaService, TaskMediaServiceRepository>();

            builder.Services.AddHttpClient<IPlaceService, PlaceServiceRepository>(client =>
            {
                client.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/");
                client.Timeout = TimeSpan.FromSeconds(10);
            });

            // 注册控制器的服务
            builder.Services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
            }).AddXmlDataContractSerializerFormatters();

            // 将AppDbContext 注册到依赖注入容器中
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            );

            // AutoMapper 会自动扫描项目里的profile文件，然后通过调用AppDomain.CurrentDomain.GetAssemblies()，
            // AutoMapper会将所有的profile文件加载到AppDomain中
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("SmtpOptions"));

            builder.Services.AddOptions<OssOptions>().Bind(builder.Configuration.GetSection("AliyunOss")).ValidateDataAnnotations().ValidateOnStart();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer(opt =>
                            {
                                var keyBytes = Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretKey"]);
                                opt.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuer = false,
                                    ValidateAudience = false,
                                    ValidateLifetime = true,
                                    ValidateIssuerSigningKey = true,
                                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                                    ClockSkew = TimeSpan.Zero
                                };
                                opt.Events = new JwtBearerEvents
                                {
                                    OnMessageReceived = context =>
                                    {
                                        var accessToken = context.Request.Query["access_token"];
                                        var path = context.HttpContext.Request.Path;

                                        if(string.IsNullOrEmpty(context.Token) && !string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                                        {
                                            context.Token = accessToken;
                                        }
                                        return Task.CompletedTask;
                                    }
                                };
                            });

            builder.Services.AddAuthorization();

            // CORS

            builder.Services.AddCors(o =>
            {
                o.AddPolicy("Frontend", p => p.WithOrigins("http://localhost:3000", "https://kiwisquare.co.nz").AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            });
            

            var app = builder.Build();

            app.UseCors("Frontend");
            app.UseAuthentication();
            app.UseAuthorization();


            // 请求处理管道，它将控制器的路由映射到处理请求的管道中
            app.MapControllers();

            app.Run();
        }
    }
}
