//  ����������� � ������� JWT-������� � ������� JavaScript

//  � ������� ������ ��� ���������� ������� ������������ � ��������� JWT-�������. ������ ���������, ��� �� ����� ���������
//  JWT-����� ��� ����������� � ����������. ��� ����� ��������� � ����� Program.cs ��������� ���:

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// �������� �� � ��������������
var people = new List<Person>
 {
    new Person("tom@gmail.com", "12345"),
    new Person("bob@gmail.com", "55555")
};

var builder = WebApplication.CreateBuilder();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.ISSUER,
            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,
            ValidateLifetime = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true
        };
    });
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/login", (Person loginData) =>
{
    // ������� ������������ 
    Person? person = people.FirstOrDefault(p => p.Email == loginData.Email && p.Password == loginData.Password);
    // ���� ������������ �� ������, ���������� ��������� ��� 401
    if (person is null) return Results.Unauthorized();

    var claims = new List<Claim> { new Claim(ClaimTypes.Name, person.Email) };
    // ������� JWT-�����
    var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

    // ��������� �����
    var response = new
    {
        access_token = encodedJwt,
        username = person.Email
    };

    return Results.Json(response);
});
app.Map("/data", [Authorize] () => new { message = "Hello World!" });

app.Run();

public class AuthOptions
{
    public const string ISSUER = "MyAuthServer"; // �������� ������
    public const string AUDIENCE = "MyAuthClient"; // ����������� ������
    const string KEY = "mysupersecret_secretkey!123";   // ���� ��� ��������
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}

record class Person(string Email, string Password);

//  ��� ������������ ������������ � ���������� ����� ��������� record-����� Person, ������� ����� ��� ��������: email � ������.
//  � ��� ��������� �������� ������ ���� ������ ��� ������������ ���������� �������� � ������ people. ������� ������ � ��� ���� ���
//  ������������.

//  ��� �������� ��������� �������� ��������� ������, ��� � � ������� ����, � ���� ��������� ����������� ����� AuthOptions, � �����,
//  ��� � � ������� ����, � ������� ������ AddJwtBearer() � ���������� ����������� ������������ ������.

//  � �������� ����� "\login", ������� ������������ POST-�������, �������� ������������ �������� ������������������ ������ ����� ��
//  ��� �������� � ���� ������� Person:
//  app.MapPost("/login", (Person loginData) =>

//  ��������� ���������� ������, �������� ����� � ������ people ������������:
//  Person ? person = people.FirstOrDefault(p => p.Email == loginData.Email && p.Password == loginData.Password);

//  ���� ������������ �� ������, �� ���� �������� ������������ email �/��� ������, �� ��������� ��������� ��� 401, ������� �������
//  � ���, ��� ������ ��������:
//  if (person is null) return Results.Unauthorized();

//  ���� ������������ ������, �� ��������� ������ �������� Claim � ����� Claim, ������� ������������ email ������������.
//  ���������� jwt-�����:
//  var claims = new List<Claim> { new Claim(ClaimTypes.Name, person.Email) };
//  var jwt = new JwtSecurityToken(
//      issuer: AuthOptions.ISSUER,
//      audience: AuthOptions.AUDIENCE,
//      claims: claims,
//      expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),  // �������� ������ �������� ����� 2 ������
//      signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
//  var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

//  ����� ��������� ����� �������. �� ������������ � ���� ������� � ������� json, ������� �������� ��� ��������:
//  access_token - ���������� ����� � username - email �������������������� ������������
//  var response = new { access_token = encodedJwt, username = person.Email }; return Results.Json(response);

//  ��� ���� �������� ����� - "/data" ���������� ������� Authorize, ������� ��� ��������� � ��� ���������� � ������� ����������
//  ���������� jwt-�����.
//  /app.Map("/data", [Authorize] (HttpContext context) => $"Hello World!");

//  �������� ������� �� javascript
//  ������ ��������� ������ ��� ������������ ����������� � ������� ������. ����, � ���� ���������� ���������� �����������
//  ����������� ������ �� ���������:
//  app.UseDefaultFiles();
//  app.UseStaticFiles();

//  � �������� ���-�������� �� ��������� ������� � ������ ��� ����������� ������ ����� wwwroot, � � ��� - ����� ���� index.html:

