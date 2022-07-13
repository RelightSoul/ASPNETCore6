//  �������� ��������� ����� � Results API

//  ������� ��������� ������������� ��������� � ����� �� ������ �����-���� ��������� ���. ��������, ���� ������������
//  �������� �������� ������ � �������, ������� ���������� ��� ��� �������� � ������������ ���� ����. ���� ������ ����������
//  ��������� ������������ � ������� ���������� ���� �� �������� ���������� ��������. � ��� ����� Results API ������������� ��� �������.

#region StatusCode
//  ����� StatusCode() ��������� ��������� ����� ��������� ���, �������� ��� �������� ���������� � ����� � �������� ���������:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/", () => Results.StatusCode(401));
//app.Map("/", () => "Hello World");

//app.Run();

//  �������� ������� �� ����� ������� ������� ����� ������ ��������� ���. �� ��� ��������� ��������� ��������� ����� ������� ����
//  ��������� ������.
#endregion

#region ����� NotFound
//  ����� NotFound() �������� ��� 404, ��������� ������� � ���, ��� ������ �� ������. � �������� ��������� � ����� ����� ��������
//  ��������� ������ ��� �������� �������, ��������, ��������� �� ������:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/about", () => Results.NotFound(new { messsage = "Resource not found"}));
//app.Map("/contacts", () => Results.NotFound("Error 404. Ivalid address"));
//app.Map("/", () => "Hello World");

//app.Run();

//  ����� ��������, ��� ������� ������� �� ��������� ������������� � ������ json.
#endregion

#region Unauthorized
//  ����� Unauthorized() �������� ��� 401, ��������� ������������, ��� �� �� ����������� ��� ������� � �������:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/contacts", () => Results.Unauthorized());
//app.Map("/", () => "Hello World");

//app.Run();
#endregion

#region BadRequest
//  ����� BadRequest() �������� ��� 400, ������� ������� � ���, ��� ������ ������������. � �������� ��������� ����� ��������
//  ��������� ������ ��� �������� �������:

//var builder = WebApplication.CreateBuilder();
//var app = builder.Build();

//app.Map("/contacts/{age:int}", (int age) =>
//{
//    if (age < 18)
//        return Results.BadRequest(new { message = "Invalid age" });
//    else
//        return Results.Content("Access is available");
//});
//app.Map("/", () => "Hello World");

//app.Run();

//  � ������ ������ ���� �������� �������� age ������ 18, �� �������� ������� ��������� ��� 400 � ���������� "Invalid age".
//  ����� �������� ��������� ����������� ���������
#endregion

#region Ok
//  ����� Ok() �������� ��������� ��� 200, ��������� �� �������� ���������� �������. � �������� ��������� ����� ���������
//  ������������ ����������:

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Map("/about", () => Results.Ok("Laudate omnes gentes laudate"));
app.Map("/contacts", () => Results.Ok(new { message = "Success!" }));
app.Map("/", () => "Hello World");


app.Run();
#endregion