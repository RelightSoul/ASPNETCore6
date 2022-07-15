//  �������������� � ������� JWT-�������

//  ����� �� �������� � ����������� � �������������� � ASP.NET Core ������������ �������� �������������� � ����������� � �������
//  JWT-�������. ��� ����� JWT-�����? JWT (��� JSON Web Token) ������������ ����� ���-��������, ������� ���������� ������ ��������
//  ������ � ������������ � ������� JSON � ������������� ����.

//  JWT-����� ������� �� ���� ������:
//  Header - ������ JSON, ������� �������� ���������� � ���� ������ � ��������� ��� ����������
//  Payload - ������ JSON, ������� �������� ������, ������ ��� ����������� ������������
//  Signature - ������, ������� ��������� � ������� ���������� ����, Headera � Payload. ��� ������ ������ ��� ����������� ������

//  ��� ������������� JWT-������� � ������ ASP.NET Core ���������� �������� Nuget-����� Microsoft.AspNetCore.Authentication.JwtBearer.

//  ������� ���������� ������� ��������� � �������� jwt-������. ��� ����� � ����� Program.cs ��������� ��������� ��� ����������:

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => 
    {
        options.TokenValidationParameters = new TokenValidationParameters 
        {
            // ���������, ����� �� �������������� �������� ��� ��������� ������
            ValidateIssuer = true,
            // ������, �������������� ��������
            ValidIssuer = AuthOptions.ISSUER,
            // ����� �� �������������� ����������� ������
            ValidateAudience = true,
            // ��������� ����������� ������
            ValidAudience = AuthOptions.AUDIENCE,
            // ����� �� �������������� ����� �������������
            ValidateLifetime = true,
            // ��������� ����� ������������
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            // ��������� ����� ������������
            ValidateIssuerSigningKey = true,
        };
    });

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.Map("/login/{username}", (string username) => 
{
    var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
    // ������� JWT-�����
    JwtSecurityToken jwt = new JwtSecurityToken(
        issuer: AuthOptions.ISSUER,
        audience: AuthOptions.AUDIENCE,
        claims: claims,
        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
    return new JwtSecurityTokenHandler().WriteToken(jwt);
});

app.Map("/data", [Authorize] () => new { message = "Hello World!" });

app.Run();

//  ��� �������� ��������� �������� ��������� ������ � ����� ���� ��������� ����������� ����� AuthOptions:

//public class AuthOptions
//{
//    public const string ISSUER = "MyAuthServer"; // �������� ������
//    public const string AUDIENCE = "MyAuthClient"; // ����������� ������
//    const string KEY = "mysupersecret_secretkey!123";   // ���� ��� ��������
//    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
//        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
//}

//  ��������� ISSUER ������������ �������� ������. ����� ����� ���������� ����� ��������.

//  ��������� AUDIENCE ������������ ����������� ������ - ����� �� ����� ���� ����� ������, ������ ��� ����, �� ������� ����������� �����.

//  ��������� KEY ������ ����, ������� ����� ����������� ��� �������� ������.

//  � ����� GetSymmetricSecurityKey() ���������� ���� ������������, ������� ����������� ��� ��������� ������. ��� ��������� ������ ���
//  ��������� ������ ������ SecurityKey. � �������� ������ ����� ��������� ������ ������������ ������ SymmetricSecurityKey, � �����������
//  �������� ���������� ������ ����, ��������� �� ���������� �����.

//  ����� �������, ��� ���������� ��� �������������� ����� ������������ ������, � ����� AddAuthentication() ���������� �������� ���������
//  JwtBearerDefaults.AuthenticationScheme.
//      builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

#region ������������ � ��������� ������
//  � ������� ������ AddJwtBearer() � ���������� ����������� ������������ ������. ��� ������������ ������ ����������� ������
//  JwtBearerOptions, ������� ��������� � ������� ������� ��������� ������ � �������. ������ ������ ����� ��������� �������. ����� ��
//  ������������ ������ �������� TokenValidationParameters, ������� ������ ��������� ��������� ������.

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidIssuer = AuthOptions.ISSUER,
//            ValidateAudience = true,
//            ValidAudience = AuthOptions.AUDIENCE,
//            ValidateLifetime = true,
//            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
//            ValidateIssuerSigningKey = true,
//        };
//    });

//  ������ TokenValidationParameters �������� ���������� �������, ������� ��������� ��������� ��������� ������� ��������� ������.
//  � ������ ������ ����������� ��������� ��������:
//      ValidateIssuer: ���������, ����� �� �������������� �������� ��� ��������� ������
//      ValidIssuer: ������, ������� ������������ �������� ������
//      ValidateAudience: ���������, ����� �� �������������� ����������� ������
//      ValidAudience: ������, ������� ������������ ����������� ������
//      ValidateLifetime: ���������, ����� �� �������������� ����� �������������
//      IssuerSigningKey: ������������ ���� ������������ - ������ SecurityKey, ������� ����� ����������� ��� ��������� ������
//      ValidateIssuerSigningKey: ���������, ����� �� �������������� ���� ������������

//  ����� ��������������� �������� �������� ��������. � ������ ����� ���������� ���� ������ ����������, ��������, �������� claims ���
//  ����� � ������� ������������ � �.�.
#endregion

#region ��������� ������
//  ����� ������������ ��� ������������ �����, ���������� ������ ��������� ��� ���� �����, � ����� ���� �������������� �������������
//  �����. � ��� ��������� ������ ����� ������������� ������� �������� ����� "/login":

//app.Map("/login/{username}", (string username) =>
//{
//    var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
//    var jwt = new JwtSecurityToken(
//            issuer: AuthOptions.ISSUER,
//            audience: AuthOptions.AUDIENCE,
//            claims: claims,
//            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)), // ����� �������� 2 ������
//            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

//    return new JwtSecurityTokenHandler().WriteToken(jwt);
//});

//  ��� �������� �������� ����� ����� �������� �������� "username" �������� ��������� ����� ������������ � ��������� ��� ��� ���������
//  ������. �� ������ ����� ��� �������� �� ���� ������ �� ���������, �������� �� ��� �����, ��� ��� �� �����, ���� ������ �������, ���
//  ������������ �����.

//  ��� �������� ������ ����������� ����������� JwtSecurityToken. ����� �� ���������� ������ ������ �������� Claim. ������� Claim ������
//  ��� �������� ��������� ������ � ������������, ��������� ������������. ����� ��� ������ ����� ��������� ��� ��������������. � ������
//  ������ ��������� � ������ ���� Claim, ������� ������ ����� ������������.

//  ����� ���������� ������� JWT-�����, ��������� � ����������� JwtSecurityToken ��������������� ���������. �������� ��������, ��� ���
//  ������������� ������ ����������� ��� �� �� ��������� � ���� ������������, ������� ���������� � ������ AuthOptions � �������
//  �������������� ��� ������������ �������� � ������ AddJwtBearer().

//  � ����� ����������� ������ JwtSecurityTokenHandler().WriteToken(jwt) ��������� ��� ����� , ������� ������������ �������.

//  ��� ������������ ��������� ������ ��������� � ���� �������� ����� localhost:xxxx/login/name.
//  ��� ��������� � �������� ����� "/login" (��������, �� ���� "/login/name", ��� "name" ����������� �������� "username") ����������
//  ����������� ��� jwt-�����, ������� ��� ���������� ���������� ��� ������� � �������� ���������� � ��������� ��������.
//  ��������, � ���� ����� ���������� ��� ���� �������� ����� "/data":
//      app.Map("/data", [Authorize] (HttpContext context) => $"Hello World!");

//  ��� ��������� ������� Authorize, �������������� ������ � ��� ��������� ������ ��� ������������������� �������������, ������� �����
//  �����. ��������, ���� �� ���������� ���������� �� ���� "/data", �� ���������� � ������� 401 (Unauthorized) - ������ �� �����������:

//  ������� ��� ��������� � ����� ������� (� �� ���� ������ ��������, � ������� ����� ������ ������ ������������������� ������������)
//  ���������� �������� ���������� ����� � ������� � ��������� Authorization:
//      "Authorization": "Bearer " + token  // token - ���������� ����� jwt-�����
//  � ��������� ����� ����������, ��� ��������� ����� ��� ������� � ��������.
#endregion

#region ����� ����
public class AuthOptions
{
    public const string ISSUER = "MyAuthServer"; // �������� ������
    public const string AUDIENCE = "MyAuthClient"; // ����������� ������
    const string KEY = "mysupersecret_secretkey!123";   // ���� ��� ��������
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}
#endregion