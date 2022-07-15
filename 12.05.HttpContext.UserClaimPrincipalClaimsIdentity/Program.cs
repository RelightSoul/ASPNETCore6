//  HttpContext.User, ClaimPrincipal � ClaimsIdentity

#region ������ ClaimPrincipal � ClaimsIdentity � �� ���� � ��������������
//  ����� �� ����� �������������� � ���������� ASP.NET Core �������� ��������� ������������, ������� ����������� � ����������
//  ��������� User ������ HttpContext:
//      public abstract System.Security.Claims.ClaimsPrincipal User { get; set; }

//  ������ �������� ������������� ����� ClaimsPrincipal �� ������������ ���� System.Security.Claims

//  ��������������� ������, ������� �������������� ������������ (��� ������������) �������� � �������� Identity ������ ClaimPrincipal:
//      public virtual System.Security.Principal.IIdentity? Identity { get; }

//  ��� �������� ������������ �������� ������������ �������� ������������. �� ��������� � ����� ������������� ����� ���� ������ �����
//  �������������, �� ����� � ������ ���������� �������� Identities:
//      public virtual IEnumerable<ClaimsIdentity> Identities { get; }

//  �������� Identity ������������ ��������� IIdentity, �, ��� �������, � �������� ����� ���������� ����������� ����� ClaimsIdentity.

//  ������ IIdentity, � ���� �������, ������������� ���������� � ������� ������������ ����� ��������� ��������:
//  AuthenticationType: ��� �������������� � ��������� ����
//  IsAuthenticated: ���������� true, ���� ������������ ����������������
//  Name: ���������� ��� ������������. ������ � �������� ��������� ����� ������������ �����, �� �������� ������������ ������ � ����������

//  ��� �������� ������� ClaimsIdentity ����� ��������� ��� �������������, ��, ��� ����, ����� ������������ ��� ����������������,
//  ����������, ��� �������, ������������ ��� ��������������, ������� ���������� ����� �����������. ��� �������������� ������������
//  ������������ ������, ������� ��������� ��������� ������� ������ ��������������. ��������:
//      var identity = new ClaimsIdentity("Cookies");

//  � ������ ������ � ��� �������������� ���������� "Cookies".

//  ��� ��������� ������������ ������������ ������ ClaimsIdentity ����� �������� � ClaimsPrincipal ���� ����� �����������, ����
//  ����� ����� AddIdetity():

//      var identity = new ClaimsIdentity("Undefined");
//      var principal = new ClaimsPrincipal(identity);

//  �� ������� �������������� ���� ��������� �� ���������� ClaimsPrincipal � ClaimsIdentity:

using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder();

// �������������� � ������� ����
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

var app = builder.Build();

app.UseAuthentication();

app.MapGet("/login", async (HttpContext context) =>
{
    var claimsIdentity = new ClaimsIdentity("Undefined");
    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
    // ��������� ������������������ ����
    await context.SignInAsync(claimsPrincipal);
    return Results.Redirect("/");
});

app.MapGet("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return "������ �������";
});
app.Map("/", (HttpContext context) =>
{
    var user = context.User.Identity;
    if (user is not null && user.IsAuthenticated)
    {
        return $"������������ ����������������. ��� ��������������: {user.AuthenticationType}";
    }
    else
    {
        return "������������ �� ����������������";
    }
});

app.Run();

//  ����� � �������� ����� app.MapGet("/login") ��������� ������������ claimsIdentity - ������ ClaimsIdentity � ����� ��������������
//  "Undefined". ����� ��������� ������ ClaimsPrincipal, ������� ��������� ������������ claimsIdentity. ��������� ������ claimsPrincipal
//  ����� ���������� � ����� context.SignInAsync(), �������, ��������� ���� ������, ������������� ������������������ ����. � � �����
//  ���������� �������� �� ���� "/".

//  �������� ����� app.Map("/") �������� ����� �������� ��������� ������������ �������� ������������ ����� �������� context.User.
//  ���������� ��� ��� ����� ������ ClaimsPrincipal, ��������� ���� � ����������� � �����. � ����� �������� ������ � ����������,
//  �������������� ASP.NET Core ��������� � ������������� ������ ������� � ������� �� ��� ������ ClaimsPrincipal, ������� ��������
//  � �������� context.User. ���� ������������ �������������� �� ������ ���� (��� � ������� ����), �� ������ � ������������ �����
//  ����������� �� ������������������ ���. ���� ����������� jwt-������, �� ������ ������� �� ����������� ������. ������ ���� ����
//  ������������������ ���� ��� ������ � ������� ���, �� ������ ClaimsPrincipal ��� ����� ����� �����������.

//  ������� ������������ ������������, �� ����� �������� ��������� ���������� � ���. ��������, ���������, �������������� �� ��,
//  �������� ��� ��������������, �������� ������ ��������� � ��� ����������.

//  ����� �������, ��� ������ ��������� � ����������, ����� � ��� �� ����������� ������� ������������������ ���, ������������
//  �� context.User �� ����������������. �� ����� �������� �� ���� "/login" ����� ������� ������� ClaimsPrincipal � ClaimsIdentity
//  � �� ��� ����� ����������� ������������������ ����. �������������� ��� ��������� �������� �� ���� "/" ������������ �����
//  ����������������
#endregion

#region ��������� ClaimsPrincipal
//  ��������� ������ HttpContext �������� ����� �������� ��������� ������������ � ����� ����� ����������, �� �� ����� ����� ���� ������
//  �������� ������������, ��� � ������� ����. ������, ���� ��� ����� ������ �������� User, � �� ���� ������ HttpContext, �� �� �����
//  ����� ����� �������� ��������� ������������ �������� ������ ClaimsPrincipal, ������� ����� ���������� �������� context.User

//app.Map("/", (ClaimsPrincipal claimsPrincipal) =>
//{
//    var user = claimsPrincipal.Identity;
//    if (user is not null && user.IsAuthenticated)
//        return "������������ ����������������";
//    else return "������������ �� ����������������";
//});
#endregion