//<!DOCTYPE html>
//<html>
//<head>
//����<meta charset="utf-8" />
//����<title>METANIT.COM</title>
//</head>
//<body>
//����<div id="userInfo" style="display:none;">
//��������<p>����� ���������� <span id="userName"></span>!</p>
//��������<input type="button" value="�����" id="logOut" />
//����</div>
//����<div id="loginForm">
//��������<h3>���� �� ����</h3>
//��������<p>
//������������<label>������� email</label><br />
//������������<input type="email" id="email" />
//��������</p>
//��������<p>
//������������<label>������� ������</label><br />
//������������<input type="password" id="password" />
//��������</p>
//��������<input type="submit" id="submitLogin" value="�����" />
//����</div>
//����<p>
//��������<input type="submit" id="getData" value="�������� ������" />
//����</p>
//����<script>
//��������var tokenKey = "accessToken";
//��������// ��� ������� �� ������ �������� ����� ���� ������ � /login ��� ��������� ������
//��������document.getElementById("submitLogin").addEventListener("click", async e => {
//������������e.preventDefault();
//������������// ���������� ������ � �������� �����
//������������const response = await fetch("/login", {
//����������������method: "POST",
//����������������headers: { "Accept": "application/json", "Content-Type": "application/json" },
//����������������body: JSON.stringify({
//��������������������email: document.getElementById("email").value,
//��������������������password: document.getElementById("password").value
//����������������})
//������������});
//������������// ���� ������ ������ ���������
//������������if (response.ok === true) {
//����������������// �������� ������
//����������������const data = await response.json();
//����������������// �������� ���������� � ��������� ������ �� ��������
//����������������document.getElementById("userName").innerText = data.username;
//����������������document.getElementById("userInfo").style.display = "block";
//����������������document.getElementById("loginForm").style.display = "none";
//����������������// ��������� � ��������� sessionStorage ����� �������
//����������������sessionStorage.setItem(tokenKey, data.access_token);
//������������}
//������������else� // ���� ��������� ������, �������� ��� �������
//����������������console.log("Status: ", response.status);
//��������});

//��������// ������ ��� ��������� �� ���� "/data" ��� ��������� ������
//��������document.getElementById("getData").addEventListener("click", async e => {
//������������e.preventDefault();
//������������// �������� ����� �� sessionStorage
//������������const token = sessionStorage.getItem(tokenKey);
//������������// ���������� ������ � "/data
//������������const response = await fetch("/data", {
//����������������method: "GET",
//����������������headers: {
//��������������������"Accept": "application/json",
//��������������������"Authorization": "Bearer " + token� // �������� ������ � ���������
//����������������}
//������������});

//������������if (response.ok === true) {
//����������������const data = await response.json();
//����������������alert(data.message);
//������������}
//������������else
//����������������console.log("Status: ", response.status);
//��������});

//��������// �������� ����� - ������ ������� ����� � ������ ��������� ������
//��������document.getElementById("logOut").addEventListener("click", e => {

//������������e.preventDefault();
//������������document.getElementById("userName").innerText = "";
//������������document.getElementById("userInfo").style.display = "none";
//������������document.getElementById("loginForm").style.display = "block";
//������������sessionStorage.removeItem(tokenKey);
//��������});
//����</script>
//</body>
//</html>

//  ������ ���� �� �������� ������� ���������� � �������� ������������ � ������ ��� ������. ������ ���� �������� ����� ��� ������.

//  ����� ������� ������ �� ����� ������ ������ ����� ������������ ������� POST �� ����� "/login". �������� �����, ������� ��������
//  �� ��������� POST-�������� �� ����� ��������, ���� �������� ���������� email � ������, �������� � ����� �����.

//  ������� ������� � ������ ������� �������������� ����� �������� ��������� ������:

//{
//    access_token : "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93
//                    cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoicXdlcnR5IiwiaHR0cDovL3NjaGVtYXMub
//                    Wljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoidXNlciIsIm5iZi
//                    I6MTQ4MTYzOTMxMSwiZXhwIjoxNDgxNjM5MzcxLCJpc3MiOiJNeUF1dGhTZXJ2ZXIiLCJhdWQiOiJ
//                    odHRwOi8vbG9jYWxob3N0OjUxODg0LyJ9.dQJF6pALUZW3wGBANy_tCwk5_NR0TVBwgnxRbblp5Ho",
//    username: "tom@gmail.com"
//}

//  �������� access_token ��� ��� � ����� ������������ ����� �������. ����� � ������� ���������� �������������� ���������� �
//  ���� ������������.

//  ��� ����, ����� � ���� js ������ ����� � ���������� ��� ��������, �� �� ����������� � ��������� sessionStorage.

//  �������������� ������ � id="getData" �� �������� ������������� ��� ������������ ����������� � ������� ������. �� �� �������
//  ����� ����������� ������ �� ������ "/data", ��� ������� � �������� ���������� ���� �������������������. ����� ��������� �����
//  � �������, ��� ����� ��������� � ������� ��������� Authorization:

//headers:
//{
//    "Accept": "application/json",
//    "Authorization": "Bearer " + token  // �������� ������ � ���������
//}

//  �������� ������ � ������ ������ ������ �� ������������, ������� ���� � ������ people
//  ��� ����� ���������� ������ ����� ������� ������� ������ � jwt-������� � ������� ������������. � ����� ����� �� ����� ������ ��
//  ������ "�������� ������" � ��� ����� ���������� � ������� "/data", ��� ������� � �������� ��������� �����.
//  � �� �� ����� ���� �� ��������� ���������� � ����� �� ������� ��� ������ ��� � ������� � �������� ������, �� ������� ������
//  401 (Unauthorized).
