//  �������������� � ������� ����

//  ����� �� ���������������� �������� �������������� � ���-���������� �������� �������������� � ������� ����. � ASP.NET Core
//  ����� ���������� ��������� ��� ������� ���� ��������������.

//  ��� �������� �������������� � ������� ���� � ����� AddAuthentication() ���������� ����� "Cookies":
//  builder.Services.AddAuthentication("Cookies")

//  ����� �� ��������� � ��������� ����� ��� ����� ���������� ��������� CookieAuthenticationDefaults.AuthenticationScheme,
//  ������� ����� �� �� ����� ��������.
//  builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)

//  ����� ����, ��� ��������� �������������� � ������ ���� ���������� ������� ����� AddCookie(), ������� ���������� ��� �����
//  ���������� ��� ���� AuthenticationBuilder:
//  public static AuthenticationBuilder AddCookie(this AuthenticationBuilder builder,
//                                              Action<CookieAuthenticationOptions> configureOptions);

//  � �������� ��������� ����� ��������� �������, ������� � ������� ������� CookieAuthenticationOptions �������������
//  ��������� ��������������.

//  ���������� �� �������, ��� ������������ ����� ���������� �������������� � ������� ����. ��� ����� ��������� � �����
//  Program.cs ��������� ���:

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder();

// �������� �� � ��������������
var people = new List<Person>
{
new Person("tom@gmail.com", "12345"),
new Person("bob@gmail.com", "55555")
};
// �������������� � ������� ����
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options => options.LoginPath = "/login");
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();�� // ���������� middleware �������������� 
app.UseAuthorization();   // ���������� middleware ����������� 

app.MapGet("/login", async (HttpContext context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
����// html-����� ��� ����� ������/������
����string loginForm = @"<!DOCTYPE html>
����<html>
����<head>
��������<meta charset='utf-8' />
��������<title>METANIT.COM</title>
����</head>
����<body>
��������<h2>Login Form</h2>
��������<form method='post'>
������������<p>
����������������<label>Email</label><br />
����������������<input name='email' />
������������</p>
������������<p>
����������������<label>Password</label><br />
����������������<input type='password' name='password' />
������������</p>
������������<input type='submit' value='Login' />
��������</form>
����</body>
����</html>";
    await context.Response.WriteAsync(loginForm);
});

app.MapPost("/login", async (string? returnUrl, HttpContext context) =>
{
����// �������� �� ����� email � ������
����var form = context.Request.Form;
����// ���� email �/��� ������ �� �����������, �������� ��������� ��� ������ 400
����if (!form.ContainsKey("email") || !form.ContainsKey("password"))
        return Results.BadRequest("Email �/��� ������ �� �����������");

    string email = form["email"];
    string password = form["password"];

    // ������� ������������ 
    Person? person = people.FirstOrDefault(p => p.Email == email && p.Password == password);
����// ���� ������������ �� ������, ���������� ��������� ��� 401
����if (person is null) return Results.Unauthorized();

    var claims = new List<Claim> { new Claim(ClaimTypes.Name, person.Email) };
����// ������� ������ ClaimsIdentity
����ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
����// ��������� ������������������ ����
����await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
    return Results.Redirect(returnUrl ?? "/");
});

app.MapGet("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/login");
});

app.Map("/", [Authorize] () => $"Hello World!");

app.Run();

record class Person(string Email, string Password);

//  ��� ������������� ������������ ����� Person:
//  record class Person(string Email, string Password);

//  � �������� �������� ���� ������ ��� ����� ������������ ������ people:
//  var people = new List<Person>
//  {
//    new Person("tom@gmail.com", "12345"),
//    new Person("bob@gmail.com", "55555")
//  };

//  ������ � ����� ������� �� ����� ���������� ���������� �� ������� ����� � ������.

//  ��� ����������� �������������� ���� ������������ ��������������� �������:
//  builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//      .AddCookie(options => options.LoginPath = "/login");

//  �������� LoginPath ������ CookieAuthenticationOptions ��������� �� ����, �� �������� ��������������������� ������ �����
//  ������������� ������������������ ��� ��������� � �������, ��� ������� � �������� ��������� ��������������.

//  �������� �����, ������� ������������ get-������� �� ���� "/login", ����� ����������� � ����� html-����� ��� ����� email � ������:

//app.MapGet("/login", async (HttpContext context) =>
//{
//    context.Response.ContentType = "text/html; charset=utf-8";
//    // html-����� ��� ����� ������/������
//    string loginForm = @"<!DOCTYPE html>
//    <html>
//    <head>
//        <meta charset='utf-8' />
//        <title>METANIT.COM</title>
//    </head>
//    <body>
//        <h2>Login Form</h2>
//        <form method='post'>
//            <p>
//                <label>Email</label><br />
//                <input name='email' />
//            </p>
//            <p>
//                <label>Password</label><br />
//                <input type='password' name='password' />
//            </p>
//            <input type='submit' value='Login' />
//        </form>
//    </body>
//    </html>";
//    await context.Response.WriteAsync(loginForm);
//});

