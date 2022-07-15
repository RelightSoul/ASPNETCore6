//  �������������� � �����������
//  �������� � �������������� � �����������

//  ������ ����� � ���������� �������� �������������� � �����������. �������������� ������������ ������� ����������� ������������.
//  ����������� ������������ ������� �����������, ����� �� ������������ ����� ������� � ���������� �������. �� ����, ���� ��������������
//  �������� �� ������ "��� �������� ������������?", �� ����������� �������� �� ������ "����� ����� ������������ ����� � �������?"
//  ASP.NET Core ����� ���������� ��������� �������������� � �����������.

#region ��������������
//  ��� ���������� �������������� � ��������� ��������� ������� �������� ����������� ��������� middleware - AuthenticationMiddleware.
//  ��� ����������� ����� middleware � �������� ����������� ����� ���������� UseAuthentication()
//        public static IApplicationBuilder UseAuthentication(this IApplicationBuilder app);

//  ������� ��������, ��� ����� UseAuthentication() ������ ������������ � �������� �� ����� ����������� middleware, ������� ����������
//  �������������� �������������.

//  ��� ���������� �������������� ���� ��������� ���������� ������� ��������������, � ���������, ������ IAuthenticationService, �������
//  �������������� � ���������� � ������� ������ AddAuthentication():
//  public static AuthenticationBuilder AddAuthentication(this IServiceCollection services)
//  public static AuthenticationBuilder AddAuthentication(this IServiceCollection services, string defaultScheme)
//  public static AuthenticationBuilder AddAuthentication(this IServiceCollection services, Action<AuthenticationOptions> configureOptions)

//  � �������� ��������� ������ ������ ������ AddAuthentication() ��������� ����� �������������� � ���� ������. ������ ������ ������
//  AddAuthentication ��������� �������, ������� ������������� ����� �������������� - ������ AuthenticationOptions.

//  ����� �� �� ������ ������ �� ������������, ��� �������������� ���������� ���������� ����� ��������������. ��� ��������
//  ���c������������� ����� ��������������:

//  "Cookies": �������������� �� ������ ����. �������� � ��������� CookieAuthenticationDefaults.AuthenticationScheme

//  "Bearer": �������������� �� ������ jwt-�������. �������� � ��������� JwtBearerDefaults.AuthenticationScheme

//  ����� �������������� ��������� �������� ������������ ���������� ��������������. ���������� �������������� ���������� � ���������
//  ���������������� �������������� ������������� �� ������ ������ �������� � ������ �� ����� ��������������.

//  ��������, ��� �������������� � ������� ���� ���������� ����� "Cookies". �������������� ��� �������������� ������������ �����
//  ���������� ���������� ���������� �������������� - ����� Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationHandler,
//  ������� �� ������ ���������� � ������� cookie ��������� ��������������.

//  � ���� ������������ ����� "Bearer", �� ��� ������, ��� ��� �������������� ����� �������������� jwt-�����, � � �������� �����������
//  �������������� ����� ����������� ����� Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerHandler. ����� ��������, ��� ���
//  �������������� � ������� jwt-������� ���������� �������� � ������ ����� Nuget ����� Microsoft.AspNetCore.Authentication.JwtBearer

//  ��� ��� � ASP.NET Core �� �� ���������� ����������� ������� �������������� � ����� ��������� ���� ��������� ����� � ��� ��� �����
//  ������������ ��������������.

//  ����� ���������� ����� �������������� ���������� ���������� �������������� ������������� ����. ��� ����� ����� ������������ ���������
//  ������:

//  AddCookie(): ���������� � ������������� �������������� � ������� ����.

//  AddJwtBearer(): ���������� � ������������� �������������� � ������� jwt-������� (��� ����� ������ ��������� Nuget-�����
//  Microsoft.AspNetCore.Authentication.JwtBearer)

//  ��� ������ ����������� ��� ������ ���������� ��� ���� AuthenticationBuilder, ������� ������������ ������� AddAuthentication():

//var builder = WebApplication.CreateBuilder();
//// ���������� �������� ��������������
//builder.Services.AddAuthentication("Bearer")  // ����� �������������� - � ������� jwt-�������
//    .AddJwtBearer();  // ����������� �������������� � ������� jwt-�������

//var app = builder.Build();

//app.UseAuthentication();    // ���������� middleware ��������������
#endregion

#region �����������
//  ����������� ������������ ������� ����������� ���� ������������ � �������, � ����� �������� ���������� �� ����� ����� ������� �
//  ��� ����� ��������.

//  ���� ����������� ������������ ��������� ����������� �������, ��� �� ����� ��� ��� ����� ����������, ����� ���������� �����
//  ��������� ��������������.

//  ��� ����������� ����������� ���������� �������� ��������� Microsoft.AspNetCore.Authorization.AuthorizationMiddleware. ��� �����
//  ����������� ���������� ����� ���������� UseAuthorization()
//      public static IApplicationBuilder UseAuthorization(this IApplicationBuilder app)

//  ����� ����, ��� ���������� ����������� ���������� ���������������� ������� ����������� � ������� ������ AddAuthorization():
//      public static IServiceCollection AddAuthorization(this IServiceCollection services)
//      public static IServiceCollection AddAuthorization(this IServiceCollection services, Action<AuthorizationOptions> configure)

//  ������ ������ ������ ��������� �������, ������� � ������� ��������� AuthorizationOptions ��������� ���������������� �����������.

//  �������� ��������� ��������� ����������� � ASP.NET Core �������� ������� AuthorizeAttribute �� ������������ ����
//  Microsoft.AspNetCore.Authorization, ������� ��������� ���������� ������ � �������� ����������. ��������:

using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer();
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.Map("/hello", [Authorize]() => "Hello man!");
app.Map("/", () => "Home Page");

app.Run();

//  ����� � ���������� ���������� ��� �������� �����: "/" � "/hello".��� ���� �������� ����� "/hello" ��������� ������� Authorize.
//  ������� ����������� ����� ������������ �������� �����.

//  ���������� ������� �������� ��������, ��� � �������� ����� "/hello" ����� ������ ������ ������������������� ������������.
#endregion