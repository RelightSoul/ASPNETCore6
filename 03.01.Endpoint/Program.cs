//  �������������
//  �������� �����. ����� Map

//  ������� ������������� �������� �� ������������� �������� �������� � ���������� � �� ��������� ����������� ������������� �������� ���
//  ��������� ������� ������������ �������� ����� ����������. �������� ����� ��� endpoint ������������ ��������� ���, ������� ������������
//  ������. �� ���� �������� ����� ���������� ������ ��������, �������� ������ ��������������� ������, � ���������� ������� �� �����
//  ��������.

//  ASP.NET Core �� ��������� ������������� ������� � ������� ���������� ��� �������� �������� �����. �������� ����� � ���� �����������
//  �������� ��������� Microsoft.AspNetCore.Routing.IEndpointRouteBuilder. �� ���������� ��� ������� ��� ���������� �������� ����� �
//  ����������. � ��������� ����� WebApplication ����� ��������� ������ ���������, �� �������������� ��� ������ ���������� �� �����
//  �������� � � ������� WebApplication.

//  ��� ������������� ������� ������������� � �������� ��������� ������� ����������� ��� ���������� ���������� middleware:

//  Microsoft.AspNetCore.Routing.EndpointMiddleware ��������� � �������� ��������� ������� �������� �����. ����������� � �������� �
//  ������� ������ UseEndpoints()

//  Microsoft.AspNetCore.Routing.EndpointRoutingMiddleware ��������� � �������� ��������� ������� ���������������� ������������� ��������
//  � ���������. ������ middleware �������� �������� �����, ������� ������������� ������� � ������� ����� ������������ ������. �����������
//  � �������� � ������� ������ UseRouting()

//  ������ ������ �� ��������� ����� ������� ���������� ��� ��� ���������� middleware. ������ WebApplicationBuilder �������������
//  �������������� �������� ����� �������, ��� ��� ��� middleware ����������� ��� ������������� �������� �����.

#region ����� Map
//  ����� ������� �������� ����������� �������� ����� � ���������� �������� ����� Map, ������� ���������� ��� ����� ���������� ��� ����
//  IEndpointRouteBuilder. �� ��������� �������� ����� ��� ��������� �������� ���� GET. ������ ����� ����� ��� ������:
//  public static RouteHandlerBuilder Map(this IEndpointRouteBuilder endpoints, RoutePattern pattern, Delegate handler);
//  public static IEndpointConventionBuilder Map(this IEndpointRouteBuilder endpoints, string pattern, RequestDelegate requestDelegate);
//  public static RouteHandlerBuilder Map(this IEndpointRouteBuilder endpoints, string pattern, Delegate handler);

//  � ���� ���� ����������� ���� ����� � �������� ��������� pattern ��������� ������ ��������, �������� ������ ��������������� ������.
//  ������ �������� ����� ������������ ��� RoutePattern ��� string.

//  ��������� �������� ������������ ��������, ������� ����� ������������ ������. ��� ����� ���� ������� ���� RequestDelegate, ���� �������
//  Delegate.

//  ����� ��������, ��� �� ����� ������ ���� ����� � ����������� ������� Map(), ������� ���������� ��� ����� ���������� ��� ����
//  IApplicationBuilder

//app.Map("/time", appBuilder =>                            // �� ������ � ���� ������� ��� IApplicationBuilder
//{
//    var time = DateTime.Now.ToShortTimeString();

//    // ��������� ������ - ������� �� ������� ����������
//    appBuilder.Use(async (context, next) =>
//    {
//        Console.WriteLine($"Time: {time}");
//        await next();  // �������� ��������� middleware
//    });

//    appBuilder.Run(async context =>
//    {
//        await context.Response.WriteAsync($"Time: {time}");
//    });
//});

//  ��������, ��������� ��������� ����������:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/",() => "Index Page");
//app.Map("/about", () => "About Page");
//app.Map("/contact", () => "Contacts Page");

//app.Run();

//  ����� ���������� ��� �������� ����� � ������� ���� ������� app.Map(). ������ ����� ��������� �������� �����, ������� ����� ������������
//  ������ �� ���� "/". � �������� ����������� ��������� ��������
//      () => "Index Page"

//  ��������� ����� �������� - ������ "Index Page" - ��� ��, ��� ����� ������������ � ����� �������.

//  ���������� ������ � ������ ������ ������ Map ��������� �������� ����� ��� ��������� �������� �� ����� "/about" � "/contact":

