Imports BackendAPI
Imports BackendAPI.Model
Imports BackendAPI.Notification
Imports BackendAPI.Services
Imports FrontEnd.Data
Imports FrontEndData
Imports FrontEndData.Models
Imports Microsoft.AspNetCore.Authentication.Cookies
Imports Microsoft.AspNetCore.Builder
Imports Microsoft.AspNetCore.Hosting
Imports Microsoft.AspNetCore.Http
Imports Microsoft.AspNetCore.Http.Connections
Imports Microsoft.AspNetCore.HttpOverrides
Imports Microsoft.AspNetCore.Identity
Imports Microsoft.AspNetCore.Mvc.ApplicationModels
Imports Microsoft.AspNetCore.Mvc.RazorPages
Imports Microsoft.AspNetCore.Routing
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.FileProviders
Imports Microsoft.Extensions.Hosting
Imports Microsoft.Extensions.Logging
Imports Microsoft.OpenApi.Models
Imports System.Diagnostics
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.AspNetCore
Imports Microsoft.Extensions.Options
Imports Newtonsoft.Json
Imports BackendAPI.FrontEndModels
Imports Microsoft.AspNetCore.Mvc.Infrastructure
Imports Microsoft.AspNetCore.StaticFiles

Module Program

    Public Property Environment As IWebHostEnvironment
    Public Property LoggerFactory As ILoggerFactory
    Public Property Configuration As IConfiguration
    Sub Main(Args() As String)
        Dim Builder = WebApplication.CreateBuilder(Args)
        'Add services to the container.
        Debug.WriteLine($"ContentRootPath: {Builder.Environment.ContentRootPath}, WebRootPath: {Builder.Environment.WebRootPath}, StaticFilesRoot: {Builder.Configuration("StaticFilesRoot")}, RazorPagesRoot: ?,  WebRootFileProvider: {Builder.Environment.WebRootFileProvider}, IsDevelopment: {Builder.Environment.IsDevelopment} ")

        Builder.Host.ConfigureLogging(Sub(hostingContext, logging)
                                          logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"))
                                          logging.AddConsole
                                          logging.AddDebug
                                          'logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Debug)
                                          logging.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Debug)
                                      End Sub)

        Builder.WebHost.ConfigureKestrel(Sub(KestrelServerOptions)
                                             KestrelServerOptions.ListenLocalhost(5158)
                                             KestrelServerOptions.ListenAnyIP(7168, Function(X) X.UseHttps)
                                         End Sub)
        Environment = Builder.Environment
        Configuration = Builder.Configuration

        Builder.Services.AddHttpContextAccessor
        Builder.Services.AddSingleton(Of IActionContextAccessor, ActionContextAccessor)

        'The call to AddMvc also calls AddRazorPages internally
        Builder.Services.AddMvcCore(Sub(MvcOptions) MvcOptions.EnableEndpointRouting = False)

        Dim AES As New AesCryptor

        Builder.Services.AddDbContext(Of ApplicationDbContext)(Function(ByVal options As DbContextOptionsBuilder)
                                                                   Return options.UseMySql(AES.DecryptSqlConnection(Builder.Configuration.GetConnectionString("DefaultConnection"), "XXXXXXXXXXXXXXXXX"),
                                                            ServerVersion.Parse("10.5.9-MariaDB-1:10.5.9+maria~xenial"), 'SHOW VARIABLES LIKE "%version%";
                                                            Sub(ByVal mySqlOption As Microsoft.EntityFrameworkCore.Infrastructure.MySqlDbContextOptionsBuilder)
                                                                mySqlOption.CommandTimeout(10)
                                                                mySqlOption.EnableRetryOnFailure(10)
                                                            End Sub)
                                                               End Function, ServiceLifetime.Transient, ServiceLifetime.Transient)

        ' configure strongly typed settings object
        Builder.Services.Configure(Of Jwt.JwtSettings)(Builder.Configuration.GetSection("JwtSetting"))

        'configure DI for application services
        Builder.Services.AddScoped(Of IUserService, UserService)
        Builder.Services.AddSingleton(Of IAesCryptor, AesCryptor)
        'Builder.Services.AddSingleton(Of INotificationCacheService, NotificationCacheService)

        Builder.Services.AddDatabaseDeveloperPageExceptionFilter

        Builder.Services.AddIdentity(Of ApplicationUser, ApplcationRole).AddDefaultUI().AddDefaultTokenProviders().AddUserStore(Of CustomUserStore).AddRoleStore(Of CustomRoleStore)()

     
        Builder.Services.Configure(Of IdentityOptions)(Sub(options)
                                                           options.SignIn.RequireConfirmedPhoneNumber = False
                                                           options.SignIn.RequireConfirmedEmail = False
                                                           options.SignIn.RequireConfirmedAccount = False
                                                           ''Password settings.
                                                           'options.Password.RequireDigit = True
                                                           'options.Password.RequireLowercase = True
                                                           'options.Password.RequireNonAlphanumeric = True
                                                           'options.Password.RequireUppercase = True
                                                           'options.Password.RequiredLength = 6
                                                           'options.Password.RequiredUniqueChars = 1
                                                           ''Lockout settings.
                                                           'options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5)
                                                           'options.Lockout.MaxFailedAccessAttempts = 5
                                                           'options.Lockout.AllowedForNewUsers = True
                                                           ''User settings.
                                                           'options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"
                                                           'options.User.RequireUniqueEmail = False
                                                       End Sub)

        ' ******* Configure the HTTP request pipeline. *******
        Dim App = Builder.Build

        Dim I As Integer = 0
        Builder.Services.OrderBy(Function(Z) Z.Lifetime.ToString).ThenBy(Function(Z) Z.ServiceType.ToString).Distinct.ToList.ForEach(Sub(X)
                                                                                                                                         I = I + 1
                                                                                                                                         Dim ST As Type = X.ImplementationType
                                                                                                                                         If ST IsNot Nothing Then
                                                                                                                                             Dim ServiceInstance = Nothing
                                                                                                                                             Try
                                                                                                                                                 ServiceInstance = App.Services.GetRequiredService(ST)
                                                                                                                                             Catch ex As System.InvalidOperationException
                                                                                                                                             End Try
                                                                                                                                             Debug.WriteLine($"{I}. {X.Lifetime} : {IIf(ServiceInstance IsNot Nothing, "HasInstance", "           ")} : {X.ServiceType}")
                                                                                                                                         End If
                                                                                                                                     End Sub)
        'Scoped    : one instance per web request
        'Transient : each time the service is requested, a new instance is created

        LoggerFactory = App.Services.GetRequiredService(Of ILoggerFactory)

        If App.Environment.IsDevelopment Then
            'The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        Else
            App.UseExceptionHandler("/Home/Error")
            App.UseHsts 'HTTPS Redirection Middleware (UseHttpsRedirection) to redirect HTTP requests to HTTPS. HSTS Middleware (UseHsts) to send HTTP Strict Transport Security Protocol (HSTS) headers to clients.
        End If

        App.UseForwardedHeaders(New ForwardedHeadersOptions With {
            .ForwardedHeaders = ForwardedHeaders.XForwardedFor Or ForwardedHeaders.XForwardedProto
            })

        If Builder.Environment.IsDevelopment Then App.UseDeveloperExceptionPage

        App.UseRouting

        '--- from Identity project
        App.UseHttpsRedirection
        App.UseStaticFiles(New StaticFileOptions With {.FileProvider = New PhysicalFileProvider(Builder.Configuration("StaticFilesRoot"))})
        App.UseStaticFiles(New StaticFileOptions With {.FileProvider = New PhysicalFileProvider(Builder.Configuration("StaticFilesRoot")), .RequestPath = "/Identity"})
        App.UseAuthentication
        App.UseAuthorization
        App.MapControllerRoute(name:="areas", pattern:="{area:exists}/{controller=Account}/{action=Index}/{id?}")
        App.MapControllerRoute(name:="default", pattern:="{controller=Home}/{action=Index}/{id?}")

        App.MapRazorPages
        '----------------------

        'Route anylizer
        App.Use(Async Function(context As HttpContext, NextRequestDelegate As RequestDelegate)
                    Dim CurrentEndpoint = context.GetEndpoint()
                    If (CurrentEndpoint Is Nothing) Then
                        Debug.WriteLine($"RequestPath {context.Request.Path} endpoint nothing.")
                        Dim StaticOptions As StaticFileOptions = New StaticFileOptions With {.FileProvider = New PhysicalFileProvider(Builder.Configuration("StaticFilesRoot"))}
                        Dim Opt As IOptions(Of StaticFileOptions) = Options.Create(StaticOptions)
                        Dim NewEnvironment As IWebHostEnvironment = Environment
                        NewEnvironment.ContentRootPath = Builder.Configuration("StaticFilesRoot")
                        NewEnvironment.WebRootPath = Builder.Configuration("StaticFilesRoot")
                        Dim StaticMiddleware = New StaticFileMiddleware(
                            Async Function()
                                'Await NextRequestDelegate(context)
                                Dim StaticFile As IFileInfo = Opt.Value.FileProvider.GetFileInfo(context.Request.Path)
                                If Not StaticFile.Exists Then
                                    Await context.Response.WriteAsync("File is missing")
                                Else
                                    Await context.Response.SendFileAsync(StaticFile)
                                End If

                            End Function,
                            NewEnvironment,
                            Opt,
                            LoggerFactory)
                        Await StaticMiddleware.Invoke(context)
                    Else
                        Debug.WriteLine($"Endpoint: {CurrentEndpoint.DisplayName}")
                        Dim Endpoint As RouteEndpoint = TryCast(CurrentEndpoint, RouteEndpoint)
                        Debug.WriteLine($"RoutePattern: {Endpoint?.RoutePattern.RawText}")
                        For j As Integer = 0 To CurrentEndpoint.Metadata.Count - 1
                            Debug.WriteLine($"Endpoint Metadata {j}: {CurrentEndpoint.Metadata(j)}")
                        Next
                        Await NextRequestDelegate(context)
                    End If
                End Function)

        App.UseEndpoints(Function(x) x.MapControllers)

        App.Run()
    End Sub
End Module