//  � ������ ������ ��� �������� ���� html-��� ��������� � ���� ������, ��, ����������� ����� ������� ��-�������, ��������,
//  ���������� ��������� html-�������� � �� ���������� ���������� �������.

//  �� ������������ ����� ������ ������ ����� ��������� ���� "email" � "password", � ����� ������� �� ������ �������� ��������
//  ���� ����� � ������� ���� POST ����� �������� � ����������� ������ �������� �����:
//  app.MapPost("/login", async (string? returnUrl, HttpContext context) => 

//  ���������� ���� �������� ����� ��������� ��� ���������. ������ �����, ��� ������������ ����� �������� ��������� ������������
//  ������ ��������� ������ HttpContext. ����� ����, ������� �������������� ������������� ���������� ����, � �������� ������������
//  ��� ������������� �� ����� ������. ��������� � ��� ����� ����� ������ � ����� ��������� ������������ ������ ��������� � "/login",
//  �� ����� �������� returnUrl �� ����� �������� ����, �� �������� ���������� ��������� ������. ������ ��������� ������ �����
//  ����� �������� ���������� � ����� ������, �� � ���� ������ ������ �������� ����� ����� �������� null.

//  � ����� �� ����������� �������� ����� ������� �������� �� ������ ����� ������������ email � ������:

//var form = context.Request.Form;
//if (!form.ContainsKey("email") || !form.ContainsKey("password"))
//    return Results.BadRequest("Email �/��� ������ �� �����������");

//string email = form["email"];
//string password = form["password"];

//  ������� ������, ���������, � ���� �� ������ � ������ ������� � ����� �������� ���� ������ - ������ people:

//Person? person = people.FirstOrDefault(p => p.Email == email && p.Password == password);
//if (person is null) return Results.Unauthorized();

//  ����� ������������ ��������� ������������������ ���, ������� ����� ����������� ��� ����������� ������� � ��� ���� � ����������:

//var claims = new List<Claim> { new Claim(ClaimTypes.Name, person.Email) };
//// ������� ������ ClaimsIdentity
//ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
//// ��������� ������������������ ����
//await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

//��� ��������� ��� � ������ HttpContext ����������� ����������� ����� SignInAsync(). � �������� ��������� �� ���������
//����������� ����� ��������������, � ����� ������ ��� "Cookies", �� ���� �������� ���������
//CookieAuthenticationDefaults.AuthenticationScheme. � � �������� ������� ��������� ���������� ������ ClaimsPrincipal,
//������� ������������ ������������.

//��� ����������� �������� � ��������� ������� ClaimsPrincipal ������� ��������� ������ claims - ����� �������� Claim - �����
//������ ����� ������, ������� ��������� ������������. ��� ������ ��������� � ����������� � ������������������ ����. ������
//����� claim ��������� ��� � ��������. � ����� ������ � ��� ������ ���� claim, ������� � �������� ���� ��������� ���������
//ClaimTypes.Name, � � �������� �������� - email ������������.

//����� ��������� ������ ClaimsIdentity, ������� ����� ��� ������������� ClaimsPrincipal. � ClaimsIdentity ���������� �����
//��������� ������ claims � ��� ��������������, � ������ ������ "Cookies". ��� �������������� ����� ������������ ������������ ������.

//� ����� ������ ������ �ontext.SignInAsync ����� ������������� ������������������ ����, ������� ����� ���������� ������� �
//��� ����������� �������� ����� ������������ ������� �� ������, ����������������� � �������������� ��� �������������� ������������.

//� ����� ����� �������������� ���������������������� ������������ ������� �� �����, � �������� ��� ����������� �� ����� ������:
//  return Results.Redirect(returnUrl??"/");

//  ��� ������ �� ����� ���������� �������� �����, ������� ������������ ������� �� ���� "/logout":

//app.MapGet("/logout", async (HttpContext context) =>
//{
//    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
//    return Results.Redirect("/login");
//});

//  ��� ������ - ����� ������ context.SignOutAsync(), ������� ������� ������������������ ����. � �������� ��������� �� ���������
//  ����� ��������������.

//  ��� ������������ ����������� ���������� ��������� �������� �����, ������� ������������ ������� � ����� ����������:
//  app.Map("/", [Authorize]() => $"Hello World!");
//  ��������� ��� �������� ����� ���������� ������� Authorize, �� ������ � ��� ����� ������ ������������������� ������������.