//  ���� � ������� ���������� ��������� ���������� ������, �� � �������� ��� ����� ���� ����� ��������, ��������:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/", () => "Index Page");
//app.Map("/about", () => "About Page");
//app.Map("/contact", () => "Contacts Page");
//app.Map("/user", () => new Person ("Tom", 33));

//app.Run();

//  ����� ��������� ������� ������ �������� ����� ���������� � ����� ������ Person. �� ��������� �������� ������� ��� ��������
//  ��������������� � JSON

//  � �������� ����� ������ �� ����������� �� ����������, ����� ��������� ��������� ��������
//      app.Map("/user", ()=>Console.WriteLine("Request Path: /user"));
//  � ������ ������ � ����������� ������ �������� ����� ������ ��������� �� ������� ��������� ����������.

//  ��� ������������� ���������� �������� ����� ������� � ����������� �����:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/", () => "Index Page");
//app.Map("/user", () => GetPerson);
//app.Map("/contact", async context =>   // ��������� �������
//{
//    await context.Response.WriteAsync("Contacts Page");
//});

//app.Run();

//Person GetPerson()
//{
//    return new Person("Tom", 33);
//}

//  � �������� ���� ���������� ������� ����������� ������� Delegate. ���� �� ���������� �������� ������ ������ � ��������� HttpContext,
//  �� ����� ������������ ������ ������ ������, ������� ��������� ������� RequestDelegate:

//  app.Map("/about", async (context) =>
//  {
//        await context.Response.WriteAsync("About Page");
//  });
#endregion

#region ��������� ���� ��������� ����������
//  ASP.NET Core ��������� ����� �������� ��� ��������� � ���������� �������� �����. ���, ��������� ��������� ���:

using System.Text;

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Map("/",() => "Index Page");
app.Map("/user",() => "User Page");
app.Map("/about", () => "About Page");

app.MapGet("/routes", (IEnumerable<EndpointDataSource> endpointSources) =>
{
    string.Join("\n", endpointSources.SelectMany(source => source.Endpoints));
});

app.Run();

//  ����� ���������� ������ �������� �����. ��� ������ �������� ����� �����������. ������� ���������� ��������� �������� �����, �������
//  ������������ ������� �� �������� "/routes" � ������� � ����� �������� ������ ���� �������� �����.

//  ����� �������� ��������� ������������ � ���������� �������� ��������� �������� ����� ���������� ������ IEnumerable<EndpointDataSource>
//  - ��������� ����� ������ � �������� ������. ������ ��������� ������� ����� ������ - ������ EndpointDataSource, ������� ������ �����
//  �������� ����� � ��������-������ Endpoints. ������ �������� ����� � ���� ������ ������������ ����� Endpoint

//  � ������� ������ endpointSources.SelectMany() �������� �� ��������� Endpoints ��� �������� �����. � ������� ������ Join() ���
//  ����������� � ���� ������ � ����������� ��������� ������ \n.

//  � ����� �� ������ � �������� ������ �� ������� �������� �����

//  ��� ������������� ����� �������� ����� ��������� � ��������� ���������� �� ������ �������� �����

//app.MapGet("/routes", (IEnumerable<EndpointDataSource> endpointSources) =>
//{
//    var sb = new StringBuilder();
//    var endpoints = endpointSources.SelectMany(es => es.Endpoints);
//    foreach (var endpoint in endpoints)
//    {
//        sb.AppendLine(endpoint.DisplayName);

//        // ������� �������� ����� ��� RouteEndpoint
//        if (endpoint is RouteEndpoint routeEndpoint)
//        {
//            sb.AppendLine(routeEndpoint.RoutePattern.RawText);
//        }

//        // ��������� ����������
//        // ������ �������������
//        // var routeNameMetadata = endpoint.Metadata.OfType<Microsoft.AspNetCore.Routing.RouteNameMetadata>().FirstOrDefault();
//        // var routeName = routeNameMetadata?.RouteName;
//        // ������ http - �������������� ���� ��������
//        //var httpMethodsMetadata = endpoint.Metadata.OfType<HttpMethodMetadata>().FirstOrDefault();
//        //var httpMethods = httpMethodsMetadata?.HttpMethods; // [GET, POST, ...]
//    }
//    return sb.ToString();
//});
#endregion

#region ����� ����
record class Person(string Name, int Age);
#endregion