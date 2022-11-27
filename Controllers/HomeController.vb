Imports Microsoft.AspNetCore.Mvc
Imports System.Diagnostics
Imports FrontEndData.Models
Imports Microsoft.Extensions.Logging
Imports Microsoft.AspNetCore.Http
Imports Microsoft.AspNetCore
Imports Microsoft.Extensions.Options
Imports Microsoft.AspNetCore.Mvc.ApplicationModels
Imports Newtonsoft.Json
Imports BackendAPI.FrontEndModels
Imports Microsoft.AspNetCore.Mvc.ViewEngines
Imports Microsoft.AspNetCore.Mvc.Infrastructure

Namespace Controllers
    Public Class HomeController
        Inherits Controller

        Private ReadOnly _logger As ILogger(Of HomeController)
        Private ReadOnly _Context As IHttpContextAccessor

        Private ReadOnly _RazorPagesOptions As RazorPages.RazorPagesOptions
        Private ReadOnly _RazorViewEngineOptions As Razor.RazorViewEngineOptions
        Private ReadOnly _MvcViewOptions As Mvc.MvcViewOptions
        Private ReadOnly _IdentityOptions As Identity.IdentityOptions
        Private ReadOnly _MvcOptions As Mvc.MvcOptions
        Private ReadOnly _CookieAuthenticationOptions As Authentication.Cookies.CookieAuthenticationOptions
        Private ReadOnly _AuthenticationOptions As Authentication.AuthenticationOptions
        Private ReadOnly _RouteOptions As Routing.RouteOptions
        Private ReadOnly _HostOptions As Microsoft.Extensions.Hosting.HostOptions
        Private ReadOnly _KestrelServerOptions As Server.Kestrel.Core.KestrelServerOptions
        Private ReadOnly _HostFilteringOptions As HostFiltering.HostFilteringOptions
        Private ReadOnly _ApiBehaviorOptions As Mvc.ApiBehaviorOptions
        Private ReadOnly _AntiforgeryOptions As Antiforgery.AntiforgeryOptions
        Private ReadOnly _Engine As ICompositeViewEngine
        Private ReadOnly _Action As IActionContextAccessor

        Public Sub New(ByVal logger As ILogger(Of HomeController), Context As IHttpContextAccessor, Engine As ICompositeViewEngine, Action As IActionContextAccessor,
                       RazorPagesOptions As IOptions(Of RazorPages.RazorPagesOptions),
                       RazorViewEngineOptions As IOptions(Of Razor.RazorViewEngineOptions),
                       MvcViewOptions As IOptions(Of Mvc.MvcViewOptions),
                       IdentityOptions As IOptions(Of Identity.IdentityOptions),
                       MvcOptions As IOptions(Of Mvc.MvcOptions),
                       CookieAuthenticationOptions As IOptions(Of Authentication.Cookies.CookieAuthenticationOptions),
                       AuthenticationOptions As IOptions(Of Authentication.AuthenticationOptions),
                       RouteOptions As IOptions(Of Routing.RouteOptions),
                       HostOptions As IOptions(Of Microsoft.Extensions.Hosting.HostOptions),
                       KestrelServerOptions As IOptions(Of Server.Kestrel.Core.KestrelServerOptions),
                       HostFilteringOptions As IOptions(Of HostFiltering.HostFilteringOptions),
                       ApiBehaviorOptions As IOptions(Of Mvc.ApiBehaviorOptions),
                       AntiforgeryOptions As IOptions(Of Antiforgery.AntiforgeryOptions))

            _logger = logger
            _Context = Context

            _RazorPagesOptions = RazorPagesOptions.Value
            _RazorViewEngineOptions = RazorViewEngineOptions.Value
            _MvcViewOptions = MvcViewOptions.Value
            _IdentityOptions = IdentityOptions.Value
            _MvcOptions = MvcOptions.Value
            _CookieAuthenticationOptions = CookieAuthenticationOptions.Value
            _AuthenticationOptions = AuthenticationOptions.Value
            _RouteOptions = RouteOptions.Value
            _HostOptions = HostOptions.Value
            _KestrelServerOptions = KestrelServerOptions.Value
            _HostFilteringOptions = HostFilteringOptions.Value
            _ApiBehaviorOptions = ApiBehaviorOptions.Value
            _AntiforgeryOptions = AntiforgeryOptions.Value
            _Engine = Engine
            _Action = Action
            '
            'Debug.Print($"RazorViewEngineOptions : {JsonConvert.SerializeObject(_RazorViewEngineOptions, formatting:=Formatting.Indented)}")
            'For Mvc locations, the {0} Is a placeholder for the action name, where the {1} Is a placeholder for the controller name, And finally any reference To {2} denotes the area's name.
            'For Razor Pages locations, the {0} placeholder denotes a view's name, where the {1} denotes a page’s name, and {2} is again the area’s name.

            'Debug.Print($"IdentityOptions : {JsonConvert.SerializeObject(_IdentityOptions, formatting:=Formatting.Indented)}")
            'Debug.Print($"AuthenticationOptions : {JsonConvert.SerializeObject(_AuthenticationOptions, formatting:=Formatting.Indented)}")
            'Debug.Print($"RouteOptions : {JsonConvert.SerializeObject(_RouteOptions, formatting:=Formatting.Indented)}")
            'Debug.Print($"HostOptions : {JsonConvert.SerializeObject(_HostOptions, formatting:=Formatting.Indented)}")
            'Debug.Print($"HostFilteringOptions : {JsonConvert.SerializeObject(_HostFilteringOptions, formatting:=Formatting.Indented)}")
            'Debug.Print($"AntiforgeryOptions : {JsonConvert.SerializeObject(_AntiforgeryOptions, formatting:=Formatting.Indented)}")
            ''Debug.Print($"RazorPagesOptions : {JsonConvert.SerializeObject(_RazorPagesOptions)}") 'No public member
            ''Debug.Print($"MvcViewOptions : {JsonConvert.SerializeObject(_MvcViewOptions)}") 'No public member
            ''Debug.Print($"MvcOptions : {JsonConvert.SerializeObject(_MvcOptions)}") 'No public member
            ''Debug.Print($"ApiBehaviorOptions : {JsonConvert.SerializeObject(_ApiBehaviorOptions)}") 'No public member
            ''Debug.Print($"CookieAuthenticationOptions : {JsonConvert.SerializeObject(_CookieAuthenticationOptions)}")  'Loop
            ''Debug.Print($"KestrelServerOptions : {JsonConvert.SerializeObject(_KestrelServerOptions})}")   'Loop

        End Sub

        Public Function Index() As IActionResult
            ViewBag.Title = "Welcome"
            ViewBag.Details = "Please login or create account for more details."
            Return View()
        End Function

        Public Function Privacy() As IActionResult
            Return View()
        End Function

        <ResponseCache(Duration:=0, Location:=ResponseCacheLocation.None, NoStore:=True)>
        Public Function [Error]() As IActionResult
            Return View(New ErrorViewModel With {
                .RequestId = If(Activity.Current?.Id, HttpContext.TraceIdentifier)
            })
        End Function
    End Class
End Namespace

